# API Design

- [API Design](#api-design)
  - [General Notes](#general-notes)
  - [Users](#users)
    - [\[POST\] /User/Register](#post-userregister)
    - [\[PUT\] /User/Verify](#put-userverify)
    - [\[POST\] /User/Login](#post-userlogin)
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

All methods require a JWT token other than the user registration, verification, and login methods.

## Users

### \[POST\] /User/Register

Register a new user. \
Emails must be unique.

Header:

- Email (string)
- Password (string)

Responses:

- Created (201)
  - Token (string)
- Bad Request (400)

### \[PUT\] /User/Verify

Verifies a user.

Header:

- Token (string)

Responses:

- OK (200)
  - Token (string)
- Not Found (404)
- Bad Request (400)

### \[POST\] /User/Login

Login a user

Header:

- Email (string)
- Password (string)

Responses:

- OK (200)
  - Token (string)
- Bad Request (400)

## Transaction Types

### \[GET\] /Type/GetAll

Gets the user's transaction types. \
The response value *InUse* is true for a type if it's assigned to any transactions.

Header: None

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

Header:

- Name (string)

Responses:

- Created (201)
- Bad Request (400)
- Unauthorized (401)

### \[PUT\] /Type/Edit

Edits a user transaction type. \
Names must be unique.

Header:

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

Header:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Bad Request (400)
- Unauthorized (401)

## Transaction Categories

### \[GET\] /Category/GetAll

Gets the user's transaction categories.

Header: None

Responses:

- OK (200)
  - List
    - Id (int)
    - Name (string)
- Unauthorized (401)

### \[POST\] /Category/Add

Adds a user transaction category.

Header:

- Name (string)

Responses:

- Created (201)
- Bad Request (400)
- Unauthorized (401)

### \[PUT\] /Category/Edit

Edits a user transaction category.

Header:

- Id (int)
- Name (string)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

### \[DELETE\] /Category/Delete

Deletes a user category.

Header:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

## Single Transactions

### \[POST\] /Transaction/Single/Add

Adds a single transaction.

Header:

- TypeId (int)
- Amount (decimal)
- TransactionDateUTC (DateTime)
- Category (int?)

Responses:

- Created (201)
- Unauthorized (401)

### \[PUT\] /Transaction/Single/Update

Updates a single transaction.

Header:

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

Header:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

## Recurring Transactions

### \[POST\] /Transaction/Recurring/Add

Adds a recurring transaction.

Header:

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

Header:

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

Header:

- Id (int)

Responses:

- OK (200)
- Not Found (404)
- Unauthorized (401)

## Transaction Queries

### \[GET\] /Transaction/GetAll

Get all transactions. Can filter using date range.

Header:

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

Header:

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

Header:

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
