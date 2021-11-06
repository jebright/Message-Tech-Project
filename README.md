# Motley Fool Skills Test

Welcome to the TMF skills test! Below you will find guidelines for fixing up an existing .NET
console application. Show us what you can do within about 8 hours.

The provided materials should include everything you need to get started but we
_absolutely_ encourage any questions you might have -- just email us [msg-tech@fool.com](mailto:msg-tech@fool.com). We're
friendly... we think. 🙂

We will schedule your interview after we have received your submission. We
look forward to discussing your experiences and challenges!

Submit your work on a GitHub repo and include all necessary code, scripts, files, and instructions
to run the app locally. **Please** include any instructions we'll need. It really helps.

Have fun! And thanks for taking the time to complete our test!

## Introduction

We've provided you with a copy of a DailyWatchlistEmails console application (read more about it's
purpose and function in the project's README). We've also provided a SQL script for creating a SQL Server
database containing the few data structures utilized by the application, including a small amount of
test data.

This application was thrown together in a hurry well over a decade ago and has since been neglected. It
got the job done when it only sent a few thousand emails each day. But now that it sends hundreds of
thousands of emails daily, its inefficiencies are resulting in many subscribers receiving their emails
unacceptably late.

The application does not send emails directly, rather it queues them up in our 3rd party marketing platform,
MailChamp 😉. MailChamp has proven capable of sending millions of emails per hour, so we're confident that
the bottleneck is within the DailyWatchlistEmails app.

## Primary Goal

Get the job done more quickly!

## Secondary Goals

- The app was originally written to target .NET Framework 3.5. We've since managed to update to .NET
  Framework 4.8, but we'd really like to see it targeting .NET Core, or even .NET 5.
- The quality of the current implementation reflects its origins as a rapid prototype. We'd like to see
  something more befitting of an application that delivers our most popular premium email. Show us what
  you can do to clean it up.

## Evaluation Criteria

- .NET and C# usage
- SQL usage
- Application design
- Thoroughness/error-handling
- Documentation
- Instructions for running the application
- Anything you did to make the application your own

## In Conclusion...

Remember:

- Have fun
- Complete as much as you can in about 8 hours
- Provide instructions for running the application, please

Again, thanks for taking the time to complete this skills test! We're excited to see what you come up with!
