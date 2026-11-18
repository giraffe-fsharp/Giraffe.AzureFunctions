# Giraffe.AzureFunctions

A simple example of using Giraffe with Azure Functions

## Requirements and build

- Azure Functions runtime/Core Tools **4**, using the **.NET isolated worker**
  with ASP.NET Core integration (not the old in-process .NET Core 3.1 host).
- .NET 8 runtime/targeting pack and .NET 10 SDK (also used by the local Paket tool).

Run `sh ./build.sh` to restore the pinned Paket tool and dependency lock,
publish both demos to `build/demo` and `build/demo2`, and run the HTTP adapter
integration tests. The tests exercise the real function entry points, including
DotLiquid rendering, JSON binding, HTML views, logging, errors, and not-found
responses. GitHub Actions runs the same command for pull requests.

The demos use the latest published Giraffe 8.3.0, Giraffe.ViewEngine
2.0.0-alpha-1, and Giraffe.DotLiquid 3.0.0-rc-1, including prereleases.
Explicit Giraffe/ViewEngine constraints prevent the older DotLiquid package's
minimum dependency from selecting an old framework. The rendered-template test
also checks that the published DotLiquid binary works with current Giraffe.
Paket uses minimum compatible transitive dependencies rather than selecting
future major versions of the worker's Microsoft.Extensions dependencies.

## Run and deploy

Install Azure Functions Core Tools 4, then run `sh ./startLocal.sh` (or
`sh ./startLocal.sh demo2`). For the first demo, browse to `/api/demo`,
`/api/demo/foo`, or `/api/demo/person`; for demo2, try `/api/ping` or
`/api/hello/Foo/Bar`.

When deploying, use a Functions v4 **isolated .NET 8** Function App with
`FUNCTIONS_WORKER_RUNTIME=dotnet-isolated`, `FUNCTIONS_EXTENSION_VERSION=~4`,
and the .NET 8 isolated stack (`DOTNET-ISOLATED|8.0` on Linux).
Deploy one demo's publish directory per Function App. Configure
`AzureWebJobsStorage` for your deployment as usual; local settings are not
published.
