module Fibonacci.Tests

open NUnit.Framework
open Fibonacci

[<Test>]
let Test_ReturnsNone () =
    let result = fibonacci (-1)
    Assert.That(None, Is.EqualTo(result))

[<Test>]
let Test_ReturnsValidNumber () =
    let result = fibonacci 10
    Assert.That(Some(55), Is.EqualTo(result))