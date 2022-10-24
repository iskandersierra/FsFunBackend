module FsFun.Backend.Users.Domain.EmittersTests

open FsFun.Backend.Users.Domain.Emitters

open Xunit
open Swensen.Unquote
open System
open FsFun.Backend.Core.Domain

[<Fact>]
let ``emitUserCommandCreateFirst on existing state`` () =
    let command: UserCommandCreateFirst =
        { displayName = "Display Name"
          firstName = "First Name"
          lastName = "Last Name"
          email = "user@gmail.com"
          subjectId = "1234abcd" }

    let state = ExistingState

    test <@ emitUserCommandCreateFirst command state = Error "User is already created" @>

[<Fact>]
let ``emitUserCommandCreateFirst on new state`` () =
    let command: UserCommandCreateFirst =
        { displayName = "Display Name"
          firstName = "First Name"
          lastName = "Last Name"
          email = "user@gmail.com"
          subjectId = "1234abcd" }

    let state = NewState

    let expectedEvents =
        [ UserEventCreatedFirst
              { displayName = command.displayName
                firstName = command.firstName
                lastName = command.lastName
                email = command.email
                subjectId = command.subjectId } ]

    test <@ emitUserCommandCreateFirst command state = Ok expectedEvents @>

[<Fact>]
let ``emitUserCommandCreate on existing state`` () =
    let command: UserCommandCreate =
        { displayName = "Display Name"
          firstName = "First Name"
          lastName = "Last Name"
          email = "user@gmail.com"
          purgeAfter = TimeSpan.FromDays 7 }

    let state = ExistingState

    let now = DateTime.Parse("2020-04-13T00:00:00Z")

    test <@ emitUserCommandCreate now command state = Error "User is already created" @>

[<Fact>]
let ``emitUserCommandCreate on new state`` () =
    let command: UserCommandCreate =
        { displayName = "Display Name"
          firstName = "First Name"
          lastName = "Last Name"
          email = "user@gmail.com"
          purgeAfter = TimeSpan.FromDays 7 }

    let state = NewState

    let now = DateTime.Parse("2020-04-13T00:00:00Z")

    let expectedEvents =
        [ UserEventCreated
              { displayName = command.displayName
                firstName = command.firstName
                lastName = command.lastName
                email = command.email
                purgeAt = now.Add command.purgeAfter } ]

    test <@ emitUserCommandCreate now command state = Ok expectedEvents @>
