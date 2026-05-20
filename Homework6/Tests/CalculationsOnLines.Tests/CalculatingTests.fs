module CalculationsOnLines.Tests

open NUnit.Framework
open StringCalculationBuilder

let calculate = StringCalculationBuilder()

[<Test>]
let Test_AdditionWithValidStrings () =
    let result = calculate {
        let! x = "1"
        let! y = "2"
        return x + y
    }
    Assert.That(Some 3, Is.EqualTo(result))
    
[<Test>]
let Test_MultiplicationWithValidStrings () =
    let result = calculate {
        let! a = "6"
        let! b = "7"
        return a * b
    }
    Assert.That(Some 42, Is.EqualTo(result))

[<Test>]
let Test_ComplexCalculation () =
    let result = calculate {
        let! a = "10"
        let! b = "5"
        let! c = "2"
        return (a + b) * c
    }
    Assert.That(Some 30, Is.EqualTo(result))

[<Test>]
let test_CalculationWithNegativeNumbers () =
    let result = calculate {
        let! x = "-5"
        let! y = "3"
        return x + y
    }
    Assert.That(Some -2, Is.EqualTo(result))

[<Test>]
let Test_ReturnsNoneWhenStringIsInvalid () =
    let result = calculate {
        let! x = "1"
        let! y = "z"
        return x + y
    }
    Assert.That(None, Is.EqualTo(result))

[<Test>]
let Test_CalculationWithZero () =
    let result = calculate {
        let! a = "10"
        let! b = "0"
        return! safeDivide a b
    }
    Assert.That(None, Is.EqualTo(result))