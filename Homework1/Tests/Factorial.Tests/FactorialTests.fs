module Factorial.Tests

open NUnit.Framework
open Factorial

[<Test>]
let Test_ReturnsValidNumber () =
    Assert.That(bigint 120, Is.EqualTo(factorial 5))

[<Test>]
let Test_ReturnForZero () =
    Assert.That(bigint 1, Is.EqualTo(factorial 0))

[<Test>]
let Test_ThrowsException () =
    Assert.Throws<System.Exception>(fun () -> factorial (-4) |> ignore)
        |> fun ex -> Assert.That(ex.Message, Is.EqualTo("Error: negative number"))