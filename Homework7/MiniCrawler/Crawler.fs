module Crawler

open System
open System.Net
open System.Net.Http
open System.Text.RegularExpressions
open System.Threading

let downloadOnePageAsync (url: string) =
    async {
        let client = new HttpClient()
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36")
        try
            let! html = client.GetStringAsync(url) |> Async.AwaitTask
            return html
        with
        | ex ->
            printfn $"The error: {ex.Message}!"
            return ""
    }

let extractLink (html: string) =
    let pattern = @"<a\s+(?:[^>]*?\s+)?href=""(?<url>https?://[^""]*)""|<a\s+(?:[^>]*?\s+)?href='(?<url>[^']*)'"
    Regex.Matches(html, pattern)
    |> Seq.cast<Match> |> Seq.choose (fun x ->
        let urlGroup = x.Groups.["url"]
        if urlGroup.Success && not (String.IsNullOrEmpty(urlGroup.Value)) then
            let decodedUrl = WebUtility.HtmlDecode(urlGroup.Value)
            Some decodedUrl
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
    async {
        let! html = downloadOnePageAsync link
        let links = extractLink html
        printfn $"Links found: {links.Length}"
        let allTasks =  links |> List.map processPageAsync
        do! allTasks |> Async.Parallel |> Async.Ignore
    }