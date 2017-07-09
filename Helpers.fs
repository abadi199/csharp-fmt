module CSharpFmt.Helpers

let flip f =
    fun b a -> f (a, b)