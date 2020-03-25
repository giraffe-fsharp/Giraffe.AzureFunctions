module Startup

open Microsoft.Azure.Functions.Extensions.DependencyInjection
open Giraffe

type WebJobsExtensionStartup () =
   inherit FunctionsStartup ()
   override __.Configure(builder: IFunctionsHostBuilder) =
       builder.Services.AddGiraffe() |> ignore

[<assembly: FunctionsStartup(typeof<WebJobsExtensionStartup>)>]
do ()
