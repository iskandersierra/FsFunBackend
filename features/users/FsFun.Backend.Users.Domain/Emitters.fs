module FsFun.Backend.Users.Domain.Emitters

open FsFun.Backend.Core.Domain

type UserCommandCreateState =
    | NewState
    | ExistingState

let emitUserCommandCreateFirst
    (command: UserCommandCreateFirst)
    (state: UserCommandCreateState) =
    match state with
    | NewState ->
        Ok [ UserEventCreatedFirst
                 { displayName = command.displayName
                   firstName = command.firstName
                   lastName = command.lastName
                   email = command.email
                   subjectId = command.subjectId } ]
    | ExistingState -> Error "User is already created"

let emitUserCommandCreate 
    (clock: IClock)
    (command: UserCommandCreate)
    (state: UserCommandCreateState) =
    match state with
    | NewState ->
        let now = clock.UtcNow
        Ok [ UserEventCreated
                 { displayName = command.displayName
                   firstName = command.firstName
                   lastName = command.lastName
                   email = command.email
                   purgeAt = now.Add(command.purgeAfter) } ]
    | ExistingState -> Error "User is already created"
