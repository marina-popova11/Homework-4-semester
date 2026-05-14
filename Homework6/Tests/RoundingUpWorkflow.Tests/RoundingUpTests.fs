module RoundingUpWorkflow.Tests

open NUnit.Framework
open System
open RoundingUp

[<Test>]
let Test_RoundingTo3 () =
    let rounding = RoundingBuilder(3)
    let withRounding = rounding {
        let! a = 2.0 / 12.0
        let! b = 3.5
        return a / b
    }
    Assert.That(withRounding, Is.EqualTo(0.048))

[<TestCase(-0.123456, 2, -0.12)>]
[<TestCase(-0.999, 1, -1.0)>]
let Test_RoundingWithNegativeNumbers (input: float, n: int, result: float) =
    let rounding = RoundingBuilder(n)
    let withRounding = rounding {
        return input
    }
    Assert.That(withRounding, Is.EqualTo(result))

[<Test>]
let Test_WithNegativeAccuracy_ThrowsException () =
    Assert.Throws<ArgumentOutOfRangeException>(fun () ->
        let rounding = RoundingBuilder(-2)
        ()
    ) |> ignore

[<Test>]
let Test_WithZeroAccuracy () =
    let rounding = RoundingBuilder(0)
    let withRounding = rounding {
        return 0.128
    }
    Assert.That(withRounding, Is.EqualTo(0.0))