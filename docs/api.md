# API Design

- [General Notes](#general-notes)
- [Users](#users)
  - [\[GET\] /user/register/password-requirements](#get-userregisterpassword-requirements)
  - [\[POST\] /user/register](#post-userregister)
  - [\[POST\] /user/register/verify/](#post-userregisterverify)
  - [\[POST\] /user/register/verify/resend](#post-userregisterverifyresend)
  - [\[POST\] /user/login](#post-userlogin)
  - [\[POST\] /user/login/verify](#post-userloginverify)
  - [\[POST\] /user/logout](#post-userlogout)
  - [\[POST\] /user/password/change](#post-userpasswordchange)
  - [\[POST\] /user/password/forgot](#post-userpasswordforgot)
  - [\[POST\] /user/password/reset](#post-userpasswordreset)
  - [\[POST\] /user/refresh-access](#post-userrefresh-access)
- [Transaction Categories](#transaction-categories)
  - [\[GET\] /transaction/category](#get-transactioncategory)
  - [\[POST\] /transaction/category](#post-transactioncategory)
  - [\[PUT\] /transaction/category](#put-transactioncategory)
  - [\[DELETE\] /transaction/category](#delete-transactioncategory)
- [Single Transaction Types](#single-transaction-types)
  - [\[GET\] /transaction/single/type/system](#get-transactionsingletypesystem)
  - [\[GET\] /transaction/single/type/user](#get-transactionsingletypeuser)
  - [\[POST\] /transaction/single/type/user](#post-transactionsingletypeuser)
  - [\[PUT\] /transaction/single/type/user](#put-transactionsingletypeuser)
  - [\[DELETE\] /transaction/single/type/user](#delete-transactionsingletypeuser)
- [Single Transactions](#single-transactions)
  - [\[GET\] /transaction/single](#get-transactionsingle)
  - [\[POST\] /transaction/single](#post-transactionsingle)
  - [\[PUT\] /transaction/single](#put-transactionsingle)
  - [\[DELETE\] /transaction/single](#delete-transactionsingle)
- [Recurring Transactions](#recurring-transactions)
  - [\[GET\] /transaction/recurring/period-type](#get-transactionrecurringperiod-type)
  - [\[GET\] /transaction/recurring](#get-transactionrecurring)
  - [\[POST\] /transaction/recurring](#post-transactionrecurring)
  - [\[PUT\] /transaction/recurring](#put-transactionrecurring)
  - [\[DELETE\] /transaction/recurring](#delete-transactionrecurring)
- [Transaction Queries](#transaction-queries)
  - [\[GET\] /transaction/average/by-period](#get-transactionaverageby-period)
  - [\[GET\] /transaction/average/by-date-range](#get-transactionaverageby-date-range)

## General Notes

- The API is implemented in ASP.NET Core.
- Most methods require a JWT bearer access token.

## Users

### \[GET\] /user/register/password-requirements

Gets the requirements for a user's password.

Path Params: None

Query Params: None

Request: None

Responses:

- OK (200):
  - requireDigit (bool)
  - requireLowercase (bool)
  - requireNonAlphanumeric (bool)
  - requireUppercase (bool)
  - requiredLength (int)

### \[POST\] /user/register

Register a new user. An email is sent to the provided email address for verification, containing a one-time verification token that lasts for a set amount of time. \
Response is intentionally vague to avoid revealing whether an email is taken or not.

Path Params: None

Query Params: None

Request:

- Email (string)
- Password (string)

Responses:

- Accepted (202)

*Does not require a bearer access token.*

### \[POST\] /user/register/verify/

Verifies a user's registration. Used through a verification link in an email. \
The user is logged in after a successful registration. The verification may fail if the verification token expired; the user may request a new verification email.

Path Params:

- RegistrationToken (string)

Query Params: None

Request: None

Responses:

- OK (200)
  - AccessToken (string)
- Bad Request (400)

*Does not require a bearer access token.*

### \[POST\] /user/register/verify/resend

Requests a new verification email for an unverified user. \
Response is intentionally vague to avoid revealing if the email exists or is verified.

Path Params: None

Query Params: None

Request:

- Email (string)

Response:

- Accepted (202)

*Does not require a bearer access token.*

### \[POST\] /user/login

Starts logging in a user. Creates a 2FA code sent to the user's email address, and a login token used by */user/login/verify*.

Path Params: None

Query Params: None

Request:

- Email (string)
- Password (string)

Responses:

- OK (200)
  - LoginToken (string)

*Does not require a bearer access token.*

### \[POST\] /user/login/verify

Completes the 2FA login. Returns an access token, which is the actual bearer access token, and a refresh token.

Path Params: None

Query Params: None

Request:

- LoginToken (string)
- LoginCode (string)

Responses:

- OK (200)
  - AccessToken (string)
  - RefreshToken (string)

*Does not require a bearer access token.*

### \[POST\] /user/logout

Logs the user out, invalidating their refresh token on the server.

Path Params: None

Query Params: None

Request: None

Responses:

- No Content (204)
- Unauthorized (401)

### \[POST\] /user/password/change

Changes the (logged in) user's password.

Path Params: None

Query Params: None

Request:

- CurrentPassword (string)
- NewPassword (string)

Responses:

- No Content (204)
- Unauthorized (401)

### \[POST\] /user/password/forgot

Allows the user to reset their password. A reset link is sent to their email containing a reset token.

Path Params: None

Query Params: None

Request:

- Email (string)

Response:

- Accepted (202)

*Does not require a bearer access token.*

### \[POST\] /user/password/reset

Completes the password reset process.

Path Params:

- ResetToken (string)

Query Params: None

Request:

- NewPassword (string)

Response:

- No Content (204)

*Does not require a bearer access token.*

### \[POST\] /user/refresh-access

Refreshes the user's access and refresh tokens using their current refresh token. Periodically called by the front end, e.g. when another API method returns *Unauthorized*.

Path Params: None

Query Params: None

Request:

- RefreshToken (string)

Response:

- OK (200)
  - AccessToken (string)
  - RefreshToken (string)
- Bad Request (400)

*Does not require a bearer access token.*

## Transaction Categories

### \[GET\] /transaction/category

Gets the user's transaction categories.

Path Params: None

Query Params: None

Request: None

Responses:

- OK (200)
  - List
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[POST\] /transaction/category

Adds a user transaction category. \
Duplicate names are not allowed.

Path Params: None

Query Params: None

Request:

- Name (string)

Responses:

- Created (201)
  - Id (int)
- Bad Request (400)
- Unauthorized (401)

### \[PUT\] /transaction/category

Edits a user transaction category. \
Duplicate names are not allowed.

Path Params:

- Id (int)

Query Params: None

Request:

- Name (string)

Responses:

- OK (200)
- Not Found (404)
- Bad Request (400)
- Unauthorized (401)

### \[DELETE\] /transaction/category

Deletes a user category.

Path Params:

- Id (int)

Query Params: None

Request: None

Responses:

- No Content (204)
- Not Found (404)
- Unauthorized (401)

## Single Transaction Types

### \[GET\] /transaction/single/type/system

Gets the system types that can be assigned to single transactions.

Path Params: None

Query Params: None

Request: None

Responses:

- OK (200)
  - List:
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[GET\] /transaction/single/type/user

Gets the user's custom types that can be assigned to single transactions.

Path Params: None

Query Params: None

Request: None

Responses:

- OK (200)
  - List:
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[POST\] /transaction/single/type/user

Adds a custom user transaction type. \
Duplicate names are not allowed among active types.

Path Params: None

Query Params: None

Request:

- Name (string)

Responses:

- Created (201)
  - Id (int)
- Bad Request (400)
- Unauthorized (401)

### \[PUT\] /transaction/single/type/user

Updates a custom user transaction type. \
Duplicate names are not allowed among active types.

Path Params:

- Id (int)

Query Params: None

Request:

- Name (string)

Responses:

- OK (200)
- Not Found (404)
- Bad Request (400)
- Unauthorized (401)

### \[DELETE\] /transaction/single/type/user

Deletes a custom user transaction type.

Path Params:

- Id (int)

Query Params: None

Request: None

Responses:

- No Content (204)
- Not Found (404)
- Unauthorized (401)

## Single Transactions

### \[GET\] /transaction/single

Gets the user's single transactions. Has optional type and date range filters. \
If the *Uncategorized* parameter is present, it controls whether uncategorized transactions are included or not.

Path Params: None

Query Params:

- Page (int)
- PageSize (int)
- TypeId (int?)
- CategoryId (int?)
- Uncategorized (bool?)
- StartDateUTC (DateTime?)
- EndDateUTC (DateTime?)

Request: None

Response:

- OK (200)
  - Transactions (List):
    - Id (int)
    - TypeId (int)
    - Amount (decimal)
    - TransactionDateUTC (DateTime)
    - CategoryId (int?)
  - Page (int)
  - MaxPages (int)
- Unauthorized (401)

### \[POST\] /transaction/single

Adds a single transaction.

Path Params: None

Query Params: None

Request:

- TypeId (int)
- Amount (decimal)
- TransactionDateUTC (DateTime)
- CategoryId (int?)

Responses:

- Created (201)
  - Id (int)
- Unauthorized (401)

### \[PUT\] /transaction/single

Updates a single transaction.

Path Params:

- Id (int)

Query Params: None

Request:

- TypeId (int)
- Amount (decimal)
- TransactionDateUTC (DateTime)
- CategoryId (int?)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

### \[DELETE\] /transaction/single

Deletes a non-recurring transaction.

Path Params:

- Id (int)

Query Params: None

Request: None

Responses:

- No Content (204)
- Not Found (404)
- Unauthorized (401)

## Recurring Transactions

### \[GET\] /transaction/recurring/period-type

Gets the possible period types for recurring transactions, which come from a C# enum.

Path Params: None

Query Params: None

Request: None

Response:

- OK (200)
  - List:
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[GET\] /transaction/recurring

Gets the user's recurring transactions. Has optional date range filters. \
If *Uncategorized* parameter is present, it controls whether uncategorized transactions are included or not.

Path Params: None

Query Params:

- Page (int)
- PageSize (int)
- CategoryId (int?)
- Uncategorized (bool?)
- StartDateUTC (DateTime?)
- EndDateUTC (DateTime?)

Request: None

Response:

- OK (200)
  - Transactions (List):
    - Id (int)
    - Name (string)
    - Amount (decimal)
    - PeriodId (int)
    - TimesPerPeriod (int)
    - CategoryId (int?)
    - StartDateUTC (DateTime)
    - EndDateUTC (DateTime?)
  - Page (int)
  - MaxPages (int)
- Unauthorized (401)

### \[POST\] /transaction/recurring

Adds a recurring transaction.

Path Params: None

Query Params: None

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
  - Id (int)
- Unauthorized (401)

### \[PUT\] /transaction/recurring

Updates a recurring transaction.

Path Params:

- Id (int)

Query Params: None

Request:

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

### \[DELETE\] /transaction/recurring

Deletes a recurring transaction.

Path Params:

- Id (int)

Query Params: None

Request: None

Responses:

- No Content (204)
- Not Found (404)
- Unauthorized (401)

## Transaction Queries

### \[GET\] /transaction/average/by-period

Get the current average transactions for a period type. \
Recurring transactions are listed individually. Single transactions are averaged, grouped by transaction type.

Path Params:

- PeriodTypeId (int / enum)

Query Params: None

Request: None

Responses:

- OK (200)
  - List:
    - CategoryId (int?)
    - Name (string)
      - Either name of recurring transaction, or type name for averaged single transactions
    - Amount (decimal)
- Unauthorized (401)

### \[GET\] /transaction/average/by-date-range

Get the current average transactions for a date range. \
Recurring transactions are listed individually. Single transactions are averaged, grouped by transaction type.

Path Params: None

Query Params:

- StartDate (DateTime)
- EndDate (DateTime?)

Request: None

Responses:

- OK (200)
  - List:
    - CategoryId (int?)
    - Name (string)
      - Either name of recurring transaction, or type name for averaged single transactions
    - Amount (decimal)
- Unauthorized (401)
