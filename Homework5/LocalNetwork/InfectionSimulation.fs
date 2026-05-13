module InfectionSimulation

open System
open Computer
open Network

let rnd = new Random()

let chooseRandom number =
    rnd.Next(number)

let start (network: Network) (maxSteps: int) =
    let number = chooseRandom network.Size
    network.Infect(number)
    network.PrintStatus()
    let mutable numberSteps = 0
    let mutable flag = true
    while flag && numberSteps < maxSteps do
        numberSteps <- numberSteps + 1
        let step = network.Step()
        network.PrintStatus()
        flag <- network.CanContinueInfection()
    numberSteps