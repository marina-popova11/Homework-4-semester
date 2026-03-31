module PrimeSequence

let isPrime n =
    match n with
    | n when n < 2 -> false
    | n when n = 2 -> true
    | n when n % 2 = 0 -> false
    | _ ->
        let border = int (sqrt (float n))
        seq { 3 .. border }
        |> Seq.forall (fun m -> n % m <> 0)       

let rec createPrime =
    seq {
        yield 2
        yield! Seq.initInfinite (fun x -> 2 * x + 3) |> Seq.filter isPrime
    }