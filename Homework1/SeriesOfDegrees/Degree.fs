module Degree

let degree n m =
    if n < 0 then Error "n should be non-negative"
    elif m < 0 then Error "m should be non-negative"
    else
        let first = 1 <<< n
        let result = List.init (m + 1) (fun i -> first * (1 <<< i))
        Ok result