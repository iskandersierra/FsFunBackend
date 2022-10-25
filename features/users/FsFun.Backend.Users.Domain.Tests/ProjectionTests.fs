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
    (envelope: EventEnvelope<UserEventCreatedFirst>) =
    let expected: UserProjection = {
        displayName = envelope.event.displayName
        firstName = envelope.event.firstName
        lastName = envelope.event.lastName
        email = envelope.event.email
        subjectId = Some envelope.event.subjectId
        purgeAt = None
        createdAt = envelope.timestamp
        modifiedAt = envelope.timestamp
    }

    let actualEnvelope = envelope |> EventEnvelope.map UserEventCreatedFirst

    test <@ projectUser actualEnvelope None = Some expected @>

[<Property>]
let ``projectUser event UserEventCreatedFirst on existing User``
    (envelope: EventEnvelope<UserEventCreatedFirst>)
    (current: UserProjection) =
    let actualEnvelope = envelope |> EventEnvelope.map UserEventCreatedFirst
    test <@ projectUser actualEnvelope (Some current) = Some current @>

[<Property>]
let ``projectUser event UserEventCreated on new User``
    (envelope: EventEnvelope<UserEventCreated>) =
    let expected: UserProjection = {
        displayName = envelope.event.displayName
        firstName = envelope.event.firstName
        lastName = envelope.event.lastName
        email = envelope.event.email
        subjectId = None
        purgeAt = Some envelope.event.purgeAt
        createdAt = envelope.timestamp
        modifiedAt = envelope.timestamp
    }
    let actualEnvelope = envelope |> EventEnvelope.map UserEventCreated
    test <@ projectUser actualEnvelope None = Some expected @>

[<Property>]
let ``projectUser event UserEventCreated on existing User``
    (envelope: EventEnvelope<UserEventCreated>)
    (current: UserProjection) =
    let actualEnvelope = envelope |> EventEnvelope.map UserEventCreated
    test <@ projectUser actualEnvelope (Some current) = Some current @>
