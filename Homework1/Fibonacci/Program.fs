module Program

open System
open Fibonacci

[<EntryPoint>]
let main _ =
    printfn "Enter the number: "
    let input = Console.ReadLine()
    match Int32.TryParse(input) with
    | (true, number) ->
        let result = fibonacci number
        match result with
        | None -> failwith "The number should be more than zero!"
        | Some a -> printfn "Fibonacci number with index %d - %d" number a
    | (false, _) ->
        printfn "Error: Invalid number format."
    0