module Program

open System
open Crawler

[<EntryPoint>]
let main _ =
    async {
        use client = createHttpClient()
        let! results = parallelDownload downloadOnePageAsync client "https://google.com"
        printfn $"Total characters from all pages: {results |> Array.sum}"
    } |> Async.RunSynchronously
    0