module FsFun.Backend.Users.Domain.Emitters

open System
open FsFun.Backend.Core.Domain

type UserCommandCreateState =
    | NewState
    | ExistingState

let emitUserCommandCreateFirst (envelope: CommandEnvelope<UserCommandCreateFirst>) (state: UserCommandCreateState) =
    match state with
    | NewState ->
        Ok [ UserEventCreatedFirst
                 { displayName = envelope.command.displayName
                   firstName = envelope.command.firstName
                   lastName = envelope.command.lastName
                   email = envelope.command.email
                   subjectId = envelope.command.subjectId } ]
    | ExistingState -> Error "User is already created"

let emitUserCommandCreate (envelope: CommandEnvelope<UserCommandCreate>) (state: UserCommandCreateState) =
    match state with
    | NewState ->
        Ok [ UserEventCreated
                 { displayName = envelope.command.displayName
                   firstName = envelope.command.firstName
                   lastName = envelope.command.lastName
                   email = envelope.command.email
                   purgeAt = envelope.timestamp.Add(envelope.command.purgeAfter) } ]
    | ExistingState -> Error "User is already created"
