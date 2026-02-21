module Interpreter

type LambdaTerm = 
    | Var of string
    | App of LambdaTerm * LambdaTerm
    | Abs of string * LambdaTerm

let rec replace x t s =
    match t with
    | Var y -> if x = y then s else t
    | App (f, g) ->
        App (replace x f s, replace x g s)
    | Abs (y, body) ->
        if x = y then t
        else
            Abs (y, replace y s body)

let rec reduction term =
    match term with
    | Var v -> term
    | App (Abs (y, body), arg) -> replace y body arg 
    | App (f, arg) -> 
        let stepF = reduction f
        if f <> stepF then App (stepF, arg)
        else
            let stepArg = reduction arg
            if arg <> stepArg then App (f, stepArg)
            else
                term
    | Abs (y, body) ->
        let stepB = reduction body
        if body <> stepB then Abs (y, stepB)
        else
            term