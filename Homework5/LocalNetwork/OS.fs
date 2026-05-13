module OS

open System

type OS = 
    | Windows
    | Linux
    | MacOS
    | Other of int

    member os.DefaultProbability =
        match os with
        | Windows -> 50
        | Linux -> 40
        | MacOS -> 60
        | Other p -> p