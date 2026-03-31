module Programs

open System
open Factorial

[<EntryPoint>]
let main _ =
    printfn "Enter the number."
    printfn "If you want to leave enter \"exit\"."

    let mutable flag = true
    while flag do
        printfn "Enter the number:"
        let input = Console.ReadLine()
        if input.ToLower() = "exit" then flag <- false
        else
            match Int32.TryParse(input) with
            | (true, number) ->
                try
                    let answer = factorial number
                    printfn "%A" answer
                with ex -> printfn "%s" ex.Message
            | (false, _) ->
                printfn "Error: Invalid number format."
    0