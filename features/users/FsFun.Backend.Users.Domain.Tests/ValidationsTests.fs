module FsFun.Backend.Users.Domain.ValidationsTests

open FsFun.Backend.Users.Domain.Validations

open Xunit
open System
open System.Xml
open Swensen.Unquote

[<Theory>]
[<InlineData(null, "Display name should not be blank")>]
[<InlineData("", "Display name should not be blank")>]
[<InlineData("    ", "Display name should not be blank")>]
[<InlineData("Ab", "Display name should have between 3 and 100 characters")>]
[<InlineData("  Ab  ", "Display name should have between 3 and 100 characters")>]
[<InlineData("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890!",
             "Display name should have between 3 and 100 characters")>]
let ``validateDisplayName invalid values`` (value: string) (error: string) =
    test <@ validateDisplayName value = Error error @>

[<Theory>]
[<InlineData("Abc")>]
[<InlineData("Valid Name")>]
[<InlineData("  Valid Name  ")>]
[<InlineData("A very long still valid display name")>]
let ``validateDisplayName valid values`` (value: string) =
    test <@ validateDisplayName value = Ok(value.Trim()) @>

[<Theory>]
[<InlineData(null, "First name should not be blank")>]
[<InlineData("", "First name should not be blank")>]
[<InlineData("    ", "First name should not be blank")>]
[<InlineData("Ab", "First name should have between 3 and 100 characters")>]
[<InlineData("  Ab  ", "First name should have between 3 and 100 characters")>]
[<InlineData("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890!",
             "First name should have between 3 and 100 characters")>]
let ``validateFirstName invalid values`` (value: string) (error: string) =
    test <@ validateFirstName value = Error error @>

[<Theory>]
[<InlineData("Abc")>]
[<InlineData("Valid Name")>]
[<InlineData("  Valid Name  ")>]
[<InlineData("A very long still valid first name")>]
let ``validateFirstName valid values`` (value: string) =
    test <@ validateFirstName value = Ok(value.Trim()) @>

[<Theory>]
[<InlineData(null, "Last name should not be blank")>]
[<InlineData("", "Last name should not be blank")>]
[<InlineData("    ", "Last name should not be blank")>]
[<InlineData("Ab", "Last name should have between 3 and 100 characters")>]
[<InlineData("  Ab  ", "Last name should have between 3 and 100 characters")>]
[<InlineData("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890!",
             "Last name should have between 3 and 100 characters")>]
let ``validateLastName invalid values`` (value: string) (error: string) =
    test <@ validateLastName value = Error error @>

[<Theory>]
[<InlineData("Abc")>]
[<InlineData("Valid Name")>]
[<InlineData("  Valid Name  ")>]
[<InlineData("A very long still valid last name")>]
let ``validateLastName valid values`` (value: string) =
    test <@ validateLastName value = Ok(value.Trim()) @>

[<Theory>]
[<InlineData(null, "Subject Id should not be blank")>]
[<InlineData("", "Subject Id should not be blank")>]
[<InlineData("    ", "Subject Id should not be blank")>]
[<InlineData("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890!",
             "Subject Id should have between 1 and 100 characters")>]
let ``validateSubjectId invalid values`` (value: string) (error: string) =
    test <@ validateSubjectId value = Error error @>

[<Theory>]
[<InlineData("A")>]
[<InlineData("Valid ID")>]
[<InlineData("  Valid ID  ")>]
[<InlineData("A very long still valid ID")>]
let ``validateSubjectId valid values`` (value: string) =
    test <@ validateSubjectId value = Ok(value.Trim()) @>

[<Theory>]
[<InlineData(null, "Email should not be blank")>]
[<InlineData("", "Email should not be blank")>]
[<InlineData("    ", "Email should not be blank")>]
[<InlineData("A@", "Email should have between 3 and 100 characters")>]
[<InlineData("  A@  ", "Email should have between 3 and 100 characters")>]
[<InlineData("thisemailisnotvalid", "Email should be a valid email address")>]
[<InlineData("@thisemailisnotvalid", "Email should be a valid email address")>]
[<InlineData("thisemailisnotvalid@", "Email should be a valid email address")>]
[<InlineData("this@email@is@not@valid", "Email should be a valid email address")>]
let ``validateEmail invalid values`` (value: string) (error: string) =
    test <@ validateEmail value = Error error @>

[<Theory>]
[<InlineData("a@b")>]
[<InlineData("user.smith@doman.here.com")>]
[<InlineData("  user.smith@doman.here.com  ")>]
let ``validateEmail valid values`` (value: string) =
    test <@ validateEmail value = Ok(value.Trim()) @>

[<Fact>]
let ``validateUserCommandCreateFirst all invalid`` () =
    let command: UserCommandCreateFirst =
        { displayName = ""
          firstName = ""
          lastName = ""
          email = ""
          subjectId = "" }

    let validation = validateUserCommandCreateFirst command

    let expectedErrors =
        [ "Display name should not be blank"
          "First name should not be blank"
          "Last name should not be blank"
          "Email should not be blank"
          "Subject Id should not be blank" ]

    test <@ validation = Error expectedErrors @>

[<Fact>]
let ``validateUserCommandCreateFirst all valid`` () =
    let command: UserCommandCreateFirst =
        { displayName = "Display Name"
          firstName = "First Name"
          lastName = "Last Name"
          email = "user@gmail.com"
          subjectId = "1234abcd" }

    let validation = validateUserCommandCreateFirst command

    test <@ validation = Ok command @>

[<Theory>]
[<InlineData("PT4M", "Purge after should be between 00:05:00 and 30.00:00:00")>]
[<InlineData("P30DT1M", "Purge after should be between 00:05:00 and 30.00:00:00")>]
let ``validatePurgeAfter invalid values`` (value: string) (error: string) =
    let purgeAfter = XmlConvert.ToTimeSpan(value)
    test <@ validatePurgeAfter purgeAfter = Error error @>

[<Theory>]
[<InlineData("PT5M")>]
[<InlineData("PT1H")>]
[<InlineData("P1D")>]
[<InlineData("P7D")>]
[<InlineData("P30D")>]
let ``validatePurgeAfter valid values`` (value: string) =
    let purgeAfter = XmlConvert.ToTimeSpan(value)
    test <@ validatePurgeAfter purgeAfter = Ok purgeAfter @>

[<Fact>]
let ``validateUserCommandCreate all invalid`` () =
    let command: UserCommandCreate =
        { displayName = ""
          firstName = ""
          lastName = ""
          email = ""
          purgeAfter = TimeSpan.Zero }

    let validation = validateUserCommandCreate command

    let expectedErrors =
        [ "Display name should not be blank"
          "First name should not be blank"
          "Last name should not be blank"
          "Email should not be blank"
          "Purge after should be between 00:05:00 and 30.00:00:00" ]

    test <@ validation = Error expectedErrors @>

[<Fact>]
let ``validateUserCommandCreate all valid`` () =
    let command: UserCommandCreate =
        { displayName = "Display Name"
          firstName = "First Name"
          lastName = "Last Name"
          email = "user@gmail.com"
          purgeAfter = TimeSpan.FromDays 7 }

    let validation = validateUserCommandCreate command

    test <@ validation = Ok command @>
