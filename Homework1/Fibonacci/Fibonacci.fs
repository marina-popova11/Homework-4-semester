// namespace FibonacciLib

module Fibonacci
let rec fibonacci n =
    if n < 0 then None
    else
        let rec loop i a b  = 
            if i = n then Some a
            else
                loop (i + 1) b (a + b)
        loop 0 0 1