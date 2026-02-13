module Factorial

let rec factorial x =
    if x < 0 then failwith "Error: negative number"
    elif x = 0 || x = 1 then 1I
    else
        bigint x * factorial (x - 1)