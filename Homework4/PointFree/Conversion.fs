module Conversion

let initialFunction x (lst: int list) =
    List.map (fun y -> x * y) lst

let func1 x = List.map (fun y -> x * y)

let func2 x = List.map ((*) x)

let func3 = (*) >> List.map

let conversion = func3