module CSharpFmt.UsingDirective

open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax
open System.Linq
open System.Collections.Generic
open CSharpFmt.Helpers
open CSharpFmt.Basics

let sortUsings (usings : SyntaxList<UsingDirectiveSyntax>) : SyntaxList<UsingDirectiveSyntax> = 
    let usingEnumerable = (usings.OrderBy (fun u -> u.ToFullString ())).AsEnumerable ()
    (SyntaxList<UsingDirectiveSyntax>()).AddRange(usingEnumerable)

let stringify (usings : SyntaxList<UsingDirectiveSyntax>) : string = 
    (usings.Select (fun u -> u.ToFullString ())).ToArray ()
        |> String.concat ""

let sortRoot (root : CompilationUnitSyntax) : CompilationUnitSyntax =
    root.Usings 
        |> sortUsings
        |> root.WithUsings 

let sort (tree : SyntaxTree) : SyntaxTree =
    tree 
        |> getRoot
        |> sortRoot
        |> (tree.WithRootAndOptions |> curryFlip) (CSharpParseOptions ())

let removeUsing (root : CompilationUnitSyntax) : CompilationUnitSyntax =
    root.WithUsings (SyntaxList<UsingDirectiveSyntax> ())

let addUsingInsideNamespace usingDirectives (namespaceDeclaration : NamespaceDeclarationSyntax) (root : CompilationUnitSyntax) : CompilationUnitSyntax =
    let updatedNamespace = 
        namespaceDeclaration.WithUsings(usingDirectives)
            :> MemberDeclarationSyntax
            |> singleArray
    
    root.AddMembers(updatedNamespace)

let removeNamespace (namespaceDeclaration : NamespaceDeclarationSyntax) (root : CompilationUnitSyntax) : CompilationUnitSyntax =
    root.RemoveNode (namespaceDeclaration, SyntaxRemoveOptions.KeepNoTrivia)

let moveInsideNamespace (tree : SyntaxTree) : SyntaxTree =
    let root = tree |> getRoot
    let usingDirectives = root.Usings
    let namespaceDeclarations = root.Members.OfType<NamespaceDeclarationSyntax>()
    
    if namespaceDeclarations.Count() = 1 then
        root 
            |> removeNamespace (namespaceDeclarations.First ())
            |> addUsingInsideNamespace usingDirectives (namespaceDeclarations.First ())
            |> removeUsing 
            |> (tree.WithRootAndOptions |> curryFlip) (CSharpParseOptions ())
    else
        tree
