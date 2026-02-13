module Reverse

let reverse list =
    let rec loop acc lst =
        match lst with
        | [] -> acc
        | x :: xs -> loop (x :: acc) xs
    loop [] list 