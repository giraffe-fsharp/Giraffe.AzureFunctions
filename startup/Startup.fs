module Startup

open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Hosting
open Giraffe

type WebJobsExtensionStartup() =
  interface IWebJobsStartup with
    member __.Configure (builder : IWebJobsBuilder) =
      builder.Services.AddGiraffe() |> ignore

[<assembly: WebJobsStartup(typeof<WebJobsExtensionStartup>)>] do()