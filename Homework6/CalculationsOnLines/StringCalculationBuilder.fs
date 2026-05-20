module StringCalculationBuilder

open System

let safeDivide x y = 
    if y = 0 then None 
    else Some (x / y)

type StringCalculationBuilder() =
    member this.Bind(x: string, f) =
        match Int32.TryParse(x) with
        | true, s -> f s
        | false, _ -> None
    member this.Return(x: int) : Option<int> = Some x

    member this.ReturnFrom(x: Option<int>) : Option<int> = x