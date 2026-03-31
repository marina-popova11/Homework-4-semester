module SeriesOfDegrees.Tests

open NUnit.Framework
open Degree

[<Test>]
let Test_ReturnsCorrectWithNegativeN () =
    let expected = [0.5; 1; 2; 4; 8; 16] 
    match degree (-1) 5 with
    | Ok result -> Assert.That(result, Is.EqualTo(expected :> obj))
    | Error msg -> Assert.Fail($"Unexpected error: {msg}")

[<Test>]
let Test_ReturnsErrorForNegativeM () =
    let result = degree 5 (-1)
    match result with
    | Error msg -> Assert.That(msg, Is.EqualTo("m should be non-negative"))
    | Ok _ -> Assert.Fail("Expected Error")

[<Test>]
let Test_ComputesCorrect () =
    let expected = [4.0; 8.0; 16.0]
    match degree 2 2 with
    | Ok result -> Assert.That(result, Is.EqualTo(expected :> obj))
    | Error msg -> Assert.Fail($"Unexpected error: {msg}")

[<Test>]
let Test_MIsZero () =
    match degree 3 0 with
    | Ok result -> Assert.That(result, Is.EqualTo([8.0] :> obj))
    | Error msg -> Assert.Fail($"Unexpected error: {msg}")