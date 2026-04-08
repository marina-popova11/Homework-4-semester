module Directory

open System
open System.IO

type Name =  Name of string
type PhoneNumber = PhoneNumber of string
type Record = {
    Name: Name
    PhoneNumber: PhoneNumber
}
type Database = Record list
type OperationResult<'T> = 
    | Success of 'T * string
    | Failure of string

let emptyDatabase : Database = []

let addRecord (database: Database) (name: string) (number: string) : OperationResult<Database> =
    let newName = Name name
    let newNumber = PhoneNumber number
    if database |> List.exists (fun x -> x.Name = newName || x.PhoneNumber = newNumber)
        then Failure $"Contact {name} or number {number} already exists!"
    else
        let newNote = {Name = newName; PhoneNumber = newNumber}
        let newDatabase = newNote :: database
        Success (newDatabase, "New note has been added!")

let searchByName (database: Database) (searchElement: string) : PhoneNumber option =
    let name = Name searchElement
    database |> List.tryFind (fun x -> x.Name = name)
    |> Option.map (fun note -> note.PhoneNumber)

let searchByPhone (database: Database) (searchElement: string) =
    let phone = (PhoneNumber) searchElement
    database |> List.tryFind (fun x -> x.PhoneNumber = phone)
    |> Option.map (fun note -> note.Name)

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

let serialize (database: Database) : string list =
    database |> List.map (fun x ->
        let (Name name) = x.Name
        let (PhoneNumber number) = x.PhoneNumber
        $"{name}: {number}")

let deserialize (data: string array) : OperationResult<Database> =
    if data.Length = 0 then
        Success (emptyDatabase, "File is empty!")
    else
        try
            let notes = 
                data
                |> Array.filter (not << System.String.IsNullOrWhiteSpace)
                |> Array.map (fun line ->
                    let parts = line.Split(": ")
                    if parts.Length <> 2 then invalidArg "line" "Invalid string format!"
                    {Name = Name (parts.[0].Trim()); PhoneNumber = PhoneNumber (parts.[1].Trim())})
                |> Array.toList
            Success (notes, "Data loaded successfully!")
        with
        | ex -> Failure $"Error reading file: {ex.Message}!"