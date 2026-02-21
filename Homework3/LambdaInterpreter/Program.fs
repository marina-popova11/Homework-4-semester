module Program

open System
open Interpreter

[<EntryPoint>]
let main _ =
    let leftPart = App (Abs ("a", Abs ("b", App (Var "b", Var "b"))), Abs ("b", App (Var "b", Var "b")))
    let fullLeft = App(leftPart, Var "b")
    let rightPart = App (Abs ("c", App(Var "c", Var "b")), Abs("a", Var "a"))
    let startTerm = App (fullLeft, rightPart)
    let result = reduction startTerm
    printfn "Result: All were calculated"
    0
