module Factorial

let rec factorial x =
    let rec loop acc n =
        if n <= 0 then acc
        else
            loop (acc * n) (n - 1)
    loop 1 x