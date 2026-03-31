module Search

let search target list =
    let rec find index lst = 
        match lst with
        | [] -> None
        | x :: _ when x = target -> Some index
        | _ :: xs -> find (index + 1) xs
    find 0 list