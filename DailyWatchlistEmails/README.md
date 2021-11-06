# DailyWatchlistEmails

The daily watchlist email is a summary of articles published during the past 
24 hours, and is tailored to each subscriber based on the instruments (stocks, crypto currencies,
etc.) they've added to their watchlist. So if an article references Amazon (AMZN), 
and a subscribed user is watching AMZN, then their email will include content about 
that article (headline, byline, link to article, etc.)

## The process:

The application is scheduled to run at 5 PM ET each week day.

1. Retrieve all users subscribed to daily watchlist email (`Email Subscription ID: 12`).
1. Retrieve all articles published between 5 PM ET yesterday and 5 PM ET today.
1. Generate XML including necessary details about articles and instruments and update the 
daily watchlist template in MailChamp (`Template ID: 9131`).
1. For each subscribed user:
   1. Retrieve the user's watched instruments.
   1. Find the intersection of instruments referenced in today's articles and the 
   user's watched instruments.
      1. If no intersection, this user doesn't get an email today. Skip to
         next user.
   1. Retrieve user info for the email (email address, name) and build comma-separated list of
      instrument ids to include in the user's email.
   1. Queue daily watchlist email via MailChamp (`Mailing ID: 2934`).

## Integrations

### Articles API

Used to retrieve the day's published articles.

### MailChamp API

Used to update the daily watchlist template and send daily watchlist emails.

Swagger docs for MailChamp are available as a Solution Item: `MailChamp_swagger.json`.

### Fool SQL Server Database

Repository for users, email subscriptions, watched instruments and more.