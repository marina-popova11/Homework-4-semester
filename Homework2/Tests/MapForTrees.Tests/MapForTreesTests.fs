module MapForTrees.Tests

open NUnit.Framework
open MapTree

[<Test>]
let Test_MapOnEmptyTree () =
    let tree: int Tree = Leaf
    let result = map (fun x -> x + 1) tree
    Assert.That(result, Is.EqualTo(Leaf : int Tree))

[<Test>]
let Test_MapWithSingleNode () =
    let tree: int Tree = Node (1, Leaf, Leaf)
    let expected = Node (5, Leaf, Leaf)
    Assert.That(expected, Is.EqualTo(map (fun x -> x * 5) tree))

[<Test>]
let Test_MapWithDeeperTree () =
    let tree: int Tree = 
        Node (1, Node (3, Node (6, Leaf, Leaf), Node (9, Leaf, Leaf)), Node (4, Node (10, Leaf, Leaf), Leaf))
    let expected =
        Node (false, Node (false, Node (true, Leaf, Leaf), Node (false, Leaf, Leaf)), Node (true, Node (true, Leaf, Leaf), Leaf))
    Assert.That(expected, Is.EqualTo(map (fun x -> x % 2 = 0) tree))