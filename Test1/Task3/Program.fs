// <copyright file="Program.fs" company="_">
// Marina Popova, 2026, under MIT License.
// </copyright>

module Program

open HashTableClass

[<EntryPoint>]
let main _ =
    let hashFunc (x: int) = x % 31
    let table = new HashTable<int>(hashFunc, 10)
    table.Add(190)
    table.Add(4)
    table.Add(42)

    printfn "%b" (table.Belongs(42))
    printfn "%b" (table.Belongs(100))

    printfn "%b" (table.Delete(4))
    printfn "%b" (table.Delete(1))
    0