module CSharpFmt.Using

open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax
open System.Linq
open System.Collections.Generic
open CSharpFmt.Helpers

let sort (usings : SyntaxList<UsingDirectiveSyntax>) : SyntaxList<UsingDirectiveSyntax> = 
    let usingEnumerable = (usings.OrderBy (fun u -> u.ToFullString ())).AsEnumerable ()
    (SyntaxList<UsingDirectiveSyntax>()).AddRange(usingEnumerable)

let stringify (usings : SyntaxList<UsingDirectiveSyntax>) : string = 
    (usings.Select (fun u -> u.ToFullString ())).ToArray ()
        |> String.concat ""



let sortRoot (root : CompilationUnitSyntax) : CompilationUnitSyntax =
    root.Usings 
        |> sort 
        |> root.WithUsings 


let sortTree (tree : SyntaxTree) : SyntaxTree =
    tree.GetRoot () 
        :?> CompilationUnitSyntax
        |> sortRoot
        |> (flip tree.WithRootAndOptions) (CSharpParseOptions ())
