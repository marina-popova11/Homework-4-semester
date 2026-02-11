open System

let rec factorial x=
    if x > 0 then if x = 1 then 1 else x * factorial (x - 1) 
    else failwith "Error: negative number"

printfn "Enter the number."
printfn "If you want to leave enter \"exit\"."

let mutable flag = true
while flag do
    printfn "Enter the number:"
    let input = Console.ReadLine()
    if input.ToLower() = "exit" then flag <- false
    else
        try
            let number = Int32.Parse(input)
            let answer = factorial number
            printfn "%d" answer
        with ex -> printfn "%s" ex.Message