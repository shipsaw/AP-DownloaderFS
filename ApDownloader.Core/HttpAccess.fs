module HttpAccess

open System.Net
open FSharp.Data

[<Literal>]
let domain = "https://www.armstrongpowerhouse.com"

[<Literal>]
let loginUrl =
    "https://www.armstrongpowerhouse.com/index.php?route=account/login"

let login username password =
    let cookieContainer = CookieContainer()

    let _ =
        Http.Request(
            loginUrl,
            body =
                FormValues [ "email", $"{username}"
                             "password", $"{password}" ],
            cookieContainer = cookieContainer
        )

    cookieContainer

(*
    let downloads =
        Http.RequestString(
            @"https://www.armstrongpowerhouse.com/index.php?route=account/download",
            cookieContainer = cookieContainer
        )

    printfn "%s" downloads
*)
