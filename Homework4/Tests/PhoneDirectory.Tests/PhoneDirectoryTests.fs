module PhoneDirectory.Tests

open NUnit.Framework 
open Directory

[<Test>]
let Test_AddNote () =
    let db = emptyDatabase
    let result = addNote db "Alice" "1234"
    match result with
    | Success (newDb, message) ->
        Assert.That(1, Is.EqualTo(List.length newDb))
        Assert.That("New notes was created!", Is.EqualTo(message))
        let first = List.head newDb
        Assert.That(Name "Alice", Is.EqualTo(first.Name))
        Assert.That(PhoneNumber "1234", Is.EqualTo(first.PhoneNumber))
    | Failure _ -> Assert.Fail("Success was expected, but an error was received!")

[<Test>]
let Test_AddNoteWithoutNameDuplicate () =
    let db = [ { Name = Name "Bob"; PhoneNumber = PhoneNumber "555" } ]
    let result = addNote db "Bob" "999"
    match result with
    | Failure message -> Assert.That(message.Contains("already exists"), Is.True)
    | Success _ -> Assert.Fail("A duplicate error was expected!")

[<Test>]
let Test_AddNoteWithoutPhoneNumberDuplicate () =
    let db = [ { Name = Name "Bob"; PhoneNumber = PhoneNumber "555" } ]
    let result = addNote db "Alice" "555"
    match result with
    | Failure message -> Assert.That(message.Contains("already exists"), Is.True)
    | Success _ -> Assert.Fail("A duplicate error was expected")

[<Test>]
let Test_Search () =
    let db = [ { Name = Name "Vera"; PhoneNumber = PhoneNumber "777" } ]
    let message = search db "name" "Vera"
    Assert.That(message.Contains("777"), Is.True)

[<Test>]
let Test_SearchNotFound () =
    let db = [ { Name = Name "Kate"; PhoneNumber = PhoneNumber "777" } ]
    let message = search db "name" "John"
    Assert.That(message.Contains("No phone numbers were found"), Is.True)

[<Test>]
let Test_SearchByPhoneNumber () =
    let db = [ { Name = Name "Vera"; PhoneNumber = PhoneNumber "777" } ]
    let message = search db "phone" "777"
    Assert.That(message.Contains("Vera"))

[<Test>]
let Test_Serialize () =
    let db = [ { Name = Name "Kate"; PhoneNumber = PhoneNumber "000" } ]
    let str = serialize db
    Assert.That("Kate: 000", Is.EqualTo(str))

[<Test>]
let Test_Deserialize () =
    let input = "Anna:111\nBoris:222"
    let result = deserialize input
    match result with
    | Success (db, _) ->
        Assert.That(2, Is.EqualTo(List.length db))
        let first = db |> List.find (fun x -> x.Name = Name "Anna")
        Assert.That(PhoneNumber "111", Is.EqualTo(first.PhoneNumber))
    | Failure message -> Assert.Fail($"Deserialization error: {message}")

[<Test>]
let Test_DeserializeIncorrectFormat () =
    let input = "InvalidLine"
    let result = deserialize input
    match result with
    | Failure message -> Assert.That(message.Contains("Invalid string format") || message.Contains("Error reading file"))
    | Success _ -> Assert.Fail("A format error was expected")