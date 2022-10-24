module FsFun.Backend.Core.Application.IClockTests

open FsFun.Backend.Core.Application

open Xunit
open System
open Swensen.Unquote

[<Fact>]
let ``SystemClock`` () =
    let clock: IClock = SystemClock()
    let clockNow = clock.UtcNow
    let now = DateTime.UtcNow
    let diffMs = (now - clockNow).Milliseconds
    test <@ diffMs <= 5 @>
