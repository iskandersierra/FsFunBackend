module FsFun.Backend.Core.Domain.ValidationsTests

open FsFun.Backend.Core.Domain.Validations

open Xunit
open FsCheck
open FsCheck.Xunit
open System
open System.Text.RegularExpressions
open Swensen.Unquote

[<Property>]
let ``validateTextTrim should always return Ok of non-null string`` (str: string) =
    let validation = validateTextTrim str

    match str with
    | null -> test <@ validation = Ok "" @>
    | str -> test <@ validation = Ok(str.Trim()) @>

[<Property>]
let ``validateNullableTextTrim should always return Ok of trimmed string`` (str: string) =
    let validation = validateNullableTextTrim str

    match str with
    | null -> test <@ validation = Ok null @>
    | str -> test <@ validation = Ok(str.Trim()) @>

[<Property>]
let ``validateNonEmpty should reject empty strings`` (str: string) (name: string) =
    let validation = validateNonEmpty name str

    match str with
    | null -> test <@ validation = Error(sprintf "%s should not be empty" name) @>
    | "" -> test <@ validation = Error(sprintf "%s should not be empty" name) @>
    | str -> test <@ validation = Ok str @>

[<Property>]
let ``validateNonBlank should reject blank strings`` (str: string) (name: string) =
    let validation = validateNonBlank name str

    match str with
    | null -> test <@ validation = Error(sprintf "%s should not be blank" name) @>
    | str when String.IsNullOrWhiteSpace(str) -> test <@ validation = Error(sprintf "%s should not be blank" name) @>
    | str -> test <@ validation = Ok str @>

[<Property>]
let ``validateTextMinLength should reject strings shorter than minLen`` (str: string) (minLen: int) (name: string) =
    let validation = validateTextMinLength name minLen str

    match str with
    | null -> test <@ validation = Ok str @>
    | str when String.length (str) < minLen ->
        test <@ validation = Error(sprintf "%s should have at least %d characters" name minLen) @>
    | str -> test <@ validation = Ok str @>

[<Property>]
let ``validateTextMaxLength should reject strings longer than maxLen`` (str: string) (maxLen: int) (name: string) =
    let validation = validateTextMaxLength name maxLen str

    match str with
    | null -> test <@ validation = Ok str @>
    | str when String.length (str) > maxLen ->
        test <@ validation = Error(sprintf "%s should have at most %d characters" name maxLen) @>
    | str -> test <@ validation = Ok str @>

[<Property>]
let ``validateTextBetweenLength should reject strings shorter than minLen or longer than maxLen``
    (str: string)
    (minLen: int)
    (maxLen: int)
    (name: string)
    =
    let validation =
        validateTextBetweenLength name minLen maxLen str

    match str with
    | null -> test <@ validation = Ok str @>
    | str when
        String.length (str) < minLen
        || String.length (str) > maxLen
        ->
        test <@ validation = Error(sprintf "%s should have between %d and %d characters" name minLen maxLen) @>
    | str -> test <@ validation = Ok str @>

[<Property>]
let ``validateTextMatches should reject strings that do not match regex`` (str: string) (message: string) =
    let regex =
        Regex("^[a-z]+$", RegexOptions.Compiled ||| RegexOptions.Singleline)

    let validation = validateTextMatches message regex str

    match str with
    | null -> test <@ validation = Ok str @>
    | str when not (regex.IsMatch(str)) -> test <@ validation = Error message @>
    | str -> test <@ validation = Ok str @>

[<Property>]
let ``validateRequiredText should trim and reject blank strings or short or long strings``
    (str: string)
    (minLen: int)
    (maxLen: int)
    (name: string)
    =
    let validation =
        validateRequiredText name minLen maxLen str

    match str with
    | null -> test <@ validation = Error(sprintf "%s should not be blank" name) @>
    | str ->
        let str = str.Trim()

        match str with
        | str when String.IsNullOrWhiteSpace(str) ->
            test <@ validation = Error(sprintf "%s should not be blank" name) @>
        | str when
            String.length (str) < minLen
            || String.length (str) > maxLen
            ->
            test <@ validation = Error(sprintf "%s should have between %d and %d characters" name minLen maxLen) @>
        | str -> test <@ validation = Ok str @>

