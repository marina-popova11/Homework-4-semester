module SearchForNumber.Tests

open NUnit.Framework
open Search

[<Test>]
let Test_ReturnsNone () =
    let result = search 5 []
    Assert.That(result, Is.EqualTo(None))

[<Test>]
let Test_FindsElementAtIndexZero () =
    let result = search 10 [10; 20; 30]
    Assert.That(result, Is.EqualTo(Some 0))

[<Test>]
let Test_ReturnsNoneWhenElementNotFound () =
    let result = search 10 [1; 2; 3; 5]
    Assert.That(result, Is.EqualTo(None))

[<Test>]
let Test_WorksWithNegativeNumbers () =
    let result = search (-3) [1; -3; 5]
    Assert.That(result, Is.EqualTo(Some 1))