namespace Demo2

[<RequireQualifiedAccess>]
module Views =
    open Giraffe.GiraffeViewEngine

    let private metaCharset (value : string) =
        meta [ attr "charset" value ]

    let private meta (key : string) (value : string) =
        meta [ attr "name" key; attr "content" value ]

    let title (pageTitle : string) =
        title [] [ sprintf "%s - Giraffe Azure Functions Demo" pageTitle |> encodedText ]

    let private master
        (pageTitle : string)
        (content   : XmlNode list) =
        html [] [
            head [] [
                yield metaCharset "utf-8"
                yield meta "description" "Giraffe Azure Functions Demo"
                yield meta "viewport"    "width=device-width, initial-scale=1.0"
                yield title pageTitle
            ]
            body [] content
        ]

    let notFound =
        [
            h1 [] [ rawText "Page not found" ]
            p [] [ rawText "Sorry, the requested page could not be found." ]
        ] |> master "Page not found"

    let index =
        [
            h1 [] [ rawText "Giraffe Azure Functions Demo" ]
            p [] [ rawText "Welcome to the this demo!" ]
        ] |> master "Home"

    let helloWorld (firstName : string) (lastName : string) =
        [
            h1 [] [ rawText "Hello World" ]
            p [] [ encodedText (sprintf "Hi %s %s, hope you like this demo!" firstName lastName) ]
        ] |> master "Hello World"