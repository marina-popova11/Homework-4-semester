module Program

open MapTree

[<EntryPoint>]
let main _ =
    let tree = Node(1, Node(2, Leaf, Node(4, Leaf, Leaf)), Node(3, Leaf, Leaf))
    let newTree = map (fun x -> x * 2) tree
    printfn "%A" newTree
    0