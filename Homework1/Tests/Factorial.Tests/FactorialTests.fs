module Factorial.Tests

open NUnit.Framework
open Factorial

[<Test>]
let Test_ReturnsValidNumber () =
    Assert.That(120, Is.EqualTo(factorial 5))

[<Test>]
let Test_ReturnForZero () =
    Assert.That(1, Is.EqualTo(factorial 0))