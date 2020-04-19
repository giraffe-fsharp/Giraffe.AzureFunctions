namespace Demo2

module Startup =
    open Microsoft.Azure.Functions.Extensions.DependencyInjection
    open Giraffe

    type AppStartup () =
      inherit FunctionsStartup ()

      override __.Configure (builder : IFunctionsHostBuilder) =
        builder.Services.AddGiraffe()
        |> ignore

    [<assembly: FunctionsStartup(typeof<AppStartup>)>]
    do ()