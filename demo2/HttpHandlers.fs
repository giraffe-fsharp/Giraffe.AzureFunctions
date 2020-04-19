namespace Demo2

[<RequireQualifiedAccess>]
module HttpHandlers =
    open Microsoft.AspNetCore.Http
    open Giraffe
    open FSharp.Control.Tasks.V2.ContextInsensitive

    let index : HttpHandler =
        Views.index
        |> htmlView

    let notFound : HttpHandler =
        Views.notFound
        |> htmlView

    let helloWorld (firstName : string, lastName : string) : HttpHandler =
        Views.helloWorld firstName lastName
        |> htmlView

    let helloWorldApi : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! person = ctx.BindJsonAsync<Models.Person>()
                let response =
                    sprintf "Hi %s %s, hope you enjoy this demo!"
                        person.FirstName
                        person.LastName
                    |> text
                return! response next ctx
            }