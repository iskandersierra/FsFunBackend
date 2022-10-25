module FsFun.Backend.Users.Application.CommandHandlers

open FsFun.Backend.Users.Domain
open FsToolkit.ErrorHandling

type HandleUserCommandCreateFirstServices = {
    validate: UserCommandCreateFirst -> Async<Validation<UserCommandCreateFirst, string>>
}

let handleUserCommandCreateFirst
    (services: HandleUserCommandCreateFirstServices)
    (command: UserCommandCreateFirst) =
    asyncResult {
        let! command = services.validate command
        return ()
    }
