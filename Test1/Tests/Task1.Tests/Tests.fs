module Task1.Tests

open NUnit.Framework
open Search

[<Test>]
let Test_SimpleSearch () =
    let list = [3; 4; 7; 5; 2; 7; 10]
    let result = search list
    Assert.That(result, Is.EqualTo(Some 2))

[<Test>]
let Test_SearchWithNegative () =
    let list = [-3; 4; -7; 5; 2; -7; -10]
    let result = search list
    Assert.That(result, Is.EqualTo(Some -10))

[<Test>]
let Test_EmptySearch () =
    let list = []
    let result = search list
    Assert.That(result, Is.EqualTo(None))
