module Run

open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs

let app : HttpHandler =

  choose [

    GET >=> route "/api/demo" >=> htmlFile "index.htm"

  ]

let run (req : HttpRequest, context : ExecutionContext) = AzureFunctions.run req context app