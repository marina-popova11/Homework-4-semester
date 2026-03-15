module Program

open System
open NetworkSystem
open InfectionSimulation

[<EntryPoint>]
let main _ =
    let computers = [|
        Computer(0, "Windows")
        Computer(1, "Linux")
        Computer(2, "Windows")
        Computer(3, "MacOS")
        Computer(4, "Linux")
        Computer(5, "Windows")
    |]

    for comp in computers do
        printfn "PC %d: %s (Probability: %d%%)" comp.ID comp.Os comp.ProbOfInfection
    let network = Network(computers)
    network.Add(computers.[0], computers.[1])
    network.Add(computers.[1], computers.[2])
    network.Add(computers.[2], computers.[3])
    network.Add(computers.[3], computers.[4])
    network.Add(computers.[4], computers.[5])
    network.Add(computers.[0], computers.[2])
    network.Add(computers.[1], computers.[3])

    printfn "Start:"
    let totalSteps = start network
    let mutable infectedCount = 0
    printfn "Final status: "
    for i in 0 .. network.Size - 1 do
        let comp = computers.[i]
        if comp.IsInfected then 
            infectedCount <- infectedCount + 1
            printfn "PC %d (%s): Infected" comp.ID comp.Os
        else
            printfn "PC %d (%s): Healthy" comp.ID comp.Os
    printfn "Total steps: %d" totalSteps

    0