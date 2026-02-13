module Program

open System
// open FibonacciLib
open Fibonacci

[<EntryPoint>]
let main _ =
    printfn "Enter the number: "
    try
        let number = Int32.Parse(Console.ReadLine())
        let result = fibonacci number
        match result with
        | None -> failwith "The number should be more than zero!"
        | Some a -> printfn "Fibonacci number with index %d - %d" number a
    with ex -> printfn "%s" ex.Message
    0