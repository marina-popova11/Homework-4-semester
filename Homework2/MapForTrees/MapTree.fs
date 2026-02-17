module MapTree

type Tree<'a> =
    | Node of 'a * Tree<'a> * Tree<'a>
    | Leaf

let rec map f tree =
    match tree with
    | Node(value, left, right) -> Node(f value, map f left, map f right)
    | Leaf -> Leaf