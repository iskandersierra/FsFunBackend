module FsFun.Backend.Users.Domain.EmittersTests

open FsFun.Backend.Users.Domain.Emitters

open Xunit
open Swensen.Unquote
open System

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
              { displayName = "Display Name"
                firstName = "First Name"
                lastName = "Last Name"
                email = "user@gmail.com"
                subjectId = "1234abcd" } ]

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

    test <@ emitUserCommandCreate command state = Error "User is already created" @>

[<Fact>]
let ``emitUserCommandCreate on new state`` () =
    let command: UserCommandCreate =
        { displayName = "Display Name"
          firstName = "First Name"
          lastName = "Last Name"
          email = "user@gmail.com"
          purgeAfter = TimeSpan.FromDays 7 }

    let state = NewState

    let expectedEvents =
        [ UserEventCreated
              { displayName = "Display Name"
                firstName = "First Name"
                lastName = "Last Name"
                email = "user@gmail.com"
                purgeAfter = TimeSpan.FromDays 7 } ]

    test <@ emitUserCommandCreate command state = Ok expectedEvents @>
