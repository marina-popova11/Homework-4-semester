module Degree

let degree n m =
    match n, m with
    | _, m when m < 0 -> Error "m should be non-negative"
    | n, m ->
        let rec stepDegree i acc =
            match i with
            | i when i > m -> List.rev acc
            | _ ->
                let result = 2.0 ** float (n + i)
                stepDegree (i + 1) (result :: acc)
        Ok (stepDegree 0 [])