module Lazy

open System.Threading
open ILazy

type private SingleThreadedState<'a>() =
    let mutable isComputed = false
    let mutable value = Unchecked.defaultof<'a>

    member this.Compute(supplier: unit -> 'a) : 'a =
        if not isComputed then
            value <- supplier()
            isComputed <- true
        value

type private LockingState<'a>() =
    let mutable isComputed = false
    let mutable value = Unchecked.defaultof<'a>
    let lockObj = obj()
    member this.Compute(supplier: unit -> 'a) : 'a =
        if Volatile.Read(&isComputed) then 
            value
        else
            lock lockObj (fun () ->
                if not (Volatile.Read(&isComputed)) then
                    value <- supplier()
                    Volatile.Write(&isComputed, true)
                value
            )

type private LockFreeState<'a>() =
    let mutable state: 'a option = None
    member this.Compute(supplier: unit -> 'a) : 'a =
        match Volatile.Read(&state) with
        | Some v -> v
        | None ->
            let candidate = supplier()
            let original = Interlocked.CompareExchange(&state, Some candidate, None)
            match original with
            | None -> candidate
            | Some v -> v


type SingleLazy<'a>(supplier: unit -> 'a) =
    let lazyState = SingleThreadedState<'a>()
    interface ILazy<'a> with
        member _.Get() = lazyState.Compute(supplier)

type MultiLazy<'a> (supplier: unit -> 'a) =
    let lazyState = LockingState<'a>()
    interface ILazy<'a> with
        member _.Get() = lazyState.Compute(supplier)
            
type LockFreeLazy<'a> (supplier: unit -> 'a) =
    let lazyState = LockFreeState<'a>()
    interface ILazy<'a> with
        member this.Get() = lazyState.Compute(supplier)