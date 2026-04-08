module SequenceCorrectness

let hasCorrectParentheses (seq: string) =
    let parenthesis =
        Map [
        (')', '(')
        (']', '[')
        ('}', '{')
        ]

    let openParentheses = Set ['('; '['; '{']

    let rec check (stack: char list) (buffer: char list) =
        match (buffer, stack) with
        | ([], []) -> true
        | ([], _ :: _) -> false
        | (next :: rest, _) when parenthesis.ContainsKey next ->
            match stack with
            | [] -> false
            | head :: tail when parenthesis.[next] = head ->
                check tail rest
            | _ -> false
        | (next :: rest, stack) when openParentheses.Contains next ->
            check (next :: stack) rest
        | (_ :: rest ,stack) -> check stack rest

    check [] (List.ofSeq seq)