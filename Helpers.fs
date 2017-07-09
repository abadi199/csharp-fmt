module CSharpFmt.Helpers

/// Flip the order of the first two arguments to a function.
let flip f =
    fun b a -> f a b

/// Change how arguments are passed to a function. 
/// This splits paired arguments into two separate arguments.
let curry f =
    fun a b -> f (a, b)

/// Change how arguments are passed to a function. 
/// This combines two arguments into a single pair.
let uncurry f =
    fun (a, b) -> f a b

/// Turns an element into an array of single element
let singleArray a =
    [|a|]

/// Curry then flip
let curryFlip f =
    f |> curry |> flip