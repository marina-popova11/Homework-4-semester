module Interpreter

type LambdaTerm =
    | Var of string
    | App of LambdaTerm * LambdaTerm
    | Abs of string * LambdaTerm

let newVar (usedVars: Set<string>) (pref: string) =
    let rec gen n =
        let name = pref + string n
        if Set.contains name usedVars then gen (n + 1)
        else name
    gen 0

let rec free term =
    match term with
    | Var x -> Set.singleton x
    | App (f, g) -> Set.union (free f) (free g)
    | Abs (x, body) -> Set.remove x (free body)

let rec alfaConvert oldVar newVar term =
    match term with
    | Var x when x = oldVar -> Var newVar
    | Var _ -> term
    | App (f, g) -> App (alfaConvert oldVar newVar f, alfaConvert oldVar newVar g)
    | Abs (x, body) when x = oldVar -> Abs (newVar, alfaConvert oldVar newVar body)
    | Abs (x, body) -> Abs (x, alfaConvert oldVar newVar body)

let rec replace x t s =
    match t with
    | Var y when y = x -> s 
    | Var _ -> t
    | App (f, g) ->
        App (replace x f s, replace x g s)
    | Abs (y, body) when x = y -> Abs (y, body)
    | Abs (y, body) ->
        if Set.contains y (free s) then
            let allVars = Set.union (free body) (free s)
            let newY = newVar allVars "v"
            let convertedBody = alfaConvert y newY body
            Abs (newY, replace x convertedBody s)
        else
            Abs (y, replace x body s)

let rec reduce term =
    match term with
    | Var v -> term
    | App (Abs (y, body), arg) -> replace y body arg 
    | App (f, arg) -> 
        let stepF = reduce f
        if f <> stepF then App (stepF, arg)
        else
            let stepArg = reduce arg
            if arg <> stepArg then App (f, stepArg)
            else
                term
    | Abs (y, body) ->
        let stepB = reduce body
        Abs (y, stepB)