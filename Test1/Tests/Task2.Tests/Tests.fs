// <copyright file="Tests.fs" company="_">
// Marina Popova, 2026, under MIT License.
// </copyright>

module Task2.Tests

open NUnit.Framework
open PrintStars

[<Test>]
let Test_SimplePrint () =
    let result = createLines 1
    Assert.That(result, Is.EqualTo("*"))

[<Test>]
let Test_PrintWithTwo () =
    let result = createLines 2
    Assert.That(result, Is.EqualTo("**\n**"))

[<Test>]
let Test_PrintWithOutLines () =
    let result = createLines 0
    Assert.That(result, Is.EqualTo("None"))