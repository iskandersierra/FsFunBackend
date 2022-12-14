namespace FsFun.Backend.Core.Application

open System

type IClock =
    abstract member UtcNow : DateTime

type SystemClock() =
    interface IClock with
        member __.UtcNow = DateTime.UtcNow
