module NetworkSystem

open System

let rnd = Random()

type Computer (id: int, os: string, ?probability: int) =
    let prob =
        match probability with
        | Some p -> p
        | None ->
            match os with
            | "Windows" -> 50
            | "Linux" -> 40
            | "MacOS" -> 60
            | _ -> 100
    member c.ID = id
    member c.Os = os
    member val  IsInfected = false with get, set
    member c.ProbOfInfection = prob

    member c.TryInfect() =
        if c.IsInfected then false
        else
            let prob = rnd.Next(100)
            let success = prob < c.ProbOfInfection
            if success then c.IsInfected <- true
            success

type Network (computers: Computer[]) =
    let size = computers.Length
    let mutable matrix = Array2D.zeroCreate<bool> size size
    member n.Size = size
    member n.Matrix = matrix
    member n.Add (comp1: Computer, comp2: Computer) =
        let firstID = comp1.ID
        let secID = comp2.ID
        if firstID >= 0 && secID >= 0 && firstID < size && secID < size then
            matrix.[firstID,secID] <- true
            matrix.[secID,firstID] <- true
        else failwith "Invalid indexes!"

    member n.Infect(index: int) =
        computers.[index].IsInfected <- true

    member n.IsConnect(comp1: Computer, comp2: Computer) =
        matrix.[comp1.ID, comp2.ID]

    member n.GetNeighbors (comp: Computer) =
        [0 .. size - 1] |> List.filter(fun i -> matrix.[comp.ID, i]) |> List.map(fun i -> computers.[i])

    member n.Step() =
        let mutable buffer : int list = []
        for i in 0 .. size - 1 do
            if not computers.[i].IsInfected then
                let neighbors = n.GetNeighbors(computers.[i])
                let hasInfected = neighbors |> List.exists(fun x -> x.IsInfected)
                if hasInfected then
                    let attempt = computers.[i].TryInfect()
                    if attempt then buffer <- i :: buffer
        
        buffer.Length > 0

    member n.PrintStatus(step: int) =
        printfn $"Step: {step}"
        for i in 0 .. size - 1 do
            let comp = computers.[i]
            let status = if comp.IsInfected then "Infected" else "Healthy"
            printfn $"PC: {comp.ID}, {comp.Os}, {status}"