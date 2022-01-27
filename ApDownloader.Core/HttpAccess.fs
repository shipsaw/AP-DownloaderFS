module HttpAccess

open System.Net
open System.Windows.Controls
open FSharp.Data

[<Literal>]
let domain = "https://www.armstrongpowerhouse.com"

[<Literal>]
let loginUrl =
    "https://www.armstrongpowerhouse.com/index.php?route=account/login"

let AttemptLogin (username, (password: obj)) =
    let cookieContainer = CookieContainer()
    let convPassword = password :?> PasswordBox

    let response =
        Http.Request(
            loginUrl,
            body =
                FormValues [ "email", $"{username}"
                             "password", $"{convPassword.Password}" ],
            cookieContainer = cookieContainer
        )

    if response.StatusCode = (int) HttpStatusCode.Redirect then
        Some cookieContainer
    else
        None

(*
    let downloads =
        Http.RequestString(
            @"https://www.armstrongpowerhouse.com/index.php?route=account/download",
            cookieContainer = cookieContainer
        )

    printfn "%s" downloads
*)
