// <copyright file="Program.fs" company="_">
// Marina Popova, 2026, under MIT License.
// </copyright>

module Program

open PrintStars

[<EntryPoint>]
let main _ =
    let r = printfn "%s" (createLines 4)
    0