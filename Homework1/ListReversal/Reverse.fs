module Reverse

let reverse list =
    let rec stepReverse acc lst =
        match lst with
        | [] -> acc
        | x :: xs -> stepReverse (x :: acc) xs
    stepReverse [] list 