module FsFun.Backend.Core.Domain.Validations

open FsToolkit.ErrorHandling
open System
open System.Text.RegularExpressions

let validateTextTrim (text: string) =
    if isNull text then "" else text.Trim()
    |> Ok

let validateNullableTextTrim (text: string) =
    if isNull text then null else text.Trim()
    |> Ok

let validateNonEmpty name (text: string) =
    if String.IsNullOrEmpty text then
        Error (sprintf "%s should not be empty" name)
    else
        Ok text

let validateNonWhitespace name (text: string) =
    if String.IsNullOrWhiteSpace text then
        Error (sprintf "%s should not be blank" name)
    else
        Ok text

let validateTextMinLength name minLen (text: string) =
    if String.length text < minLen then
        Error (sprintf "%s should have at least %d characters" name minLen)
    else
        Ok text

let validateTextMaxLength name maxLen (text: string) =
    if String.length text > maxLen then
        Error (sprintf "%s should have at most %d characters" name maxLen)
    else
        Ok text

let validateTextBetweenLength name minLen maxLen (text: string) =
    if String.length text < minLen || String.length text > maxLen then
        Error (sprintf "%s should have at between %d and %d characters" name minLen maxLen)
    else
        Ok text

let validateTextMatches message (regex: Regex) (text: string) =
    if not (regex.IsMatch(text)) then
        Error message
    else
        Ok text

let validateRequiredText name minLen maxLen (text: string) =
    validation {
        let! text = validateTextTrim text
        let! text = validateNonWhitespace name text
        let! text = validateTextBetweenLength name minLen maxLen text
        return text
    }

let validateGreaterThan name (minValue: 'a) (value: 'a) =
    if value <= minValue then
        Error (sprintf "%s should be greater than %A" name minValue)
    else
        Ok value

let validateGreaterThanOrEqualTo name (minValue: 'a) (value: 'a) =
    if value < minValue then
        Error (sprintf "%s should be greater than or equal to %A" name minValue)
    else
        Ok value

let validateLessThan name (maxValue: 'a) (value: 'a) =
    if value >= maxValue then
        Error (sprintf "%s should be less than %A" name maxValue)
    else
        Ok value

let validateLessThanOrEqualTo name (maxValue: 'a) (value: 'a) =
    if value > maxValue then
        Error (sprintf "%s should be less than or equal to %A" name maxValue)
    else
        Ok value

let validateBetweenInclusive name (minValue: 'a) (maxValue: 'a) (value: 'a) =
    if value < minValue || maxValue < value then
        Error (sprintf "%s should be between %A and %A" name minValue maxValue)
    else
        Ok value

let validateBetweenExclusive name (minValue: 'a) (maxValue: 'a) (value: 'a) =
    if value <= minValue || maxValue <= value then
        Error (sprintf "%s should be exclusivelly between %A and %A" name minValue maxValue)
    else
        Ok value
