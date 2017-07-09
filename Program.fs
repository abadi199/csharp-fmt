open System
open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax
open System.Linq
open System.Collections.Generic



[<EntryPoint>]
let main argv =

    let fileName : string = 
        if Array.length argv > 0 then argv.[0] else "Sample/Sample.cs"

    let file : string = 
        IO.File.ReadAllText fileName

    let ast : SyntaxTree = 
        CSharpSyntaxTree.ParseText file

    let sortedAst : SyntaxTree =
        ast |> CSharpFmt.Using.sortTree


    printfn "Original\n%s" (ast.ToString ())
    printfn "Sorted\n%s" (sortedAst.ToString ())

    // return an integer exit code
    0

