module Lazy

open System.Threading
open ILazy

type private LazyState<'a> () =
    let mutable isComputed = 0
    let mutable value = Unchecked.defaultof<'a>

    member this.Single (supplier: unit -> 'a) =
        if not (isComputed = 1) then
            value <- supplier()
            isComputed <- 1
        value

    member this.Multi (supplier: unit -> 'a, lockObj: obj) =
        if isComputed = 1 then value
        else
            lock lockObj (fun () ->
                if not (isComputed = 1) then
                    value <- supplier()
                    isComputed <- 1
                value
            )

    member this.LockFree (supplier: unit -> 'a) =
        if isComputed = 1 then
            value
        else
            let candidate = supplier()
            if Interlocked.CompareExchange(&isComputed, 1, 0) = 0 then
                value <- candidate
                candidate
            else
                Thread.Yield() |> ignore
                value


type SingleLazy<'a>(supplier: unit -> 'a) =
    let lazyState = LazyState<'a>()
    interface ILazy<'a> with
        member _.Get() = lazyState.Single(supplier)

type MultiLazy<'a> (supplier: unit -> 'a) =
    let lazyState = LazyState<'a>()
    let lockObj : obj = new System.Object()
    interface ILazy<'a> with
        member _.Get() = lazyState.Multi(supplier, lockObj)
            
type LockFreeLazy<'a> (supplier: unit -> 'a) =
    let lazyState = LazyState<'a>()
    let lockObj : obj = new System.Object()
    interface ILazy<'a> with
        member this.Get() = lazyState.LockFree(supplier)