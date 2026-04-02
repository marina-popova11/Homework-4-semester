// <copyright file="HashTableClass.fs" company="_">
// Marina Popova, 2026, under MIT License.
// </copyright>

module HashTableClass

type HashTable<'a when 'a : equality> (hashFunc : 'a -> int, size : int) =
    do
        if size <= 0 then 
            invalidArg "size" "Size must be positive"

    // An array of lists
    let mutable lists : list<'a> array = Array.create size [] 

    // Retrieves the index of an element using a hash function
    let getIndex (element: 'a) =
        abs (hashFunc element) % size
 
    // Adds an element
    member h.Add (value: 'a) =
        let index = getIndex value
        if not (lists[index] |> List.exists (fun x -> x = value)) then
            lists[index] <- value :: lists[index]

    // Checks if an element is in the hash table
    member h.Belongs (value: 'a) : bool =
        let index = getIndex value
        lists[index] |> List.exists (fun x -> x = value)

    // Deletes an element
    member h.Delete (value: 'a) : bool=
        let index = getIndex value
        let element = lists[index]
        if lists[index] |> List.exists (fun x -> x = value) then
            lists[index] <- element |> List.filter (fun x -> x <> value)
            true
        else
            false
