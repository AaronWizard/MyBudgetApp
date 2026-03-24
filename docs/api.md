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
- [Transaction Types](#transaction-types)
  - [\[GET\] /Type/GetAll](#get-typegetall)
  - [\[POST\] /Type/Add](#post-typeadd)
  - [\[PUT\] /Type/Edit](#put-typeedit)
  - [\[DELETE\] /Type/Delete](#delete-typedelete)
- [Transaction Categories](#transaction-categories)
  - [\[GET\] /Category/GetAll](#get-categorygetall)
  - [\[POST\] /Category/Add](#post-categoryadd)
  - [\[PUT\] /Category/Edit](#put-categoryedit)
  - [\[DELETE\] /Category/Delete](#delete-categorydelete)
- [Single Transactions](#single-transactions)
  - [\[POST\] /Transaction/Single/Add](#post-transactionsingleadd)
  - [\[PUT\] /Transaction/Single/Update](#put-transactionsingleupdate)
  - [\[DELETE\] /Transaction/Single/Delete](#delete-transactionsingledelete)
- [Recurring Transactions](#recurring-transactions)
  - [\[POST\] /Transaction/Recurring/Add](#post-transactionrecurringadd)
  - [\[PUT\] /Transaction/Recurring/Update](#put-transactionrecurringupdate)
  - [\[DELETE\] /Transaction/Recurring/Delete](#delete-transactionrecurringdelete)
- [Transaction Queries](#transaction-queries)
  - [\[GET\] /Transaction/GetAll](#get-transactiongetall)
  - [\[GET\] Transaction/Average/ByPeriod](#get-transactionaveragebyperiod)
  - [\[GET\] Transactions/Average/ByDateRange](#get-transactionsaveragebydaterange)

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

Requests a new verification email. \
Response is intentionally vague to avoid revealing whether an email is taken or not.

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

Completes the 2FA login.

Request:

- LoginToken (string)
- LoginCode (string)

Responses:

- OK (200)
  - AccessToken (string)

*Does not require a bearer access token.*

### \[POST\] /User/Logout

Logs the user out.

Responses:

- No Content (204)

### \[POST\] /User/Password/Change

Changes the (logged in) user's password.

Request:

- CurrentPassword (string)
- NewPassword (string)

Responses:

- No Content (204)

### \[POST\] /User/Password/Forgot

Allows the user to reset their password. A reset link is sent to their email. A reset token is created and kept for a set amount of time.

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

Response:

- OK (200)
  - AccessToken (string)

*Does not require a bearer access token.*

## Transaction Types

### \[GET\] /Type/GetAll

Gets the user's transaction types. \
The response value *InUse* is true for a type if it's assigned to any transactions.

Request: None

Responses:

- OK (200)
  - List
    - Id (int)
    - Name (string)
    - InUse (bool)
- Unauthorized (401)

### \[POST\] /Type/Add

Adds a user transaction type. \
Names must be unique.

Request:

- Name (string)

Responses:

- Created (201)
- Bad Request (400)
- Unauthorized (401)

### \[PUT\] /Type/Edit

Edits a user transaction type. \
Names must be unique.

Request:

- Id (int)
- Name (string)

Responses:

- OK (200)
- Not Found (404)
- Bad Request (400)
- Unauthorized (401)

### \[DELETE\] /Type/Delete

Deletes a user type. \
A type may not be deleted if it's assigned to any transactions.

Request:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Bad Request (400)
- Unauthorized (401)

## Transaction Categories

### \[GET\] /Category/GetAll

Gets the user's transaction categories.

Request: None

Responses:

- OK (200)
  - List
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[POST\] /Category/Add

Adds a user transaction category.

Request:

- Name (string)

Responses:

- Created (201)
- Bad Request (400)
- Unauthorized (401)

### \[PUT\] /Category/Edit

Edits a user transaction category.

Request:

- Id (int)
- Name (string)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

### \[DELETE\] /Category/Delete

Deletes a user category.

Request:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

## Single Transactions

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

### \[POST\] /Transaction/Recurring/Add

Adds a recurring transaction.

Request:

- Name (string)
- Amount (decimal)
- PeriodType (int)
- TimesPerPeriod (int)
- StartDateUTC (DateTime)
- EndDateUTC (DateTime)
- Category (int?)

Responses:

- Created (201)
- Unauthorized (401)

### \[PUT\] /Transaction/Recurring/Update

Updates a recurring transaction.

Request:

- Id (int)
- Name (string)
- Amount (decimal)
- PeriodType (int)
- TimesPerPeriod (int)
- StartDateUTC (DateTime)
- EndDateUTC (DateTime)
- Category (int?)

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

### \[GET\] /Transaction/GetAll

Get all transactions. Can filter using date range.

Request:

- StartDateUTC (DateTime?)
- EndDateUTC (DateTime?)

Output:

- OK (200)
  - List:
    - Scheduled
      - Id (int)
      - Name (string)
      - Amount (decimal)
      - PeriodType (int)
      - TimesPerPeriod (int)
      - StartDateUTC (DateTime)
      - EndDateUTC (DateTime)
      - Category (int?)
    - Single
      - Id (int)
      - Name (string)
      - Amount (decimal)
      - TransactionDateUTC (DateTime)
      - Category (int?)
- Unauthorized (401)

### \[GET\] Transaction/Average/ByPeriod

Get the current average transactions for a period type. \
Recurring transactions are listed individually. Single transactions are averaged, grouped by type.

Request:

- PeriodType (enum)
  - Month
  - Year

Responses:

- OK (200)
  - List:
    - Category
    - Name
      - Either name of recurring transaction or type name for averaged single transactions
    - Amount
- Unauthorized (401)

### \[GET\] Transactions/Average/ByDateRange

Get the current average transactions for a date range. \
Recurring transactions are listed individually. Single transactions are averaged, grouped by type.

Request:

- StartDate (DateTime)
- EndDate (DateTime?)

Responses:

- OK (200)
  - List:
    - Category
    - Name
      - Either name of recurring transaction or type name for averaged single transactions
    - Amount
- Unauthorized (401)
