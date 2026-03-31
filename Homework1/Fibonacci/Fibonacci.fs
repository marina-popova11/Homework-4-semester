module Fibonacci
let fibonacci n  =
    match n with
    | n when n < 0 -> None
    | 0 -> Some 0
    | 1 -> Some 1
    | _ -> 
        let rec stepFib i prev cur  = 
            if i = n then Some prev
            else
                stepFib (i + 1) cur (prev + cur)
        stepFib 0 0 1
        