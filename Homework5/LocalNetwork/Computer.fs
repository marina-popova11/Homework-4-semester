module Computer

open System
open OS

let rnd = Random(DateTime.Now.Millisecond)

type Computer (id: int, os: OS, ?probability: int) =
    let prob =
        match probability with
        | Some p -> p
        | None -> os.DefaultProbability

    member c.ID = id
    member c.Os = os
    member c.Probability = prob
    member val IsInfected = false with get, set
    member c.ProbOfInfection = prob

    member c.TryInfect() =
        if c.IsInfected then false
        else
            let prob = rnd.Next(100)
            let success = prob < c.ProbOfInfection
            if success then c.IsInfected <- true
            success