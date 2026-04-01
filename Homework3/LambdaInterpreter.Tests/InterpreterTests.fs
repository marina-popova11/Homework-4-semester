module LambdaInterpreter.Tests

open NUnit.Framework
open Interpreter

[<Test>]
let Test_SimpleReplace () =
    let expr = Var "35"
    Assert.That(Var "35", Is.EqualTo(replace "3" expr (Var "4")))

[<Test>]
let Test_SimpleApplicationReplace () =
    let expr = App (Var "x", Var "y")
    let result = replace "x" expr (Var "a")
    Assert.That(result, Is.EqualTo(App (Var "a", Var "y")))

[<Test>]
let Test_SimpleAbstractionReplace () =
    let expr = Abs ("x", Var "x")
    let result = replace "x" expr (Var "2")
    Assert.That(result, Is.EqualTo(Abs ("x", Var "x")))

[<Test>]
let Test_SimpleReduction () =
    let expr = App (Abs ("x", Var "x"), Var "y")
    let result = reduce expr
    Assert.That(result, Is.EqualTo(Var "y"))

[<Test>]
let Test_Reduction () =
    let expr = App (Abs ("x", Var "z"), Var "y")
    let result = reduce expr
    Assert.That(result, Is.EqualTo(Var "z"))

[<Test>]
let Test_LeftReduction () =
    let expr = App (App (Abs ("x", App (Var "x", Var "z")), Var "y"), Var "z")
    let result = reduce expr
    Assert.That(result, Is.EqualTo(App (App (Var "y", Var "z"), Var "z")))