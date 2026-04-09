module PointFree.Tests

open FsCheck
open FsCheck.NUnit
open Conversion

[<NUnit.Framework.TestFixture>]
type PointFreeTests() =
    [<Property>]
    let Test_InitFuncEqualsFunc1 (x: int) (lst: int list) =
        initialFunction x lst = func1 x lst

    [<Property>]
    let Test_Func1EqualsFunc2 (x: int) (lst: int list) =
        func1 x lst = func2 x lst

    [<Property>]
    let Test_Func2EqualsFunc3 (x: int) (lst: int list) =
        func2 x lst = func3 x lst