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
    | Var v -> v
    | App (f, g) -> reduction 
    | Abs (term1, term2) -> reduction 