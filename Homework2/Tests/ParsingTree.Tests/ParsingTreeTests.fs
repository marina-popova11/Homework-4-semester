module ParsingTree.Tests

open NUnit.Framework
open ParseTree

[<Test>]
let Test_ComputeSimpleNumber () =
    let number = "10"
    Assert.That(10, Is.EqualTo(number |> tokenize |> parse |> compute))

[<Test>]
let Test_ComputeAddition () =
    let seq = "1+ 2"
    Assert.That(3, Is.EqualTo(seq |> tokenize |> parse |> compute))

[<Test>]
let Test_ComputeSubtraction () =
    let seq = "10 - 4"
    Assert.That(6, Is.EqualTo(seq |> tokenize |> parse |> compute))

[<Test>]
let Test_ComputeWithBrackets () =
    let seq = "(3 + 4) * 5"
    Assert.That(35, Is.EqualTo(seq |> tokenize |> parse |> compute))

[<Test>]
let Test_ComputeFewBrackets () =
    let seq = "((2 + 3) * 4) - 5"
    Assert.That(15, Is.EqualTo(seq |> tokenize |> parse |> compute))

[<Test>]
let Test_DivisionByZeroThrowsException () =
    let seq = "2 / 0"
    Assert.Throws<System.Exception>(fun () -> seq |> tokenize |> parse |> compute |> ignore)
        |> fun ex -> Assert.That(ex.Message, Is.EqualTo("You can't divide by zero!"))

[<Test>]
let Test_IncompleteExpressionThrowsException () =
    let seq = "2 -"
    Assert.Throws<System.Exception>(fun () -> seq |> tokenize |> parse |> compute |> ignore)
        |> fun ex -> Assert.That(ex.Message, Is.EqualTo("Unexpected end of input"))

[<Test>]
let Test_UnclosedBracketsThrowsException () =
    let seq = "(2 - 2"
    Assert.Throws<System.Exception>(fun () -> seq |> tokenize |> parse |> compute |> ignore)
        |> fun ex -> Assert.That(ex.Message, Is.EqualTo("Expected ')'"))

[<Test>]
let Test_UnexpectedTokenThrowsException () =
    let seq = "qwe"
    Assert.Throws<System.Exception>(fun () -> seq |> tokenize |> parse |> compute |> ignore)
        |> fun ex -> Assert.That(ex.Message, Is.EqualTo("Unexpected: {head}"))
