namespace FsFun.Backend.Users.Domain

open System

type UserCommandCreateFirst =
    { displayName: string
      firstName: string
      lastName: string
      email: string

      subjectId: string }

type UserCommandCreate =
    { displayName: string
      firstName: string
      lastName: string
      email: string

      purgeAfter: TimeSpan }

//type UserCommandRename =
//    { id: string

//      displayName: string
//      firstName: string
//      lastName: string }

//type UserCommandChangePurgeDate =
//    { id: string

//      purgeAfter: TimeSpan }

//type UserCommandActivate =
//    { id: string }

//type UserCommandDeactivate =
//    { id: string }

