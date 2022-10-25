namespace FsFun.Backend.Core.Domain

open System

type EventEnvelope<'event> =
    { entityId: string
      version: int
      timestamp: DateTime
      event: 'event }

type CommandEnvelope<'command> =
    { entityId: string
      timestamp: DateTime
      command: 'command }

module EventEnvelope =
    let map fn (envelope: EventEnvelope<'a>) =
        { entityId = envelope.entityId
          version = envelope.version
          timestamp = envelope.timestamp
          event = fn envelope.event }
