module PointFree.Tests

open FsCheck
open FsCheck.NUnit
open Conversion

[<NUnit.Framework.TestFixture>]
type PointFreeTests() =
    [<Property>]
    let Test_FunctionsAreEquivalent (x: int) (lst: int list) =
        let actualFunctionResult = conversion x lst
        let result = initialFunction x lst
        actualFunctionResult = result