module Program

open System
open ParseTree

[<EntryPoint>]
let main _ =
    printfn "Enter the expression: "
    let seq = Console.ReadLine()
    try
        let tokens = tokenize seq
        let expr = parse tokens
        let result = compute expr
        printfn "Result: %d" result
    with
        | ex -> printfn "Error: %s" ex.Message

    0