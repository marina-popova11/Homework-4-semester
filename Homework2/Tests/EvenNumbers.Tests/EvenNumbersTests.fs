module EvenNumbers.Tests

open FsCheck
open FsCheck.NUnit
open Functions

[<Property>]
let Test_AllFunctionsAreEquivalent (lst: int list) =
    let resultFilter = countingWithFilter lst
    let resultMap = countingWithMap lst
    let resultFold = countingWithFold lst
    resultFilter = resultFold && resultFold = resultMap

[<Property>]
let Test_ResultNeverExceedsListLength (lst: int list) =
    countingWithFilter lst <= List.length lst

[<Property>]
let Test_ResultAlwaysNonNegative (lst: int list) =
    countingWithFold lst >= 0
