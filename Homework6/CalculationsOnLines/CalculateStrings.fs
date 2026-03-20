module CalculateStrings

open System

type CalculateBuilder() =
    member this.Bind(x: string, f) =
        match Int32.TryParse(x) with
        | (true, s) ->
            f s
        | (false, _) ->
            None
    member this.Return(x: int) : Option<int> = Some x