module Directory

open System
open System.IO

type Name =  Name of string
type PhoneNumber = PhoneNumber of string
type Note = {
    Name: Name
    PhoneNumber: PhoneNumber
}
type Database = Note list
type OperationResult<'T> = 
    | Success of 'T * string
    | Failure of string

let emptyDatabase : Database = []

let addNote (database: Database) (name: string) (number: string) : OperationResult<Database> =
    let newName = Name name
    let newNumber = PhoneNumber number
    if database |> List.exists (fun x -> x.Name = newName || x.PhoneNumber = newNumber)
        then Failure $"Contact {name} or number {number} already exists!"
    else
        let newNote = {Name = newName; PhoneNumber = newNumber}
        let newDatabase = newNote :: database
        Success (newDatabase, "New notes was created!")
let search (database: Database) (subject: string) (searchElement: string) =
    match subject with
    | "name" ->
        let name = Name searchElement
        match database |> List.tryFind (fun x -> x.Name = name) with
        | Some note ->
            let (PhoneNumber number) = note.PhoneNumber
            $"Phone number {number} found by name {searchElement}!"
        | None ->
            $"No phone numbers were found for name {searchElement}!"
    | "phone" ->
        let phone = (PhoneNumber) searchElement
        match database |> List.tryFind (fun x -> x.PhoneNumber = phone) with
        | Some note ->
            let (Name name) = note.Name
            $"Name {name} found by phone number {searchElement}!"
        | None ->
            $"No names were found for phone number {searchElement}!"
    | _ -> "Unknown operation!"

let display (database: Database) =
    if List.isEmpty database then printfn "Database is empty!"
    else
        database |> List.iter (fun x ->
            let name = x.Name
            let number = x.PhoneNumber
            printfn $"{name}: {number}")

let serialize (database: Database) : string =
    database |> List.map (fun x ->
        let (Name name) = x.Name
        let (PhoneNumber number) = x.PhoneNumber
        $"{name}: {number}")
    |> String.concat "\n"

let deserialize (data: string) : OperationResult<Database> =
    if System.String.IsNullOrWhiteSpace(data) then
        Success (emptyDatabase, "File is empty!")
    else
        try
            let notes = 
                data.Split('\n')
                |> Array.filter (not << System.String.IsNullOrWhiteSpace)
                |> Array.map (fun line ->
                    let parts = line.Split(':')
                    if parts.Length <> 2 then invalidArg "line" "Invalid string format!"
                    {Name = Name (parts.[0].Trim()); PhoneNumber = PhoneNumber (parts.[1].Trim())})
                |> Array.toList
            Success (notes, "Data loaded successfully!")
        with
        | ex -> Failure $"Error reading file: {ex.Message}!"