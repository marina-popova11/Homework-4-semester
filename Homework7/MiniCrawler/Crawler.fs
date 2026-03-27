module Crawler

open System
open System.Net.Http
open System.Text.RegularExpressions

let downloadOnePageAsync (url: string) =
    async {
        let client = new HttpClient()
        try
            let! html = client.GetStringAsync(url) |> Async.AwaitTask
            return html
        with
        | ex ->
            printfn $"The error: {ex.Message}!"
            return ""
    }

let extractLink (html: string) =
    let pattern = @"<a\s+(?:[^>]*?\s+)?href=""(http://[^""]*)""|<a\s+(?:[^>]*?\s+)?href='([^']*)'"
    Regex.Matches(html, pattern)
    |> Seq.cast<Match> |> Seq.choose (fun x ->
        let urlGroup = x.Groups.["url"]
        if urlGroup.Success && not (String.IsNullOrEmpty(urlGroup.Value)) then
            Some urlGroup.Value
        else
            None
        )
    |> Seq.distinct |> Seq.toList
    
let processPageAsync (link: string) =
    async {
        let! data = downloadOnePageAsync link
        let numberOfChars = data.Length
        printfn $"{link} - {numberOfChars}"
    }

let parallelDownload (link: string) =
    processPageAsync link
    Async.Parallel()