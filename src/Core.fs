[<AutoOpen>]
module Giraffe.AzureFunctions

open Microsoft.AspNetCore.Http
open Giraffe
open System.Threading.Tasks
open FSharp.Control.Tasks
open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Hosting

let run (req : HttpRequest) (context : ExecutionContext) (app : HttpHandler) =
  task {
    let hostingEnvironment = req.HttpContext.GetService<IHostingEnvironment>()
    hostingEnvironment.ApplicationName <- context.FunctionName
    hostingEnvironment.ContentRootPath <- context.FunctionDirectory
    let finish = Some >> Task.FromResult
    let! result = app finish req.HttpContext
    match result with
    | Some ctx -> return ctx.Response
    | _ -> return failwith "Unhandled request"
  }