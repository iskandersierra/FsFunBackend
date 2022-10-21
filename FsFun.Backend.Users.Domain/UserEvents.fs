namespace FsFun.Backend.Users.Domain

open System

type UserEvent =
    | UserEventCreatedFirst of UserEventCreatedFirst
    | UserEventCreated of UserEventCreated

and UserEventCreatedFirst =
    { displayName: string
      firstName: string
      lastName: string
      email: string

      subjectId: string }

and UserEventCreated =
    { displayName: string
      firstName: string
      lastName: string
      email: string

      purgeAfter: TimeSpan }
