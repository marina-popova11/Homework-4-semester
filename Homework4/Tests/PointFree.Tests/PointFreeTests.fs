module PointFree.Tests

open FsCheck
open FsCheck.NUnit
open Conversation

[<NUnit.Framework.TestFixture>]
type PointFreeTests() =
    let initialFunction x (lst: int list) =
        List.map (fun y -> x * y) lst

    [<Property>]
    let Test_FunctionsAreEquivalent (x: int) (lst: int list) =
        let actualFunctionResult = conversation x lst
        let result = initialFunction x lst
        actualFunctionResult = result