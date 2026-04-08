module DirectoryUI

open System
open System.IO
open Directory

let printOptions () =
    printfn """
- exit - Exit
- add <name> <phone> - Add new note
- find by name <name> - Search by name  
- find by phone <phone> - Search by number
- display - Display all the current contents of the database
- save <filename> - Save data in file
- load <filename> - Read data from a file
"""
    printfn "Enter the option number that you would like to use: "

let rec options (database: Database) =
    match Console.ReadLine() with
    | null -> ()
    | input ->
        let parts = input.Trim().Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
        match parts with
        | [||] ->
            printf "Enter command: "
            options database
        | [| "exit" |] ->
            ()
        | [| "add"; name; phone |] ->
            match addRecord database name phone with
            | Success (newDatabase, message) ->
                printfn $"{message}"
                options newDatabase
            | Failure (message) ->
                printfn $"{message}"
                options database
        | [| "add"; _ |] ->
            printfn "Usage: add <name> <phone>"
            printf "Enter command: "
            options database
        | [| "find"; "by"; "name"; name |] ->
            searchByName database name 
            |> displaySearchResultByName name
            |> printfn "%s"
            printf "Enter command: "
            options database
        | [| "find"; "by"; "phone"; phone |] ->
            searchByPhone database phone 
            |> displaySearchResultByPhone phone
            |> printfn "%s"
            printf "Enter command: "
            options database
        | [| "display" |] ->
            display database
            printf "Enter command: "
            options database
        |[| "save"; filename |] ->
            if String.IsNullOrWhiteSpace(filename) then
                printfn "Filename cannot be empty!"
                printf "Enter command: "
                options database
            else
                try
                    let lines = serialize database
                    File.WriteAllLines(filename, lines)
                    printfn "Database was successfully saved to file %s!" filename
                    printf "Enter command: "
                    options database
                with ex ->
                    printfn "Error: %s" ex.Message
                    printf "Enter command: "
                    options database
        | [| "load"; filename |] ->
            if not (File.Exists(filename)) then
                printfn "File not found!"
                printf "Enter command: "
                options database
            else
                let lines = File.ReadAllLines(filename)
                match deserialize lines with
                | Success (newDatabase, message) ->
                    printfn "%s" message
                    printf "Enter command: "
                    options newDatabase
                | Failure message ->
                    printfn "%s" message
                    printf "Enter command: "
                    options database
        | _ -> 
            printfn "Unknown command!"
            printf "Enter command: "
            options database