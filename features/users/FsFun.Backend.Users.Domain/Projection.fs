module FsFun.Backend.Users.Domain.Projection

open System

type UserProjection =
    { displayName: string
      firstName: string
      lastName: string
      email: string

      subjectId: string option
      purgeAt: DateTime option }

let project (event: UserEvent) (current: UserProjection option) =
    match current, event with
    | None, UserEventCreatedFirst event ->
        Some
            { displayName = event.displayName
              firstName = event.firstName
              lastName = event.lastName
              email = event.email

              subjectId = Some event.subjectId
              purgeAt = None }

    //| Some current, UserEventCreated event ->
            
