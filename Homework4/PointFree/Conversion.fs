module Conversion

let initialFunction x (lst: int list) =
    List.map (fun y -> x * y) lst

let multiply x y = y * x

let map f = List.map (f)

let conversion = multiply >> map