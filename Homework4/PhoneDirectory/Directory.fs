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
    match database |> List.tryFind (fun x -> x.PhoneNumber = newNumber) with
    | Some existingREcord ->
        let updatedDatabase = 
            database |> List.map (fun record ->
                if record.PhoneNumber = newNumber then
                    { record with Name = newName }
                else record)
        Success (updatedDatabase, $"Phone number {number} owner changed to {name}!")
    | None ->
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