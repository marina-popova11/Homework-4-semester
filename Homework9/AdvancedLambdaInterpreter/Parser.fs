module Parser

open System
open FParsec
open Interpreter

type InputObjects = {
    Definition: (string * LambdaTerm) list
    Expression: LambdaTerm
}

let spaces: Parser<unit, unit> = skipMany (pchar ' ' <|> pchar '\t' <|> pchar '\n')

let identifier =
    let isChar c = System.Char.IsLetterOrDigit c || c = '_'
    many1Satisfy2L System.Char.IsLetter isChar "identifier"
let pEquality = pchar '=' .>> spaces

let pLet = pstring "let" .>> spaces

let pSlash = pchar '\\' .>> spaces

let pDot = pchar '.' .>> spaces

let pTerm, pTermImpl = createParserForwardedToRef()

let pAtomTerm =
    choice [
        between (pchar '(' .>> spaces) (pchar ')' .>> spaces) pTerm
        identifier |>> Var
    ]

let pApp =
    chainl1 pAtomTerm (spaces >>. preturn (fun f g -> App (f, g)))

let pLambda =
    pSlash >>. many1 (identifier .>> spaces) >>= fun args ->
        pDot >>. pTerm |>> fun body ->
            List.foldBack (fun arg acc -> Abs (arg, acc)) args body

let pSimpleTerm =
    choice [
        pLambda
        pApp
    ]

do pTermImpl := pSimpleTerm

let pDefinition =
    pLet >>. identifier .>> pEquality .>>. pTerm |>> fun (name, term) -> (name, term)

let pProgram =
    let pLines =
        choice [
            attempt (pDefinition |>> fun def -> Choice1Of2 def)
            (pTerm |>> fun t -> Choice2Of2 t)
        ]
    
    let pDefs = many (pDefinition .>> spaces)
    
    pDefs .>>. (pTerm .>> spaces .>> eof) |>> fun (defs, main) ->
    { Definition = defs; Expression = main }

let parseString (str: string) =
    run pProgram str

let parseFile (filePath: string) =
    let body = System.IO.File.ReadAllText(filePath)
    run pProgram body