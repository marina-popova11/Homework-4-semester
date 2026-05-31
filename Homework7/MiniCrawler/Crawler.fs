module Crawler

open System
open System.Net
open System.Net.Http
open System.Text.RegularExpressions
open System.Threading

type DownloadFunction = HttpClient -> string -> Async<string option>

let downloadOnePageAsync (client: HttpClient) (url: string) =
    async {
        try
            let! html = client.GetStringAsync(url) |> Async.AwaitTask
            return Some html
        with
        | ex ->
            printfn $"The error: {ex.Message}!"
            return None
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
    
let processPageAsync (downloadFn: DownloadFunction) (client: HttpClient) (link: string) =
    async {
        let! http = downloadFn client link
        match http with
        | Some data ->
            let numberOfChars = data.Length
            printfn $"{link} - {numberOfChars}"
            return Some numberOfChars
        | None -> return None
    }

let parallelDownload (downloadFn: DownloadFunction) (client: HttpClient) (link: string) =
    async {
        let! html = downloadFn client link
        match html with
        | Some data ->
            let links = extractLink data
            printfn $"Links found: {links.Length}"
            let allTasks =  links |> List.map (processPageAsync downloadFn client)
            let! results = allTasks |> Async.Parallel
            let successfulResult = results |> Array.choose id
            printfn $"Successfully processed {successfulResult.Length} out of {links.Length} links"
            return successfulResult
        | None ->
            printfn $"Failed to download initial page: {link}"
            return [||] 
    }

let createHttpClient () =
    let client = new HttpClient()
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36")
    client.Timeout <- TimeSpan.FromSeconds(30.0)
    client