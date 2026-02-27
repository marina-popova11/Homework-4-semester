module Conversation

let multiply x y = y * x

let map f = List.map (f)

let conversation = // : int x -> int l -> int l =
    multiply >> map