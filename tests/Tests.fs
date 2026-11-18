module Tests

open System
open System.IO
open System.Net
open System.Net.Http
open System.Text
open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection
open Xunit

let server demo =
    let root = Path.GetFullPath(Path.Combine(__SOURCE_DIRECTORY__, "..", demo))
    let invoke = if demo = "demo" then Run.run else Demo2.Function.run
    let builder =
        WebHostBuilder()
            .UseContentRoot(root)
            .ConfigureServices(fun services ->
                services.AddLogging() |> ignore
                services.AddGiraffe() |> ignore)
            .Configure(fun app ->
                app.Run(RequestDelegate(fun ctx ->
                    let result = invoke ctx.Request
                    result.ExecuteResultAsync(ActionContext(HttpContext = ctx)))))
    new TestServer(builder)

[<Theory>]
[<InlineData("demo", "/api/demo", 200, "<html")>]
[<InlineData("demo", "/api/demo/foo", 200, "Foo")>]
[<InlineData("demo", "/api/demo/person", 200, "Foo")>]
[<InlineData("demo", "/api/demo/logging", 200, "ok")>]
[<InlineData("demo", "/api/demo/failing", 500, "FAILURE")>]
[<InlineData("demo", "/missing", 404, "Not Found")>]
[<InlineData("demo2", "/api", 200, "Giraffe Azure Functions Demo")>]
[<InlineData("demo2", "/api/ping", 200, "pong")>]
[<InlineData("demo2", "/api/hello/Foo/Bar", 200, "Hi Foo Bar")>]
[<InlineData("demo2", "/missing", 404, "Page not found")>]
let ``Functions adapters execute handlers with current published packages`` (demo: string, path: string, status: int, expected: string) =
    task {
        use host = server demo
        use client = host.CreateClient()
        use! response = client.GetAsync(path)
        let! body = response.Content.ReadAsStringAsync()
        Assert.Equal(enum<HttpStatusCode> status, response.StatusCode)
        Assert.Contains(expected, body)
    }

[<Fact>]
let ``Functions adapter binds a JSON request body`` () =
    task {
        use host = server "demo2"
        use client = host.CreateClient()
        use content = new StringContent("""{"firstName":"Foo","lastName":"Bar"}""", Encoding.UTF8, "application/json")
        use! response = client.PostAsync("/api/hello", content)
        let! body = response.Content.ReadAsStringAsync()
        Assert.Equal(HttpStatusCode.OK, response.StatusCode)
        Assert.Contains("Hi Foo Bar", body)
    }
