// <copyright file="Tests.fs" company="_">
// Marina Popova, 2026, under MIT License.
// </copyright>

module Task3.Tests

open NUnit.Framework
open HashTableClass

let intHash (x: int) = x

[<Test>]
let Test_AddAndContains () =
    let table = new HashTable<int>(intHash, 10)
    table.Add(42)
    Assert.That(table.Belongs(42), Is.True)

[<Test>]
let Test_DeleteExistingElement () =
    let table = new HashTable<int>(intHash, 10)
    table.Add(42)
    Assert.That(table.Delete(42), Is.True)

[<Test>]
let Test_DeleteNotExistingElement () =
    let table = new HashTable<int>(intHash, 10)
    table.Add(42)
    Assert.That(table.Delete(10), Is.False)
