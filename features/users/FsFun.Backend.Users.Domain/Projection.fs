module FsFun.Backend.Users.Domain.Projection

open System
open FsFun.Backend.Core.Domain

type UserProjection =
    { displayName: string
      firstName: string
      lastName: string
      email: string

      subjectId: string option
      purgeAt: DateTime option
      
      createdAt: DateTime
      modifiedAt: DateTime }

let projectUser (envelope: EventEnvelope<UserEvent>) (current: UserProjection option) =
    match envelope.event, current with
    | UserEventCreatedFirst event, None ->
        Some
            { displayName = event.displayName
              firstName = event.firstName
              lastName = event.lastName
              email = event.email

              subjectId = Some event.subjectId
              purgeAt = None
              
              createdAt = envelope.timestamp
              modifiedAt = envelope.timestamp }

    | UserEventCreatedFirst _, Some p -> Some p

    | UserEventCreated event, None ->
        Some
            { displayName = event.displayName
              firstName = event.firstName
              lastName = event.lastName
              email = event.email

              subjectId = None
              purgeAt = event.purgeAt |> Some
              
              createdAt = envelope.timestamp
              modifiedAt = envelope.timestamp }

    | UserEventCreated _, Some p -> Some p
