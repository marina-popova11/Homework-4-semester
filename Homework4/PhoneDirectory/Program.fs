module Program

open Directory
open DirectoryUI

[<EntryPoint>]
let main _ =
    printOptions ()
    options emptyDatabase
    0