module MiniCrawler.Tests

open NUnit.Framework
open System
open System.Net.Http
open Crawler

[<Test>]
let Test_ExtractsLinksInSingleQuotes () =
    let html = """<a href='https://test.ru/path'>Link</a>"""
    let result = extractLink html
    Assert.That(List.length result, Is.EqualTo(1))
    Assert.That(result.[0], Is.EqualTo("https://test.ru/path"))

[<Test>]
let Test_ExtractsMultipleLinks () =
    let html = """
        <a href="http://site1.com">One</a>
        <a href="https://site2.ru">Two</a>
        <a href='http://site3.org/path'>Three</a>
    """
    let result = extractLink html
    Assert.That(List.length result, Is.EqualTo(3))
    Assert.That(result |> List.contains "http://site1.com", Is.True)
    Assert.That(result |> List.contains "https://site2.ru", Is.True)
    Assert.That(result |> List.contains "http://site3.org/path", Is.True)

[<Test>]
let Test_DecodesHTMLEntitiesInURL () =
    let html = """<a href="https://example.com/search?q=hello&amp;lang=ru">Link</a>"""
    let result = extractLink html
    Assert.That(List.length result, Is.EqualTo(1))
    Assert.That( result.[0], Is.EqualTo("https://example.com/search?q=hello&lang=ru"))

[<Test>]
let Test_ReturnsEmptyList () =
    let html = """<p>No links here</p>"""
    let result = extractLink html
    Assert.That(result, Is.Empty)

[<Test>]
let Test_RemovesDuplicateLinks () =
    let html = """
        <a href="http://duplicate.com">First</a>
        <a href="http://duplicate.com">Second</a>
        <a href="http://unique.com">Unique</a>
    """
    let result = extractLink html
    Assert.That(List.length result, Is.EqualTo(2))
    Assert.That(result |> List.contains "http://duplicate.com", Is.True)
    Assert.That(result |> List.contains "http://unique.com", Is.True)

type DownloadFunction = HttpClient -> string -> Async<string option>

let createMockDownloader (pages: Map<string, string>) : DownloadFunction =
    fun (client: HttpClient) (url: string) ->
        async {
            do! Async.Sleep(10)
            
            match pages.TryFind url with
            | Some content -> return Some content
            | None -> 
                printfn $"[MOCK] Page not found: {url}"
                return None
        }

[<Test>]
let Test_ProcessPageAsync_ReturnsSizeForValidPage () =
    async {
        let mockPages = Map.ofList [
            "http://test.com/page1", "<html><body>Hello World</body></html>"
        ]
        
        use client = new HttpClient()
        let mockDownload = createMockDownloader mockPages
        
        let! result = processPageAsync mockDownload client "http://test.com/page1"
        
        Assert.That(result.IsSome, Is.True)
        match result with
        | Some size ->
            Assert.That(size, Is.EqualTo(37))
        | None -> Assert.Fail("Expected Some result")
    } |> Async.RunSynchronously

[<Test>]
let Test_ProcessPageAsync_ReturnsNoneForInvalidPage () =
    async {
        let mockPages = Map.empty<string, string>
        
        use client = new HttpClient()
        let mockDownload = createMockDownloader mockPages
        
        let! result = processPageAsync mockDownload client "http://nonexistent.com"
        Assert.That(result.IsNone, Is.True)
    } |> Async.RunSynchronously

[<Test>]
let Test_ParallelDownload_ProcessesAllLinksAndReturnsResults () =
    async {
        let mockPages = Map.ofList [
            "http://main.com", """
                <html>
                <a href="http://page1.com">Page 1</a>
                <a href="http://page2.com">Page 2</a>
                <a href="http://page1.com">Duplicate</a>
                </html>
            """
            "http://page1.com", "<html>Content of page 1</html>"
            "http://page2.com", "<html>Page 2 content here</html>"
        ]
        
        use client = new HttpClient()
        let mockDownload = createMockDownloader mockPages
        
        let! results = parallelDownload mockDownload client "http://main.com"
        
        Assert.That(results.Length, Is.EqualTo(2))
        Assert.That(results |> Array.forall (fun size -> size > 0), Is.True)
    } |> Async.RunSynchronously

[<Test>]
let Test_ParallelDownload_HandlesMissingPages () =
    async {
        let mockPages = Map.ofList [
            "http://main.com", """
                <html>
                <a href="http://exists.com">Exists</a>
                <a href="http://missing.com">Missing</a>
                </html>
            """
            "http://exists.com", "<html>Existing page</html>"
        ]
        
        use client = new HttpClient()
        let mockDownload = createMockDownloader mockPages
        
        let! results = parallelDownload mockDownload client "http://main.com"
        
        Assert.That(results.Length, Is.EqualTo(1))
        Assert.That(results.[0], Is.EqualTo(26))
    } |> Async.RunSynchronously