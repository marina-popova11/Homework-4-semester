module RoundingBuilder

open System

type RoundingBuilder(precision: int) =
    do
        if precision < 0 then
            raise (ArgumentOutOfRangeException("precision", "Precision cannot be negative."))

    member private this.Round(x: float) =
        Math.Round(x, precision)

    member this.Bind(x: float, f: float -> float) =
        let roundedX = this.Round(x)
        let y = f roundedX
        this.Round(y)

    member this.Return(x: float) =
        this.Round(x)