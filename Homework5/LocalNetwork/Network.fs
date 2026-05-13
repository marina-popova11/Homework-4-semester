module Network

open System
open Computer

type Network (computers: Computer[], connections: (int * int) list) =
    let size = computers.Length
    let mutable matrix = Array2D.zeroCreate<bool> size size
    let mutable currentStep = 0
    do
        for (id1, id2) in connections do
            if id1 >= 0 && id2 >= 0 && id1 < size && id2 < size then
                matrix.[id1, id2] <- true
                matrix.[id2, id1] <- true
            else
                failwith $"Invalid indexes: {id1}, {id2}"

    member n.Size = size
    member n.Matrix = matrix
    member n.AddLink (comp1: Computer, comp2: Computer) =
        let firstID = comp1.ID
        let secID = comp2.ID
        if firstID >= 0 && secID >= 0 && firstID < size && secID < size then
            matrix.[firstID,secID] <- true
            matrix.[secID,firstID] <- true
        else failwith "Invalid indexes!"

    member n.Infect(index: int) =
        computers.[index].IsInfected <- true

    member n.AreConnect(comp1: Computer, comp2: Computer) =
        matrix.[comp1.ID, comp2.ID]

    member n.GetNeighbors (comp: Computer) =
        [0 .. size - 1] |> List.filter(fun i -> matrix.[comp.ID, i]) |> List.map(fun i -> computers.[i])

    member n.Step() =
        currentStep <- currentStep + 1
        let mutable newlyInfected : int list = []
        let infected =
            computers |> Array.filter (fun c -> c.IsInfected)
        
        let neighborsOfInfected =
            infected |> Array.collect (fun comp -> n.GetNeighbors(comp) |> List.toArray)

        let potentialInfected =
            neighborsOfInfected |> Array.distinct |> Array.filter (fun c -> not c.IsInfected)

        for computer in potentialInfected do
            let attempt = computer.TryInfect()
            if attempt then
                newlyInfected <- computer.ID :: newlyInfected
        
        newlyInfected.Length > 0

    member n.CurrentStep = currentStep
    member n.PrintStatus() =
        printfn $"Step: {currentStep}"
        computers |> Array.iter (fun comp ->
            let status = if comp.IsInfected then "Infected" else "Healthy"
            printfn $"PC: {comp.ID}, {comp.Os}, {status}"
        )

    member n.CanContinueInfection () =
        let infected =
            computers |> Array.filter (fun comp -> comp.IsInfected)
        infected |> Array.exists (fun comp ->
            let neighbors = n.GetNeighbors(comp)
            neighbors |> List.exists (fun neighbor ->
                not neighbor.IsInfected && neighbor.Probability > 0))
    
    member n.InfectedNumber = computers |> Array.filter (fun comp -> comp.IsInfected) |> Array.length