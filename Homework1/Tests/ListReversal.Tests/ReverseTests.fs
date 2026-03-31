module ListReversal.Tests

open NUnit.Framework
open Reverse

[<Test>]
let Test_ReverseEmptyList () =
    let result = reverse []
    Assert.That(result, Is.EqualTo([] :> obj))

[<Test>]
let Test_ReverseListWithOneElement () =
    let result = reverse [42]
    Assert.That(result, Is.EqualTo([42] :> obj))

[<Test>]
let Test_ReversesListOfIntegers () =
    let input  = [1; 2; 3; 4; 5]
    let expected = [5; 4; 3; 2; 1]
    let result = reverse input
    Assert.That(result, Is.EqualTo(expected :> obj))


[<Test>]
let Test_ReverseTwiceGivesOriginalList () =
    let original = [10; 20; 30; 40]
    let reversed = reverse original
    let restored = reverse reversed
    Assert.That(restored, Is.EqualTo(original :> obj))