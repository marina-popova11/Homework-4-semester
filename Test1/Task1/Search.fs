// <copyright file="Search.fs" company="_">
// Marina Popova, 2026, under MIT License.
// </copyright>

module Search

// Searches for the smallest item in the list
let search lst =
    match lst with
    | [] -> None
    | x :: xs ->
        let rec find lst minEl =
            match lst with
            | [] -> Some minEl
            | head :: tail ->
                let el = if head < minEl then head else minEl
                find tail el
        find lst x
