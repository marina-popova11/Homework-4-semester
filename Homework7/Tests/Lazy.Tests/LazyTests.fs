module Lazy.Tests

open NUnit.Framework
open System.Threading
open System.Threading.Tasks
open System.Collections.Concurrent
open System
open Lazy
open ILazy

let runConcurrentTestAsync (lazyObj: ILazy<'T>) (threadCount: int) =
    async {
        let results = ConcurrentBag<'T>()
        let actualThreadCount = if threadCount > 0 then threadCount else Environment.ProcessorCount * 2
        let tasks = 
            [1..actualThreadCount] |> List.map (fun _ ->
                Task.Run(fun () ->
                    let res = lazyObj.Get()
                    results.Add(res) |> ignore
                )
            )
        
        do! Task.WhenAll(tasks) |> Async.AwaitTask
        return results.ToArray()
    }

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
    Assert.That(!callCount, Is.EqualTo(1), "Supplier should be called exactly once")

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
        Guid.NewGuid()
    
    let lazyState = MultiLazy<Guid>(supplier) :> ILazy<Guid>
    let results = runConcurrentTestAsync lazyState 50 |> Async.RunSynchronously
    Assert.That(50, Is.EqualTo(results.Length))
    let first = results.[0]
    for res in results do
        Assert.That(first, Is.EqualTo(res))
    Assert.That(!callCount, Is.EqualTo(1), "Supplier should be called exactly once due to locking")

[<Test>]
let Test_MultiLazyWorksCorrectlyValueAlreadyCalculated () =
    let callCount = ref 0
    let supplier = fun () ->
        Interlocked.Increment(callCount) |> ignore
        "Cached"
    
    let lazyState = MultiLazy<string>(supplier) :> ILazy<string>
    let _ = lazyState.Get()
    let countAfterFirst = !callCount
    let results = runConcurrentTestAsync lazyState 20 |> Async.RunSynchronously
    
    Assert.That(1, Is.EqualTo(countAfterFirst))
    Assert.That(!callCount, Is.EqualTo(1), "Supplier should not be called again after initial computation")
    for res in results do
        Assert.That(res, Is.EqualTo("Cached"))

[<Test>]
let Test_LockFreeLazy () =
    let callCount = ref 0
    let supplier = fun () -> 
        Thread.Sleep(10)
        let id = Interlocked.Increment(callCount)
        Guid.NewGuid()
    
    let lazyState = LockFreeLazy<Guid>(supplier) :> ILazy<Guid>
    
    let results = runConcurrentTestAsync lazyState 50 |> Async.RunSynchronously
    let distinctResults = results |> Seq.distinct |> Seq.length
    Assert.That(1, Is.EqualTo(distinctResults))
    Assert.That(!callCount, Is.GreaterThanOrEqualTo(1), "Supplier should be called at least once")

[<Test>]
let Test_LockFreeLazyCanPerformCalculationMoreThanOnce () =
    let callCount = ref 0
    let supplier = fun () -> 
        Thread.Sleep(20)
        Interlocked.Increment(callCount) |> ignore
        "Data"
    
    let lazyState = LockFreeLazy<string>(supplier) :> ILazy<string>
    let _ = runConcurrentTestAsync lazyState 50 |> Async.RunSynchronously
    Assert.That(!callCount, Is.GreaterThanOrEqualTo(1))