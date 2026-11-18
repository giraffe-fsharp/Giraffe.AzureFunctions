namespace Demo2

module Startup =
    open Giraffe
    open Microsoft.Extensions.Hosting

    [<EntryPoint>]
    let main _ =
        let host =
            HostBuilder()
                .ConfigureFunctionsWebApplication()
                .ConfigureServices(fun services -> services.AddGiraffe() |> ignore)
                .Build()
        host.Run()
        0
