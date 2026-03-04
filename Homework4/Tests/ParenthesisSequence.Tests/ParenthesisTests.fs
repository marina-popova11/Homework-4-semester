module ParenthesisSequence.Tests

open NUnit.Framework
open CheckingNumber

[<Test>]
let Test_WithCorrectSeq () =
    let seq = "(ggfhkkw(hhee[fnnrr]))"
    let result = isValid(seq)
    Assert.That(result, Is.True)

[<Test>]
let Test_WithOnlyBrackets () =
    let seq = "([({})]{[])"
    let result = isValid(seq)
    Assert.That(result, Is.False)

