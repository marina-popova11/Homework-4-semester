module Program

open System
open Degree

[<EntryPoint>]
let main _ = 
    let readInt (prompt: string) : Option<int> =
        printf "%s" prompt
        let input = Console.ReadLine()
        match Int32.TryParse(input) with
        | (true, value) -> Some value
        | (false, _) -> None

    printfn "Enter the numbers: n - initial degree, m - number of degrees that will be added to n."
    match readInt "n = ", readInt "m = " with
    | Some n, Some m ->
        match degree n m with
        | Ok list -> printfn "%A" list
        | Error msg -> printfn "Error: %s" msg
    | None, _ -> printfn "Error: invalid input for n"
    | _, None -> printfn "Error: invalid input for m"

    0
