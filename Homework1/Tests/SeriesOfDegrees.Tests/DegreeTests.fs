module SeriesOfDegrees.Tests

open NUnit.Framework
open Degree

[<Test>]
let Test_ReturnsErrorForNegativeN () =
    let result = degree (-1) 5
    match result with
    | Error msg -> Assert.That(msg, Is.EqualTo("n should be non-negative"))
    | Ok _ -> Assert.Fail("Expected Error")

[<Test>]
let Test_ReturnsErrorForNegativeM () =
    let result = degree 5 (-1)
    match result with
    | Error msg -> Assert.That(msg, Is.EqualTo("m should be non-negative"))
    | Ok _ -> Assert.Fail("Expected Error")

[<Test>]
let Test_ComputesCorrect () =
    let expected = [4; 8; 16]
    match degree 2 2 with
    | Ok result -> Assert.That(result, Is.EqualTo(expected :> obj))
    | Error msg -> Assert.Fail($"Unexpected error: {msg}")

[<Test>]
let Test_MIsZero () =
    match degree 3 0 with
    | Ok result -> Assert.That(result, Is.EqualTo([8] :> obj))
    | Error msg -> Assert.Fail($"Unexpected error: {msg}")