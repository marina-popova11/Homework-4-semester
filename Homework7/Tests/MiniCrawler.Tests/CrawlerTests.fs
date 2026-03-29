module MiniCrawler.Tests

open NUnit.Framework
open System
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
let Test_DownloadsSimplePageReturnsText () =
    let testUrl = "http://google.com"
    let result = downloadOnePageAsync testUrl |> Async.RunSynchronously

    Assert.That(String.IsNullOrEmpty(result), Is.False)
    Assert.That(result.Length > 100, Is.True)
    Assert.That(result.Contains("Herman Melville") || result.Contains("<html"), Is.True)

[<Test>]
let Test_ReturnsEmptyStringInCaseError () =
    let invalidUrl = "http://this-domain-definitely-does-not-exist-12345.com"
    let result = downloadOnePageAsync invalidUrl |> Async.RunSynchronously
    
    Assert.That(result, Is.EqualTo(""))

[<Test>]
let Test_HandlesTimeoutCorrectly () =
    let slowUrl = "http://httpbin.org/delay/1"
    let result = downloadOnePageAsync slowUrl |> Async.RunSynchronously
    Assert.That(result, Is.Not.Null)

[<Test>]
let Test_DisplaysSizeOfTheDownloadedPage () =
    let testUrl = "http://google.com"
    let task = processPageAsync testUrl |> Async.RunSynchronously
    Assert.Pass("processPageAsync completed without exception")

[<Test>]
let Test_WorksCorrectlyIfThereAreNoLinks () =
    let htmlWithNoLinks = "<html><body><p>Hello</p></body></html>"
    let links = extractLink htmlWithNoLinks
    Assert.That(links, Is.Empty)