module Search

let search target list =
    let rec find index lst = 
        match lst with
        | [] -> None
        | x::xs ->
            if x = target then Some index
            else find (index + 1) xs
    find 0 list