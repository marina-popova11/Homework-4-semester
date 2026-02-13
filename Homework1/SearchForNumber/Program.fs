module Program

open System
open Search
    
[<EntryPoint>]
let main _ =
    let readArrayFromConsole() =
        printfn "Enter the numbers in array"
        let input = Console.ReadLine()
        input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        |> Array.choose (fun s ->
            match Int32.TryParse(s) with
            | (true, n) -> Some n
            | (false, _) -> 
                printfn "Warning: '%s' is not a number and will be skipped." s
                None)
        |> Array.toList

    let list= readArrayFromConsole()
    printfn "Enter the number for search: "
    let number = Console.ReadLine()
    match Int32.TryParse(number) with
    | (true, value) -> 
        let position = search value list
        match position with
            | None -> printfn "Element not found"
            | Some index -> printfn "Index: %d" index
    | (false, _) -> printfn "Invalid input"
    0