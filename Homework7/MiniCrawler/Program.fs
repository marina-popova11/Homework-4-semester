module Program

open System
open Crawler

[<EntryPoint>]
let main _ =
    let url  = "http://google.com"
    parallelDownload url |> Async.RunSynchronously |> ignore
    printfn "All links successfully downloaded! "
    0