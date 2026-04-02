module Program

open PrintStars

[<EntryPoint>]
let main _ =
    let r = printfn "%s" (createLines 4)
    0