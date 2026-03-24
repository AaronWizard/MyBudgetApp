# API Design

- [General Notes](#general-notes)
- [Users](#users)
  - [\[POST\] /User/Register](#post-userregister)
  - [\[POST\] /User/Register/Verify](#post-userregisterverify)
  - [\[POST\] /User/Register/Verify/Resend](#post-userregisterverifyresend)
  - [\[POST\] /User/Login](#post-userlogin)
  - [\[POST\] /User/Login/Verify](#post-userloginverify)
  - [\[POST\] /User/Logout](#post-userlogout)
  - [\[POST\] /User/Password/Change](#post-userpasswordchange)
  - [\[POST\] /User/Password/Forgot](#post-userpasswordforgot)
  - [\[POST\] /User/Password/Reset](#post-userpasswordreset)
  - [\[POST\] /User/RefreshAccess](#post-userrefreshaccess)
- [Transaction Categories](#transaction-categories)
  - [\[GET\] /Transaction/Category/GetAll](#get-transactioncategorygetall)
  - [\[POST\] /Transaction/Category/Add](#post-transactioncategoryadd)
  - [\[PUT\] /Transaction/Category/Edit](#put-transactioncategoryedit)
  - [\[DELETE\] /Transaction/Category/Delete](#delete-transactioncategorydelete)
- [Single Transaction Types](#single-transaction-types)
  - [\[GET\] /Transaction/Single/Type/GetSystem](#get-transactionsingletypegetsystem)
  - [\[GET\] /Transaction/Single/Type/GetUser](#get-transactionsingletypegetuser)
  - [\[POST\] /Transaction/Single/Type/Add](#post-transactionsingletypeadd)
  - [\[PUT\] /Transaction/Single/Type/Edit](#put-transactionsingletypeedit)
  - [\[DELETE\] /Transaction/Single/Type/Delete](#delete-transactionsingletypedelete)
- [Single Transactions](#single-transactions)
  - [\[GET\] /Transaction/Single/GetAll](#get-transactionsinglegetall)
  - [\[POST\] /Transaction/Single/Add](#post-transactionsingleadd)
  - [\[PUT\] /Transaction/Single/Update](#put-transactionsingleupdate)
  - [\[DELETE\] /Transaction/Single/Delete](#delete-transactionsingledelete)
- [Recurring Transactions](#recurring-transactions)
  - [\[GET\] /Transaction/Recurring/PeriodType](#get-transactionrecurringperiodtype)
  - [\[GET\] /Transaction/Recurring/GetAll](#get-transactionrecurringgetall)
  - [\[POST\] /Transaction/Recurring/Add](#post-transactionrecurringadd)
  - [\[PUT\] /Transaction/Recurring/Update](#put-transactionrecurringupdate)
  - [\[DELETE\] /Transaction/Recurring/Delete](#delete-transactionrecurringdelete)
- [Transaction Queries](#transaction-queries)
  - [\[GET\] Transaction/Average/ByPeriod](#get-transactionaveragebyperiod)
  - [\[GET\] Transaction/Average/ByDateRange](#get-transactionaveragebydaterange)

## General Notes

- The API is implemented in ASP.NET Core.
- Most methods require a JWT bearer access token.

## Users

### \[POST\] /User/Register

Register a new user. An email is sent to the provided email address for verification, containing a one-time verification token that lasts for a set amount of time. \
Response is intentionally vague to avoid revealing whether an email is taken or not.

Request:

- Email (string)
- Password (string)

Responses:

- Accepted (202)

*Does not require a bearer access token.*

### \[POST\] /User/Register/Verify

Verifies a user's registration. Used through a verification link in an email. \
The user is logged in after a successful registration. The verification may fail if the verification token expired; the user may request a new verification email.

Request:

- RegistrationToken (string)

Responses:

- OK (200)
  - AccessToken (string)
- Bad Request (400)

*Does not require a bearer access token.*

### \[POST\] /User/Register/Verify/Resend

Requests a new verification email for an unverified user. \
Response is intentionally vague to avoid revealing if the email exists or is verified.

Request:

- Email (string)

Response:

- Accepted (202)

*Does not require a bearer access token.*

### \[POST\] /User/Login

Starts logging in a user. Creates a 2FA code sent to the user's email address, and a login token used by */User/Login/Verify*.

Request:

- Email (string)
- Password (string)

Responses:

- OK (200)
  - LoginToken (string)

*Does not require a bearer access token.*

### \[POST\] /User/Login/Verify

Completes the 2FA login. Returns an access token, which is the actual bearer access token, and a refresh token.

Request:

- LoginToken (string)
- LoginCode (string)

Responses:

- OK (200)
  - AccessToken (string)
  - RefreshToken (string)

*Does not require a bearer access token.*

### \[POST\] /User/Logout

Logs the user out, invalidating their refresh token on the server.

Responses:

- No Content (204)
- Unauthorized (401)

### \[POST\] /User/Password/Change

Changes the (logged in) user's password.

Request:

- CurrentPassword (string)
- NewPassword (string)

Responses:

- No Content (204)
- Unauthorized (401)

### \[POST\] /User/Password/Forgot

Allows the user to reset their password. A reset link is sent to their email containing a reset token.

Request:

- Email (string)

Response:

- Accepted (202)

*Does not require a bearer access token.*

### \[POST\] /User/Password/Reset

Completes the password reset process.

Request:

- ResetToken (string)
- NewPassword (string)

Response:

- No Content (204)

*Does not require a bearer access token.*

### \[POST\] /User/RefreshAccess

Refreshes the user's access token (which only lasts 15 minutes). Periodically called by the front end, e.g. when another API method returns *Unauthorized*.

Request:

- RefreshToken (string)

Response:

- OK (200)
  - AccessToken (string)
- Bad Request (400)

*Does not require a bearer access token.*

## Transaction Categories

### \[GET\] /Transaction/Category/GetAll

Gets the user's transaction categories.

Request: None

Responses:

- OK (200)
  - List
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[POST\] /Transaction/Category/Add

Adds a user transaction category. \
Duplicate names are not allowed.

Request:

- Name (string)

Responses:

- Created (201)
- Bad Request (400)
- Unauthorized (401)

### \[PUT\] /Transaction/Category/Edit

Edits a user transaction category. \
Duplicate names are not allowed.

Request:

- Id (int)
- Name (string)

Responses:

- OK (200)
- Not Found (404)
- Bad Request (400)
- Unauthorized (401)

### \[DELETE\] /Transaction/Category/Delete

Deletes a user category.

Request:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

## Single Transaction Types

### \[GET\] /Transaction/Single/Type/GetSystem

Gets the system types that can be assigned to single transactions.

Responses:

- OK (200)
  - List:
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[GET\] /Transaction/Single/Type/GetUser

Gets the user's custom types that can be assigned to single transactions.

Responses:

- OK (200)
  - List:
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[POST\] /Transaction/Single/Type/Add

Adds a custom user transaction type. \
Duplicate names are not allowed among active types.

Request:

- Name (string)

Responses:

- OK (200)
- Not Found (404)
- Bad Request (400)
- Unauthorized (401)

### \[PUT\] /Transaction/Single/Type/Edit

Updates a custom user transaction type. \
Duplicate names are not allowed among active types.

Request:

- Id (int)
- Name (string)

Responses:

- OK (200)
- Not Found (404)
- Bad Request (400)
- Unauthorized (401)

### \[DELETE\] /Transaction/Single/Type/Delete

Deletes a custom user transaction type.

Request:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

## Single Transactions

### \[GET\] /Transaction/Single/GetAll

Gets the user's single transactions. Has optional type and date range filters.

ToDo: How to include filter for category that's both optional and allows user to filter for transactions without a category?

Request:

- TypeId (int?)
- StartDateUTC (DateTime?)
- EndDateUTC (DateTime?)

Response:

- OK (200)
  - List:
    - Id (int)
    - TypeId (int)
    - Amount (decimal)
    - TransactionDateUTC (DateTime)
    - CategoryId (int?)
- Unauthorized (401)

### \[POST\] /Transaction/Single/Add

Adds a single transaction.

Request:

- TypeId (int)
- Amount (decimal)
- TransactionDateUTC (DateTime)
- Category (int?)

Responses:

- Created (201)
- Unauthorized (401)

### \[PUT\] /Transaction/Single/Update

Updates a single transaction.

Request:

- Id (int)
- TypeId (int)
- Amount (decimal)
- TransactionDateUTC (DateTime)
- Category (int?)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

### \[DELETE\] /Transaction/Single/Delete

Deletes a non-recurring transaction.

Request:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

## Recurring Transactions

### \[GET\] /Transaction/Recurring/PeriodType

Gets the possible period types for recurring transactions, which come from a C# enum.

Response:

- OK (200)
  - List:
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[GET\] /Transaction/Recurring/GetAll

Gets the user's single transactions. Has optional date range filters.

ToDo: How to include filter for category that's both optional and allows user to filter for transactions without a category?

Request:

- StartDateUTC (DateTime?)
- EndDateUTC (DateTime?)

Response:

- OK (200)
  - List:
    - Id (int)
    - Name (string)
    - Amount (decimal)
    - PeriodId (int)
    - TimesPerPeriod (int)
    - CategoryId (int?)
    - StartDateUTC (DateTime)
    - EndDateUTC (DateTime?)
- Unauthorized (401)

### \[POST\] /Transaction/Recurring/Add

Adds a recurring transaction.

Request:

- Name (string)
- Amount (decimal)
- PeriodId (int)
- TimesPerPeriod (int)
- CategoryId (int?)
- StartDateUTC (DateTime)
- EndDateUTC (DateTime?)

Responses:

- Created (201)
- Unauthorized (401)

### \[PUT\] /Transaction/Recurring/Update

Updates a recurring transaction.

Request:

- Id (int)
- Name (string)
- Amount (decimal)
- PeriodId (int)
- TimesPerPeriod (int)
- CategoryId (int?)
- StartDateUTC (DateTime)
- EndDateUTC (DateTime?)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

### \[DELETE\] /Transaction/Recurring/Delete

Deletes a recurring transaction.

Request:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

## Transaction Queries

### \[GET\] Transaction/Average/ByPeriod

Get the current average transactions for a period type. \
Recurring transactions are listed individually. Single transactions are averaged, grouped by transaction type.

Request:

- PeriodType (enum)
  - Month
  - Year

Responses:

- OK (200)
  - List:
    - Category
    - Name
      - Either name of recurring transaction, or type name for averaged single transactions
    - Amount
- Unauthorized (401)

### \[GET\] Transaction/Average/ByDateRange

Get the current average transactions for a date range. \
Recurring transactions are listed individually. Single transactions are averaged, grouped by transaction type.

Request:

- StartDate (DateTime)
- EndDate (DateTime?)

Responses:

- OK (200)
  - List:
    - Category
    - Name
      - Either name of recurring transaction, or type name for averaged single transactions
    - Amount
- Unauthorized (401)
