module InfectionSimulation

open System
open NetworkSystem

let rnd = new Random()

let chooseRandom number =
    rnd.Next(number)

let start (network: Network) =
    let number = chooseRandom network.Size
    network.Infect(number)
    printfn "Initial infection: computer %d" number
    let mutable numberSteps = 0
    let mutable flag = true
    while flag do
        let step = network.Step()
        if not step then flag <- false
        else numberSteps <- numberSteps + 1
    numberSteps