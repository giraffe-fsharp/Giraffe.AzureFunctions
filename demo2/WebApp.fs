namespace Demo2

[<RequireQualifiedAccess>]
module WebApp =
    open Microsoft.Extensions.Logging
    open Giraffe

    let routes : HttpHandler =
        choose [
            GET  >=> route  "/api"     >=> HttpHandlers.index
            GET  >=> route  "/api/ping" >=> text "pong"
            GET  >=> routef "/api/hello/%s/%s" HttpHandlers.helloWorld
            POST >=> route  "/api/hello" >=> HttpHandlers.helloWorldApi
            setStatusCode 404 >=> HttpHandlers.notFound
        ]

    let errorHandler (ex : exn) (logger : ILogger) =
        logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
        clearResponse
        >=> ServerErrors.INTERNAL_ERROR ex.Message