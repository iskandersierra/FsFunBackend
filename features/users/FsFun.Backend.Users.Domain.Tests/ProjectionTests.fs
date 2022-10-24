module FsFun.Backend.Users.Domain.ProjectionTests

open FsFun.Backend.Users.Domain
open FsFun.Backend.Users.Domain.Projection

open Xunit
open Swensen.Unquote
open System
open FsFun.Backend.Core.Domain
open FsCheck.Xunit

[<Property>]
let ``projectUser event UserEventCreatedFirst on new User``
    (event: UserEventCreatedFirst) =
    let expected: UserProjection = {
        displayName = event.displayName
        firstName = event.firstName
        lastName = event.lastName
        email = event.email
        subjectId = Some event.subjectId
        purgeAt = None
    }

    test <@ projectUser (UserEventCreatedFirst event) None = Some expected @>

[<Property>]
let ``projectUser event UserEventCreatedFirst on existing User``
    (event: UserEventCreatedFirst)
    (current: UserProjection) =
    test <@ projectUser (UserEventCreatedFirst event) (Some current) = Some current @>

[<Property>]
let ``projectUser event UserEventCreated on new User``
    (event: UserEventCreated) =
    let expected: UserProjection = {
        displayName = event.displayName
        firstName = event.firstName
        lastName = event.lastName
        email = event.email
        subjectId = None
        purgeAt = Some event.purgeAt
    }

    test <@ projectUser (UserEventCreated event) None = Some expected @>

[<Property>]
let ``projectUser event UserEventCreated on existing User``
    (event: UserEventCreated)
    (current: UserProjection) =
    test <@ projectUser (UserEventCreated event) (Some current) = Some current @>
