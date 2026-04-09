module ParenthesisSequence.Tests

open NUnit.Framework
open SequenceCorrectness

[<Test>]
let Test_WithCorrectSeq () =
    let seq = "(ggfhkkw(hhee[fnnrr]))"
    let result = hasCorrectParentheses(seq)
    Assert.That(result, Is.True)

[<Test>]
let Test_WithOnlyBrackets () =
    let seq = "([({})]{[])"
    let result = hasCorrectParentheses(seq)
    Assert.That(result, Is.False)

[<Test>]
[<TestCase("()()", ExpectedResult = true)>]
[<TestCase("[][][]", ExpectedResult = true)>]
[<TestCase("{}{}{}", ExpectedResult = true)>]
[<TestCase("()[]{}", ExpectedResult = true)>]
let Test_RepeatedPairsShouldBeValid (input: string) =
    hasCorrectParentheses input