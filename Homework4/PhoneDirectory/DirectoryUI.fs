module DirectoryUI

open System
open System.IO
open Directory

let printOptions () =
    printfn """
0 - Exit,
1 - Add new note (Name, number),
2 - Search by name,
3 - Search by number,
4 - Display all the current contents of the database,
5 - Save data in file,
6 - Read data from a file.
"""
    printfn "Enter the option number that you would like to use: "

let rec options (database: Database) =
    printOptions ()
    match Console.ReadLine() with
    | "0" -> ()
    | "1" ->
        printfn "Enter the name and phone number through 'enter': "
        let name = Console.ReadLine()
        let number = Console.ReadLine()
        match addNote database name number with
        | Success (newDatabase, message) ->
            printfn $"{message}"
            options newDatabase
        | Failure (message) ->
            printfn $"{message}"
            options database        
    | "2" -> 
        printfn "Enter the name to search phone number: "
        let name = Console.ReadLine()
        let message = search database "name" name
        printfn $"{message}"
        options database
    | "3" ->
        printfn "Enter the phone number to search phone name: "
        let number = Console.ReadLine()
        let message = search database "phone" number
        printfn $"{message}"
        options database
    | "4" ->
        display database
        options database
    | "5" ->
        printfn "Enter the filename: "
        let filename = Console.ReadLine()
        if String.IsNullOrWhiteSpace(filename) then printfn "Filename cannot be empty!"
        else
            let notes = serialize database
            try
                File.WriteAllText(filename, notes)
                printfn $"Database was successfully copied to a file {filename}!"
                options database
            with ex ->
                printfn $"Error: {ex.Message}"
    | "6" ->
        printfn "Enter the filename: "
        let filename = Console.ReadLine()
        if not (File.Exists(filename)) then printfn "File not found!"
        else
            let notes = File.ReadAllText(filename)
            match deserialize notes with
            | Success (newDatabase, message) ->
                printfn $"{message}"
                options newDatabase
            | Failure (message) ->
                printfn $"{message}"
                options database
    | _ ->
        printfn "Unknown command!"
        options database