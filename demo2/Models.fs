namespace Demo2

[<RequireQualifiedAccess>]
module Models =

    [<CLIMutable>]
    type Person =
        {
            FirstName   : string
            LastName    : string
        }