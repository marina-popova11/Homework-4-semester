module Program

open Directory
open DirectoryUI

[<EntryPoint>]
let main _ =
    printOptions ()
    workCycle emptyDatabase
    0