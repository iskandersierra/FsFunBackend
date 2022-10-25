module FsFun.Backend.Users.Domain.EmittersTests

open FsFun.Backend.Users.Domain.Emitters

open Xunit
open Swensen.Unquote
open System
open FsFun.Backend.Core.Domain

[<Fact>]
let ``emitUserCommandCreateFirst on existing state`` () =
    let now = DateTime.Parse("2020-04-13T00:00:00Z")

    let envelope: CommandEnvelope<UserCommandCreateFirst> =
        { entityId = "1234"
          timestamp = now
          command =
            { displayName = "Display Name"
              firstName = "First Name"
              lastName = "Last Name"
              email = "user@gmail.com"
              subjectId = "1234abcd" } }

    let state = ExistingState

    test <@ emitUserCommandCreateFirst envelope state = Error "User is already created" @>

[<Fact>]
let ``emitUserCommandCreateFirst on new state`` () =
    let now = DateTime.Parse("2020-04-13T00:00:00Z")

    let envelope: CommandEnvelope<UserCommandCreateFirst> =
        { entityId = "1234"
          timestamp = now
          command =
            { displayName = "Display Name"
              firstName = "First Name"
              lastName = "Last Name"
              email = "user@gmail.com"
              subjectId = "1234abcd" } }

    let state = NewState

    let expectedEvents =
        [ UserEventCreatedFirst
              { displayName = envelope.command.displayName
                firstName = envelope.command.firstName
                lastName = envelope.command.lastName
                email = envelope.command.email
                subjectId = envelope.command.subjectId } ]

    test <@ emitUserCommandCreateFirst envelope state = Ok expectedEvents @>

[<Fact>]
let ``emitUserCommandCreate on existing state`` () =
    let now = DateTime.Parse("2020-04-13T00:00:00Z")

    let envelope: CommandEnvelope<UserCommandCreate> =
        { entityId = "1234"
          timestamp = now
          command =
            { displayName = "Display Name"
              firstName = "First Name"
              lastName = "Last Name"
              email = "user@gmail.com"
              purgeAfter = TimeSpan.FromDays 7 } }

    let state = ExistingState

    test <@ emitUserCommandCreate envelope state = Error "User is already created" @>

[<Fact>]
let ``emitUserCommandCreate on new state`` () =
    let now = DateTime.Parse("2020-04-13T00:00:00Z")

    let envelope: CommandEnvelope<UserCommandCreate> =
        { entityId = "1234"
          timestamp = now
          command =
            { displayName = "Display Name"
              firstName = "First Name"
              lastName = "Last Name"
              email = "user@gmail.com"
              purgeAfter = TimeSpan.FromDays 7 } }

    let state = NewState

    let expectedEvents =
        [ UserEventCreated
              { displayName = envelope.command.displayName
                firstName = envelope.command.firstName
                lastName = envelope.command.lastName
                email = envelope.command.email
                purgeAt = now.Add envelope.command.purgeAfter } ]

    test <@ emitUserCommandCreate envelope state = Ok expectedEvents @>