[<Property>]
let ``validateGreaterThan on int should reject values less than or equal to min``
    (min: int)
    (value: int)
    (name: string)
    =
    let validation = validateGreaterThan name min value

    match value with
    | value when value <= min -> test <@ validation = Error(sprintf "%s should be greater than %d" name min) @>
    | value -> test <@ validation = Ok value @>

[<Property>]
let ``validateGreaterThan on DateTime should reject values less than or equal to min``
    (min: DateTime)
    (value: DateTime)
    (name: string)
    =
    let validation = validateGreaterThan name min value

    match value with
    | value when value <= min -> test <@ validation = Error(sprintf "%s should be greater than %A" name min) @>
    | value -> test <@ validation = Ok value @>

[<Property>]
let ``validateGreaterThanOrEqualTo on int should reject values less than min`` (min: int) (value: int) (name: string) =
    let validation =
        validateGreaterThanOrEqualTo name min value

    match value with
    | value when value < min ->
        test <@ validation = Error(sprintf "%s should be greater than or equal to %d" name min) @>
    | value -> test <@ validation = Ok value @>

[<Property>]
let ``validateGreaterThanOrEqualTo on DateTime should reject values less than min``
    (min: DateTime)
    (value: DateTime)
    (name: string)
    =
    let validation =
        validateGreaterThanOrEqualTo name min value

    match value with
    | value when value < min ->
        test <@ validation = Error(sprintf "%s should be greater than or equal to %A" name min) @>
    | value -> test <@ validation = Ok value @>

[<Property>]
let ``validateLessThan on int should reject values greater than or equal to max``
    (max: int)
    (value: int)
    (name: string)
    =
    let validation = validateLessThan name max value

    match value with
    | value when value >= max -> test <@ validation = Error(sprintf "%s should be less than %d" name max) @>
    | value -> test <@ validation = Ok value @>

[<Property>]
let ``validateLessThan on DateTime should reject values greater than or equal to max``
    (max: DateTime)
    (value: DateTime)
    (name: string)
    =
    let validation = validateLessThan name max value

    match value with
    | value when value >= max -> test <@ validation = Error(sprintf "%s should be less than %A" name max) @>
    | value -> test <@ validation = Ok value @>

[<Property>]
let ``validateLessThanOrEqualTo on int should reject values greater than max`` (max: int) (value: int) (name: string) =
    let validation = validateLessThanOrEqualTo name max value

    match value with
    | value when value > max -> test <@ validation = Error(sprintf "%s should be less than or equal to %d" name max) @>
    | value -> test <@ validation = Ok value @>

[<Property>]
let ``validateLessThanOrEqualTo on DateTime should reject values greater than max``
    (max: DateTime)
    (value: DateTime)
    (name: string)
    =
    let validation = validateLessThanOrEqualTo name max value

    match value with
    | value when value > max -> test <@ validation = Error(sprintf "%s should be less than or equal to %A" name max) @>
    | value -> test <@ validation = Ok value @>

[<Property>]
let ``validateBetweenInclusive on int should reject values less than min or greater than max``
    (min: int)
    (max: int)
    (value: int)
    (name: string)
    =
    let validation =
        validateBetweenInclusive name min max value

    match value with
    | value when value < min || value > max ->
        test <@ validation = Error(sprintf "%s should be between %d and %d" name min max) @>
    | value -> test <@ validation = Ok value @>

[<Property>]
let ``validateBetweenExclusive on int should reject values less than or equal to min or greater than or equal to max``
    (min: int)
    (max: int)
    (value: int)
    (name: string)
    =
    let validation =
        validateBetweenExclusive name min max value

    match value with
    | value when value <= min || value >= max ->
        test <@ validation = Error(sprintf "%s should be exclusivelly between %d and %d" name min max) @>
    | value -> test <@ validation = Ok value @>
