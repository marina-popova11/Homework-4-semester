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

let displaySearchResultByName (searchElement: string) (searchResult: PhoneNumber option) =
    match searchResult with
    | Some (PhoneNumber number) ->
        $"Phone number {number} found by name {searchElement}!"
    | None ->
        $"No phone numbers were found for name {searchElement}!"

let displaySearchResultByPhone (searchElement: string) (searchResult: Name option) =
    match searchResult with
    | Some (Name name) ->
        $"Name {name} found by phone number {searchElement}!"
    | None ->
        $"No names were found for phone number {searchElement}!"

let display (database: Database) =
    if List.isEmpty database then printfn "Database is empty!"
    else
        database |> List.iter (fun x ->
            let name = x.Name
            let number = x.PhoneNumber
            printfn $"{name}: {number}")

let rec workCycle (database: Database) =
    match Console.ReadLine() with
    | null -> ()
    | input ->
        let parts = input.Trim().Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
        match parts with
        | [||] ->
            printf "Enter command: "
            workCycle database
        | [| "exit" |] ->
            ()
        | [| "add"; name; phone |] ->
            match addRecord database name phone with
            | Success (newDatabase, message) ->
                printfn $"{message}"
                workCycle newDatabase
            | Failure (message) ->
                printfn $"{message}"
                workCycle database
        | [| "add"; _ |] ->
            printfn "Usage: add <name> <phone>"
            printf "Enter command: "
            workCycle database
        | [| "find"; "by"; "name"; name |] ->
            searchByName database name 
            |> displaySearchResultByName name
            |> printfn "%s"
            printf "Enter command: "
            workCycle database
        | [| "find"; "by"; "phone"; phone |] ->
            searchByPhone database phone 
            |> displaySearchResultByPhone phone
            |> printfn "%s"
            printf "Enter command: "
            workCycle database
        | [| "display" |] ->
            display database
            printf "Enter command: "
            workCycle database
        |[| "save"; filename |] ->
            if String.IsNullOrWhiteSpace(filename) then
                printfn "Filename cannot be empty!"
                printf "Enter command: "
                workCycle database
            else
                try
                    let lines = serialize database
                    File.WriteAllLines(filename, lines)
                    printfn "Database was successfully saved to file %s!" filename
                    printf "Enter command: "
                    workCycle database
                with ex ->
                    printfn "Error: %s" ex.Message
                    printf "Enter command: "
                    workCycle database
        | [| "load"; filename |] ->
            if not (File.Exists(filename)) then
                printfn "File not found!"
                printf "Enter command: "
                workCycle database
            else
                let lines = File.ReadAllLines(filename)
                match deserialize lines with
                | Success (newDatabase, message) ->
                    printfn "%s" message
                    printf "Enter command: "
                    workCycle newDatabase
                | Failure message ->
                    printfn "%s" message
                    printf "Enter command: "
                    workCycle database
        | _ -> 
            printfn "Unknown command!"
            printf "Enter command: "
            workCycle database