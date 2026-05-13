module Program

open System
open OS
open Computer
open Network
open InfectionSimulation

let computers = [|
    Computer(0, Windows)
    Computer(1, Linux)
    Computer(2, Windows)
    Computer(3, MacOS)
    Computer(4, Linux)
    Computer(5, Windows)
|]

for comp in computers do
    printfn "PC %d: %A (Probability: %d%%)" comp.ID comp.Os comp.ProbOfInfection
let connections = [
    (0, 1)
    (1, 2)
    (2, 3)
    (3, 4)
    (4, 5)
    (0, 2)
    (1, 3)
]
let network = Network(computers, connections)

printfn "Start:"
let maxSteps = 100
let totalSteps = start network maxSteps
let mutable infectedCount = 0
printfn "Final status: "
network.PrintStatus()
printfn "Total steps: %d" totalSteps