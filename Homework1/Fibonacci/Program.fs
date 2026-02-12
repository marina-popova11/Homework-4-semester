open System

let rec fibonacci n =
    if n < 0 then None
    else
        let rec loop i a b  = 
            if i = n then Some a
            else
                loop (i + 1) b (a + b)
        loop 0 0 1

printfn "Enter the number: "
try
    let number = Int32.Parse(Console.ReadLine())
    let result = fibonacci number
    match result with
    | None -> failwith "The number should be more than zero!"
    | Some a -> printfn "Fibonacci number with index %d - %d" number a
with ex -> printfn "%s" ex.Message