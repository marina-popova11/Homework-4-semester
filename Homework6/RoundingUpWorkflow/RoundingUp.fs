module RoundingUp

open System

type RoundingBuilder(n: int) =
    do
        if n < 0 then
            raise (ArgumentOutOfRangeException("n", "Precision cannot be negative."))
    member this.Bind(x: float, f) =
        f x

    member this.Return(x: float) =
        let number = System.Math.Pow(10, n)
        System.Math.Round(x * number) / number