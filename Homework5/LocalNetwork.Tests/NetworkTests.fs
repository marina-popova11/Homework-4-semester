module LocalNetwork.Tests

open NUnit.Framework
open NetworkSystem
open InfectionSimulation

[<Test>]
let Test_Probability100 () =
    let computers = [|
        Computer(0, "Windows", 100)
        Computer(1, "MacOs", 100)
        Computer(2, "Linux", 100)
        Computer(3, "Windows", 100)
    |]

    let network = Network(computers)
    network.Add(computers.[0], computers.[1])
    network.Add(computers.[1], computers.[2])
    network.Add(computers.[2], computers.[3])
    network.Infect(0)
    let mutable steps = 0
    let mutable flag = true
    while flag do
        steps <- steps + 1
        flag <- network.Step()

    let allInfected = computers |> Array.forall(fun x -> x.IsInfected)
    Assert.That(allInfected, Is.True)

[<Test>]
let Test_Probability0 () =
    let computers = [|
        Computer(0, "Windows", 0)
        Computer(1, "MacOs", 0)
        Computer(2, "Linux", 0)
    |]

    let network = Network(computers)
    network.Add(computers.[0], computers.[1])
    network.Add(computers.[1], computers.[2])
    network.Infect(0)
    let mutable steps = 0
    let mutable flag = true
    while flag && steps < 10 do
        steps <- steps + 1
        flag <- network.Step()

    let onlyFirstInfected = 
        computers.[0].IsInfected && 
        not computers.[1].IsInfected && 
        not computers.[2].IsInfected

    Assert.That(onlyFirstInfected, Is.True)

[<Test>]
let Test_IsolatedComputer () =
    let computers = [|
        Computer(0, "Windows", 100)
        Computer(1, "MacOs", 100)
        Computer(2, "Linux", 100)
        Computer(3, "Windows", 100)
    |]

    let network = Network(computers)
    network.Add(computers.[0], computers.[1])
    network.Add(computers.[1], computers.[2])
    network.Infect(0)
    let mutable flag = true
    while flag do
        flag <- network.Step()

    let isolated = not computers.[3].IsInfected
    Assert.That(isolated, Is.True)