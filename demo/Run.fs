module Run

open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Giraffe.DotLiquid
open Microsoft.Extensions.Logging

[<CLIMutable>] //For XML serialization
type Person =
  { FirstName : string
    LastName  : string }

let fooBar = { FirstName = "Foo"; LastName = "Bar" }

let app : HttpHandler =

  choose [

    GET >=> route "/api/demo" >=> htmlFile "index.htm"

    GET >=> route "/api/demo/foo" >=> negotiate fooBar

    GET >=> route "/api/demo/person" >=> dotLiquidTemplate "text/html" "Templates/Person.liquid" fooBar

  ]

[<FunctionName "Giraffe">]
let run ([<HttpTrigger (AuthorizationLevel.Anonymous, Route = "{*any}")>] req : HttpRequest, context : ExecutionContext, log : ILogger) =
  let hostingEnvironment = req.HttpContext.GetHostingEnvironment()
  hostingEnvironment.ContentRootPath <- context.FunctionAppDirectory
  let func = Some >> Task.FromResult
  { new Microsoft.AspNetCore.Mvc.IActionResult with
      member _.ExecuteResultAsync(ctx) = 
        app func ctx.HttpContext :> Task }
