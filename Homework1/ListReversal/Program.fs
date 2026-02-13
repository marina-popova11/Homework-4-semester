module Program

open System
open Reverse

[<EntryPoint>]
let main _ =
    let readArrayFromConsole() =
        printfn "Enter the symbols in array"
        let input = Console.ReadLine()
        input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList

    let list = readArrayFromConsole()
    let newList = reverse list
    printfn "%A" newList
    0