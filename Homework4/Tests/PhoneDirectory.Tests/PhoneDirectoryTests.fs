module PhoneDirectory.Tests

open NUnit.Framework 
open Directory

[<Test>]
let Test_AddNote () =
    let db = emptyDatabase
    let result = addRecord db "Alice" "1234"
    match result with
    | Success (newDb, message) ->
        Assert.That(1, Is.EqualTo(List.length newDb))
        Assert.That("New note has been added!", Is.EqualTo(message))
        let first = List.head newDb
        Assert.That(Name "Alice", Is.EqualTo(first.Name))
        Assert.That(PhoneNumber "1234", Is.EqualTo(first.PhoneNumber))
    | Failure _ -> Assert.Fail("Success was expected, but an error was received!")

[<Test>]
let Test_AddNoteWithNameDuplicate () =
    let db = [ { Name = Name "Bob"; PhoneNumber = PhoneNumber "555" } ]
    let result = addRecord db "Bob" "999"
    match result with
    | Success (newDb, message) ->
        Assert.That(message, Does.Contain "added")
        Assert.That(List.length newDb, Is.EqualTo(2))
    | Failure _ -> Assert.Fail("Success was expected")

[<Test>]
let Test_AddNoteWithoutPhoneNumberDuplicate () =
    let db = [ { Name = Name "Bob"; PhoneNumber = PhoneNumber "555" } ]
    let result = addRecord db "Alice" "555"
    match result with
    | Success (newDb, message) ->
        Assert.That(List.length newDb, Is.EqualTo 1)
        Assert.That(message, Does.Contain "owner changed")
        let record = List.head newDb
        Assert.That(record.Name, Is.EqualTo (Name "Alice"))
        Assert.That(record.PhoneNumber, Is.EqualTo (PhoneNumber "555"))
    | Failure _ -> Assert.Fail("Success was expected")

[<Test>]
let Test_SearchByNameWhenNameExists () =
    let db = [ { Name = Name "Vera"; PhoneNumber = PhoneNumber "777" } ]
    let result = searchByName db "Vera"
    match result with
    | Some (PhoneNumber number) -> Assert.That(number, Is.EqualTo("777"))
    | None -> Assert.Fail("Expected some, but None")

[<Test>]
let Test_SearchByNameNotFound () =
    let db = [ { Name = Name "Kate"; PhoneNumber = PhoneNumber "777" } ]
    let result = searchByName db "John"
    Assert.That(result, Is.EqualTo(None))

[<Test>]
let Test_SearchByPhoneNumber () =
    let db = [ { Name = Name "Vera"; PhoneNumber = PhoneNumber "777" } ]
    let result = searchByPhone db "777"
    match result with
    | Some (Name name) -> Assert.That(name, Is.EqualTo "Vera")
    | None -> Assert.Fail("Expected Some, but None")

[<Test>]
let Test_Serialize () =
    let db = [ { Name = Name "Kate"; PhoneNumber = PhoneNumber "000" } ]
    let result = serialize db
    let expected = ["Kate: 000"]
    Assert.That(result, Is.EqualTo(expected: obj))

[<Test>]
let Test_Deserialize () =
    let input = [| "Anna: 111"; "Boris: 222" |]
    let result = deserialize input
    match result with
    | Success (db, _) ->
        Assert.That(2, Is.EqualTo(List.length db))
        let first = db |> List.find (fun x -> x.Name = Name "Anna")
        Assert.That(PhoneNumber "111", Is.EqualTo(first.PhoneNumber))
    | Failure message -> Assert.Fail($"Deserialization error: {message}")

[<Test>]
let Test_DeserializeIncorrectFormat () =
    let input = [| "InvalidLine" |]
    let result = deserialize input
    match result with
    | Failure message -> Assert.That(message.Contains("Invalid string format") || message.Contains("Error reading file"))
    | Success _ -> Assert.Fail("A format error was expected")