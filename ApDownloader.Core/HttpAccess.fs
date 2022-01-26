module HttpAccess

open System.Net

let printWebsiteLogin =
    let cookieContainer = CookieContainer()

    let test =
        Http.Request(
            @"https://www.armstrongpowerhouse.com/index.php?route=account/login",
            body =
                FormValues [ "email", "USER_EMAIL"
                             "password", "USER_PASSWORD" ]
        )

    test.Cookies
    |> Map.iter (fun k v -> cookieContainer.Add(Cookie(k, v, "/", "www.armstrongpowerhouse.com")))

    let downloads =
        Http.RequestString(
            @"https://www.armstrongpowerhouse.com/index.php?route=account/download",
            cookieContainer = cookieContainer
        )

    printfn "%s" downloads
