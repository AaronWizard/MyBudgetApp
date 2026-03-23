# Database Design

## General notes

- C# syntax is used in this document. Type names with a "?" represent nullable fields.
- Many tables have *CreatedDateUTC*, *UpdatedDateUTC*, and *DeletedDateUTC* fields for auditing purposes. Records with a non-null *DeletedDateUTC* field are considered deleted.

## Tables

The **User** table stores the users of the app, who log in and record their transactions. Users log in with an email and password. Registration requires the user to click a link in a confirmation email. Users with a non-null *DeletedDateUTC* field are considered deleted / deactivated. Duplicate emails are not allowed, even with deactivated users.

- User
  - Id (int)
  - Email (string)
  - PasswordHash (string)
  - EmailConfirmed (bool)
  - CreatedDateUTC (DateTime)
  - UpdatedDateUTC (DateTime?)
  - DeletedDateUTC (DateTime?)

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

The **TransactionCategory** table contains user-custom categories for different transactions. Categories may be used by both single transactions and reoccurring transactions. Unlike types, categories are optional, may be deleted, and there are no system-defined categories. Deleting a category sets the categories of related transactions to null.

- TransactionCategory
  - Id (int)
  - UserId (int) *(Users/Id)*
  - Name (string)

The *SingleTransaction* table represents a single non-reoccurring payment or expense.

- SingleTransaction
  - Id (int)
  - UserId (int) *(Users/Id)*
  - TypeId (int) *(TransactionType/Id)*
  - Amount (decimal)
  - TransactionDateUTC (DateTime)
  - CategoryId (int?) *(Category/Id)*
  - CreatedDateUTC (DateTime)
  - UpdatedDateUTC (DateTime?)
  - DeletedDateUTC (DateTime?)

The *TransactionPeriod* table is an enum table for categorizing types of time periods for reoccurring transactions.

- TransactionPeriod *(lookup)*
  - Id (int)
  - Name (string)

The possible records in TransactionPeriod are:

- Monthly
- Yearly
- Biweekly

The *ReoccurringTransaction* table represents reoccurring transactions. They include information on how often they occur represented by their period type (monthly, yearly, etc.) and how often they occur in that period (e.g. once a month vs twice a month). They do not have a transaction type ID like single transactions do; logically a reoccurring transaction represents multiple single transactions of the same type. Duplicate names are not allowed among a user's active reoccurring transactions.

- ReoccurringTransaction
  - Id (int)
  - UserId (int) *(Users/Id)*
  - Name (string)
  - Amount (decimal)
  - PeriodId (int) *(TransactionPeriod/Id)*
  - TimesPerPeriod (int)
  - CategoryId (int?) *(Category/Id)*
  - StartDateUTC (DateTime)
  - EndDateUTC (DateTime?)
  - CreatedDateUTC (DateTime)
  - UpdatedDateUTC (DateTime?)
  - DeletedDateUTC (DateTime?)
