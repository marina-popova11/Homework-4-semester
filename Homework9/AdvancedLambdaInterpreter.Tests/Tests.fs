module AdvancedLambdaInterpreter.Tests

open NUnit.Framework
open FParsec
open Interpreter
open Parser

let parseSuccess (input: string) =
    match parseString input with
    | Success (result, _, _) -> result
    | Failure (errorMsg, _, state) ->
        Assert.Fail($"Parsing failed unexpectedly: {errorMsg}")
        { Definition = []; Expression = Var "" }
let parseFailure (input: string) =
    match parseString input with
    | Success _ -> Assert.Fail("Expected parsing to fail, but it succeeded.")
    | Failure _ -> ()

[<Test>]
let Test_ParseSimpleVariable () =
    let input = "x"
    let result = parseSuccess input
    Assert.That(Var "x", Is.EqualTo(result.Expression))
    Assert.That(result.Definition, Is.Empty)

[<Test>]
let Test_ParseIdentityLambda () =
    let input = "\\x. x"
    let expected = Abs ("x", Var "x")
    let result = parseSuccess input
    Assert.That(result.Expression, Is.EqualTo(expected))

[<Test>]
let Test_ParseWithSeveralArgs () =
    let input = "\\ x y. x y"
    let expected = Abs ("x", Abs ("y", App (Var "x", Var "y")))
    let result = parseSuccess input
    Assert.That(result.Expression, Is.EqualTo(expected))