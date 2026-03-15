module InfectionSimulation

open System
open NetworkSystem

let rnd = new Random()

let chooseRandom number =
    rnd.Next(number)

let start (network: Network) =
    let number = chooseRandom network.Size
    network.Infect(number)
    network.PrintStatus(0)
    let mutable numberSteps = 0
    let mutable flag = true
    while flag do
        numberSteps <- numberSteps + 1
        let step = network.Step()
        network.PrintStatus(numberSteps)
        if not step then flag <- false
    numberSteps