module LocalNetwork.Tests

open NUnit.Framework
open Computer
open Network
open InfectionSimulation

[<Test>]
let Test_Probability100 () =
    let computers = [|
        Computer(0, Windows, 100)
        Computer(1, MacOS, 100)
        Computer(2, Linux, 100)
        Computer(3, Windows, 100)
    |]

    let connections = [(0, 1); (1, 2); (2, 3)]
    let network = Network(computers, connections)
    network.Infect(0)
    ignore (start network 10)

    let allInfected = computers |> Array.forall(fun x -> x.IsInfected)
    Assert.That(allInfected, Is.True)

[<Test>]
let Test_Probability0 () =
    let computers = [|
        Computer(0, Windows, 0)
        Computer(1, MacOS, 0)
        Computer(2, Linux, 0)
    |]

    let connections = [(0, 1); (1, 2)]
    let network = Network(computers, connections)
    network.Infect(0)
    for i in 1 .. 10 do
        ignore (network.Step())
    let onlyFirstInfected = 
        computers.[0].IsInfected && 
        not computers.[1].IsInfected && 
        not computers.[2].IsInfected

    Assert.That(onlyFirstInfected, Is.True)

[<Test>]
let Test_IsolatedComputer () =
    let computers = [|
        Computer(0, Windows, 100)
        Computer(1, MacOS, 100)
        Computer(2, Linux, 100)
        Computer(3, Windows, 100)
    |]

    let connections = [(0, 1); (1, 2)]
    let network = Network(computers, connections)
    network.Infect(0)

    let isolated = not computers.[3].IsInfected
    Assert.That(isolated, Is.True)