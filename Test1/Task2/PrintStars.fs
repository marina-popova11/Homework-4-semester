module PrintStars

// Creates a list of lines, and then connects everything into a string using \n
let createLines n =
    match n with
    | x when x <= 0 -> "None"
    | _ ->
        let lines = [0 .. n - 1] |> List.map (fun i ->
            match i with
            | i when i = 0 || i = n - 1 ->
                String.replicate n "*"
            | _ -> "*" + String.replicate (n - 2) " " + "*")
        String.concat "\n" lines
            