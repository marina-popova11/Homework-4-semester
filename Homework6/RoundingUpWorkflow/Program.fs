module Program

open System
open RoundingUp

[<EntryPoint>]
let main _ =
    printfn "Enter the accuracy to rounding up: "
    let givenAcc = Console.ReadLine()
    match Int32.TryParse(givenAcc) with
    | (true, s) ->
        let rounding = RoundingBuilder(s)
        let withRounding = rounding {
            let! a = 2.0 / 12.0
            let! b = 3.5
            return a / b
        }

        printfn $"Result rounded to {s} places: {withRounding}"
    | (false, _) ->
        printfn "Enter the number!"
    0    