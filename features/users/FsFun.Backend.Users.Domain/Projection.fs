module FsFun.Backend.Users.Domain.Projection

open System

type UserProjection =
    { displayName: string
      firstName: string
      lastName: string
      email: string

      subjectId: string option
      purgeAt: DateTime option }

let projectUser (event: UserEvent) (current: UserProjection option) =
    match event, current with
    | UserEventCreatedFirst event, None ->
        Some
            { displayName = event.displayName
              firstName = event.firstName
              lastName = event.lastName
              email = event.email

              subjectId = Some event.subjectId
              purgeAt = None }

    | UserEventCreatedFirst _, Some p -> Some p

    | UserEventCreated event, None ->
        Some
            { displayName = event.displayName
              firstName = event.firstName
              lastName = event.lastName
              email = event.email

              subjectId = None
              purgeAt = event.purgeAt |> Some }

    | UserEventCreated _, Some p -> Some p
