module PrimeNumbers.Tests

open NUnit.Framework
open PrimeSequence

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test_Create10 () =
    let expected = [2; 3; 5; 7; 11; 13; 17; 19; 23; 29]
    let result = createPrime |> Seq.take 10 |> Seq.toList
    Assert.That(expected, Is.EqualTo(result :> obj))

[<Test>]
let Test_For2 () =
    let expected = [2]
    let result = createPrime |> Seq.take 1 |> Seq.toList
    Assert.That(expected, Is.EqualTo(result :> obj))

[<Test>]
let Test_Take100Element () =
    let result = createPrime |> Seq.item 100
    Assert.That(547, Is.EqualTo(result))