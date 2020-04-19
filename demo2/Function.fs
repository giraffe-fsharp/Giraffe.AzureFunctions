namespace Demo2

module Function =
    open Microsoft.Azure.WebJobs
    open Microsoft.Azure.WebJobs.Extensions.Http
    open Microsoft.AspNetCore.Http
    open Microsoft.Extensions.Logging
    open System.Threading.Tasks
    open FSharp.Control.Tasks.V2
    open Giraffe

    [<FunctionName "Giraffe">]
    let run ([<HttpTrigger (AuthorizationLevel.Anonymous, Route = "{*any}")>]
        req : HttpRequest, ectx : ExecutionContext, log : ILogger) =

        let env = req.HttpContext.GetHostingEnvironment()
        env.ContentRootPath <- ectx.FunctionAppDirectory

        let func = Some >> Task.FromResult

        { new Microsoft.AspNetCore.Mvc.IActionResult with
            member _.ExecuteResultAsync(ctx) =
                task {
                    try
                        return! WebApp.routes func ctx.HttpContext :> Task
                    with exn ->
                        return! WebApp.errorHandler exn log func ctx.HttpContext :> Task
                }
                :> Task }