open System

let degree n m =
    if n < 0 then failwith "Error: n should be more than zero"
    if m < 0 then failwith "Error: m should be more than zero"
    else
        let first = 1 <<< n
        List.init(m + 1) (fun i -> first * (1 <<< i))

let main = 

    printfn "Enter the numbers: n - initial degree, m - number of degrees that will be added to n."
    try
        let n = Int32.Parse(Console.ReadLine())
        let m = Int32.Parse(Console.ReadLine())
        let list = degree n m
        printfn "%A" list
    with ex-> printfn "%s" ex.Message
