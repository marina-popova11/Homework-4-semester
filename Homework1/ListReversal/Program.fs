open System

let reverse list =
    let rec loop acc lst =
        match lst with
        | [] -> acc
        | x :: xs -> loop (x :: acc) xs
    loop [] list 

let readArrayFromConsole() =
    printfn "Enter the symbols in array"
    let input = Console.ReadLine()
    input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList

let list = readArrayFromConsole()
let newList= reverse list
printfn "%A" newList