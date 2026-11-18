namespace Demo2

module Function =
    open Microsoft.Azure.Functions.Worker
    open Microsoft.AspNetCore.Http
    open Microsoft.Extensions.Logging
    open System.Threading.Tasks
    open Giraffe

    [<Function "Giraffe">]
    let run ([<HttpTrigger (AuthorizationLevel.Anonymous, Route = "{*any}")>]
        req : HttpRequest) =
        let log = req.HttpContext.GetLogger()

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