module FsFun.Backend.Users.Domain.UserCommandHandlers

type UserCommandCreateState =
    | NewState
    | ExistingState

let handleUserCommandCreateFirst (command: UserCommandCreateFirst) (state: UserCommandCreateState) =
    match state with
    | NewState ->
        Ok [ UserEventCreatedFirst
                 { displayName = command.displayName
                   firstName = command.firstName
                   lastName = command.lastName
                   email = command.email
                   subjectId = command.subjectId } ]
    | ExistingState -> Error "User is already created"

let handleUserCommandCreate (command: UserCommandCreate) (state: UserCommandCreateState) =
    match state with
    | NewState ->
        Ok [ UserEventCreated
                 { displayName = command.displayName
                   firstName = command.firstName
                   lastName = command.lastName
                   email = command.email
                   purgeAfter = command.purgeAfter } ]
    | ExistingState -> Error "User is already created"
