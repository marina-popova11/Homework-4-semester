module CheckingNumber

let isValid (seq: string) =
    let parenthesis =
        Map [
        (')', '(')
        (']', '[')
        ('}', '{')
        ]

    let openParenthesis = Set ['('; '['; '{']

    let rec check (stack: char list) (buffer: char list) =
        match buffer with
        | [] ->
            match stack with
            | [] -> true
            | _ -> false
        | head :: tail ->
            if parenthesis.ContainsKey head then
                match stack with
                | [] -> false
                | next :: rest ->
                    let current = parenthesis.[head]
                    if current = next then
                        check rest tail
                    else
                        false
            elif openParenthesis.Contains head then
                check (head :: stack) tail
            else
                check stack tail

    check [] (List.ofSeq seq)