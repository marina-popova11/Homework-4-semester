module RoundingUp

open System

type RoundingBuilder(n: int) =
    member this.Bind(x: float, f) =
        f x
    member this.Return(x: float) =
        let number = System.Math.Pow(10, n)
        System.Math.Round(x * number) / number