module ParseTree

open System

type Expr =
    | Number of int
    | Add of Expr * Expr
    | Multiply of Expr * Expr
    | Subtraction of Expr * Expr
    | Division of Expr * Expr

let rec compute expr = 
    match expr with
    | Number n -> n
    | Add (l, r) -> compute l + compute r
    | Multiply (l, r) -> compute l * compute r
    | Subtraction (l, r) -> compute l - compute r
    | Division (l, r) ->
        let divisor = compute r
        if divisor = 0 then failwith "You can't divide by zero!"
        else
            compute l / divisor

let tokenize (s: string) =
    s.Replace(" ", "")
        .Replace("+", " + ")
        .Replace("-", " - ")
        .Replace("*", " * ")
        .Replace("/", " / ")
        .Replace("(", " ( ")
        .Replace(")", " ) ")
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList

let parse (seq: string list) =
    let rec calculate seq = parseFirst seq
    and parseFirst seq = 
        let rec loop acc seq =
            match seq with
            | "+" :: rest ->
                let (r, rest2) = parseSecond rest
                loop (Add(acc, r)) rest2
            | "-" :: rest ->
                let (r, rest2) = parseSecond rest
                loop (Subtraction(acc, r)) rest2
            | _ -> (acc, seq)
        let (l ,rest) = parseSecond seq
        loop l rest
    and parseSecond seq =
        let rec loop acc seq =
            match seq with
            | "*" :: rest ->
                let (r, rest2) = parseThird rest
                loop (Multiply(acc, r)) rest2
            | "/" :: rest ->
                let (r, rest2) = parseThird rest
                loop (Division(acc, r)) rest2
            | _ -> (acc,seq)
        let (l, rest) = parseThird seq
        loop l rest
    and parseThird seq =
        match seq with
        | [] -> failwith "Unexpected end of input"
        | "(" :: tail ->
            let (l, rest) = calculate tail
            match rest with
            | ")" :: rest2 -> (l, rest2)
            | _ -> failwith "Expected ')'"
        | head :: tail ->
            match Int32.TryParse(head) with
            | (true, number) -> (Number number, tail)
            | (false, _) -> failwith "Unexpected: {head}"
    let (expr, rest) = calculate seq
    expr
