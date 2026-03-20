module Program

open CalculateStrings

[<EntryPoint>]
let main _ =
    let calculate = CalculateBuilder()
    let result = calculate {
        let! x = "1"
        let! y = "Z"
        let z = x + y
        return z
    }

    match result with
    | Some value ->
        printfn $"The result of calculating: {value}"
    | None ->
        printfn "There is no result, there may be a mismatch of types."
    0