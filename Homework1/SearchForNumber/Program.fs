open System

let search target list =
    let rec find index lst = 
        match lst with
        | [] -> None
        | x::xs ->
            if x = target then Some index
            else find (index + 1) xs
    find 0 list
    

let readArrayFromConsole() =
    printfn "Enter the numbers in array"
    let input = Console.ReadLine()
    input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
    |> Array.map int
    |> Array.toList

let list= readArrayFromConsole()
printfn "Enter the number for search: "
let number = Console.ReadLine()
match Int32.TryParse(number) with
| (true, value) -> 
    let position = search value list
    match position with
        | None -> printfn "Element not found"
        | Some index -> printfn "%d" index
| (false, _) -> printfn "Invalid input"