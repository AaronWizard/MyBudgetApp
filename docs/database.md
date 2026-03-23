# Database Design

- [General notes](#general-notes)
- [Tables](#tables)
  - [User](#user)
  - [TransactionType](#transactiontype)
  - [TransactionCategory](#transactioncategory)
  - [SingleTransaction](#singletransaction)
  - [TransactionPeriod](#transactionperiod)
  - [RecurringTransaction](#recurringtransaction)

## General notes

- C# syntax is used in this document. Type names with a "?" represent nullable fields.
- Many tables have *CreatedDateUTC*, *UpdatedDateUTC*, and *DeletedDateUTC* fields for auditing purposes. Records with a non-null *DeletedDateUTC* field are considered deleted.

## Tables

### User

The **User** table stores the users of the app, who log in and record their transactions. Users log in with an email and password. Registration requires the user to click a link in a confirmation email. Users with a non-null *DeletedDateUTC* field are considered deleted / deactivated. Duplicate emails are not allowed, even with deactivated users.

- User
  - Id (int)
  - Email (string)
  - PasswordHash (string)
  - EmailConfirmed (bool)
  - CreatedDateUTC (DateTime)
  - UpdatedDateUTC (DateTime?)
  - DeletedDateUTC (DateTime?)

### TransactionType

The **TransactionType** table contains the types for single transactions. The application comes with a set of default system types while the user may add their own types; user types have a non-null *UserId* value. A transaction type may not be deleted if it's assigned to a transaction. Transaction types are soft deleted to preserve data integrity with deleted transactions. Duplicate names are not allowed among the system types and a user's active types.

- TransactionType
  - Id (int)
  - UserId (int?) *(Users/Id)*
  - Name (string)
  - CreatedDateUTC (DateTime)
  - UpdatedDateUTC (DateTime?)
  - DeletedDateUTC (DateTime?)

The system transaction types are:

- Groceries
- Utilities
- Entertainment

### TransactionCategory

The **TransactionCategory** table contains user-custom categories for different transactions. Categories may be used by both single transactions and recurring transactions. Unlike types, categories are optional and there are no system-defined categories.

- TransactionCategory
  - Id (int)
  - UserId (int) *(Users/Id)*
  - Name (string)
  - CreatedDateUTC (DateTime)
  - UpdatedDateUTC (DateTime?)
  - DeletedDateUTC (DateTime?)

### SingleTransaction

The *SingleTransaction* table represents a single, ad-hoc, non-recurring payment or expense.

- SingleTransaction
  - Id (int)
  - UserId (int) *(Users/Id)*
  - TypeId (int) *(TransactionType/Id)*
  - Amount (decimal)
  - TransactionDateUTC (DateTime)
  - CategoryId (int?) *(TransactionCategory/Id)*
  - CreatedDateUTC (DateTime)
  - UpdatedDateUTC (DateTime?)
  - DeletedDateUTC (DateTime?)

### TransactionPeriod

The *TransactionPeriod* table is an enum table for categorizing types of time periods for recurring transactions.

- TransactionPeriod *(lookup)*
  - Id (int)
  - Name (string)

The possible records in TransactionPeriod are:

- Monthly
- Yearly
- Biweekly

### RecurringTransaction

The *RecurringTransaction* table represents recurring transactions. They include information on how often they occur represented by their period type (monthly, yearly, etc.) and how often they occur in that period (e.g. once a month vs twice a month). Duplicate names are not allowed among a user's active recurring transactions.

Recurring transactions do not have types like single transactions do. The Name field serves as the identifying label for the transaction concept itself (e.g. "Rent" or "Salary"). A type is only relevant for single transactions where categorization for reporting is needed. Essentially, a recurring transaction is a transaction type unto itself.

- RecurringTransaction
  - Id (int)
  - UserId (int) *(Users/Id)*
  - Name (string)
  - Amount (decimal)
  - PeriodId (int) *(TransactionPeriod/Id)*
  - TimesPerPeriod (int)
  - CategoryId (int?) *(TransactionCategory/Id)*
  - StartDateUTC (DateTime)
  - EndDateUTC (DateTime?)
  - CreatedDateUTC (DateTime)
  - UpdatedDateUTC (DateTime?)
  - DeletedDateUTC (DateTime?)
