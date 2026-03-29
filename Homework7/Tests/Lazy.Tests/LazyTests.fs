module Lazy.Tests

open NUnit.Framework
open System.Threading
open System.Collections.Concurrent
open System
open Lazy
open ILazy

let runConcurrentTest (lazyObj: ILazy<'T>) (threadCount: int) =
    let results = ConcurrentBag<'T>()
    let threads = 
        [1..threadCount] |> List.map (fun _ ->
            new Thread(fun () ->
                let res = lazyObj.Get()
                results.Add(res) |> ignore
            )
        )
    
    threads |> List.iter (fun t -> t.Start())
    threads |> List.iter (fun t -> t.Join())
    
    results.ToArray()

[<Test>]
let Test_SingleLazy () =
    let callCount = ref 0
    let supplier = fun () -> 
        incr callCount
        "Result"
    
    let lazyState = SingleLazy<string>(supplier) :> ILazy<string>
    let r1 = lazyState.Get()
    let r2 = lazyState.Get()
    let r3 = lazyState.Get()
    
    Assert.That("Result", Is.EqualTo(r1))
    Assert.That("Result", Is.EqualTo(r2))
    Assert.That("Result", Is.EqualTo(r3))

[<Test>]
let Test_SingleLazyReturnsTheSameObject () =
    let supplier = fun () -> DateTime.Now
    
    let lazyState = SingleLazy<DateTime>(supplier) :> ILazy<DateTime>
    let r1 = lazyState.Get()
    let r2 = lazyState.Get()
    
    Assert.That(r1, Is.EqualTo(r2))

[<Test>]
let Test_MultiLazy () =
    let callCount = ref 0
    let supplier = fun () -> 
        Thread.Sleep(50)
        Interlocked.Increment(callCount) |> ignore
        "UniqueResult"
    
    let lazyState = MultiLazy<string>(supplier) :> ILazy<string>
    let results = runConcurrentTest lazyState 50
    Assert.That(50, Is.EqualTo(results.Length))
    let first = results.[0]
    for res in results do
        Assert.That(first, Is.EqualTo(res))

[<Test>]
let Test_MultiLazyWorksCorrectlyValueAlreadyCalculated () =
    let callCount = ref 0
    let supplier = fun () -> incr callCount; "Cached"
    
    let lazyState = MultiLazy<string>(supplier) :> ILazy<string>
    let _ = lazyState.Get()
    let countAfterFirst = !callCount
    let results = runConcurrentTest lazyState 20
    
    Assert.That(1, Is.EqualTo(countAfterFirst))

[<Test>]
let Test_LockFreeLazy () =
    let callCount = ref 0
    let supplier = fun () -> 
        Thread.Sleep(10)
        let id = Interlocked.Increment(callCount)
        sprintf "Result-%d" id
    
    let lazyState = LockFreeLazy<string>(supplier) :> ILazy<string>
    
    let results = runConcurrentTest lazyState 50
    let distinctResults = results |> Seq.distinct |> Seq.length
    Assert.That(1, Is.EqualTo(distinctResults))

[<Test>]
let Test_LockFreeLazyCanPerformCalculationMoreThanOnce () =
    let callCount = ref 0
    let supplier = fun () -> 
        Thread.Sleep(20) // Длинная пауза увеличивает шанс гонки
        Interlocked.Increment(callCount) |> ignore
        "Data"
    
    let lazyState = LockFreeLazy<string>(supplier) :> ILazy<string>
    let _ = runConcurrentTest lazyState 50
    Assert.That(!callCount, Is.GreaterThanOrEqualTo(1))