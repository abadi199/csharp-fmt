module CSharpFmt.Basics 

open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax

let getRoot (tree : SyntaxTree) : CompilationUnitSyntax =
    tree.GetRoot () :?> CompilationUnitSyntax
    