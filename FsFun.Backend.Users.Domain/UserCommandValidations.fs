module FsFun.Backend.Users.Domain.Validations

open FsFun.Backend.Core.Domain.Validations
open FsToolkit.ErrorHandling
open System
open System.Text.RegularExpressions

// Fields

let validateDisplayName displayName =
    validateRequiredText "Display name" 3 100 displayName

let validateFirstName firstName =
    validateRequiredText "First name" 3 100 firstName

let validateLastName lastName =
    validateRequiredText "Last name" 3 100 lastName

let validateSubjectId subjectId =
    validateRequiredText "Subject Id" 1 100 subjectId

let internal minPurgeAfter = TimeSpan.FromMinutes 5.0
let internal maxPurgeAfter = TimeSpan.FromDays 30.0
let validatePurgeAfter purgeAfter =
    validateBetweenInclusive "Purge after" minPurgeAfter maxPurgeAfter purgeAfter

let internal emailRegex =
    Regex(@"^[^@]+@[^@]+$", RegexOptions.Compiled ||| RegexOptions.Singleline)

let validateEmail email =
    validation {
        let! email = validateRequiredText "Email" 3 100 email
        let! email = validateTextMatches "Email should have propper format" emailRegex email
        return email
    }

// Commands

let validateUserCommandCreateFirst (command: UserCommandCreateFirst) =
    validation {
        let! displayName = validateDisplayName command.displayName
        and! firstName = validateFirstName command.firstName
        and! lastName = validateLastName command.lastName
        and! email = validateEmail command.email
        and! subjectId = validateSubjectId command.subjectId

        return
            { displayName = displayName
              firstName = firstName
              lastName = lastName
              email = email
              subjectId = subjectId }
    }

let validateUserCommandCreate (command: UserCommandCreate) =
    validation {
        let! displayName = validateDisplayName command.displayName
        and! firstName = validateFirstName command.firstName
        and! lastName = validateLastName command.lastName
        and! email = validateEmail command.email
        and! purgeAfter = validatePurgeAfter command.purgeAfter

        return
            { displayName = displayName
              firstName = firstName
              lastName = lastName
              email = email
              purgeAfter = purgeAfter }
    }
