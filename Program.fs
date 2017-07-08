// Learn more about F# at http://fsharp.org

open System
open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax
open System.Linq
open System.Collections.Generic

let sort (usings : IEnumerable<UsingDirectiveSyntax>) : IEnumerable<UsingDirectiveSyntax> = 
    (usings.OrderBy (fun u -> u.ToFullString ())).AsEnumerable ()

let stringify (usings : IEnumerable<UsingDirectiveSyntax>) : string = 
    (usings.Select (fun u -> u.ToFullString ())).ToArray ()
        |> String.concat ""

[<EntryPoint>]
let main argv =
    let fileName = if Array.length argv > 0 then argv.[0] else "Sample/Sample.cs"
    let file = IO.File.ReadAllText fileName
    let ast = CSharpSyntaxTree.ParseText file
    let root  = ast.GetRoot () :?> CompilationUnitSyntax
    let usings = root.Usings |> stringify
    let sortedUsings = root.Usings |> sort |> stringify

    printfn "Original\n%s" usings
    printfn "Sorted\n%s" sortedUsings

    0 // return an integer exit code

