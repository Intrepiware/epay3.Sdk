# epay3 SDK Implementation TODO

This document tracks the implementation progress for the epay3 SDK based on the public API swagger documentation.

## ✅ Completed

### Core Resources
- **Tokens Resource** (`/api/v1/tokens`)
  - ✅ GET `/{id}` - Get token details
  - ✅ POST - Create token
  - ✅ DELETE `/{id}` - Delete token

- **Transactions Resource** (`/api/v1/transactions`)
  - ✅ GET `/{id}` - Get transaction details
  - ✅ POST - Create transaction
  - ✅ POST `/authorize` - Authorize transaction
  - ✅ POST `/{id}/refund` - Refund transaction
  - ✅ POST `/{id}/void` - Void transaction

- **Payment Schedules Resource** (`/api/v1/paymentSchedules`)
  - ✅ GET `/{id}` - Get payment schedule details
  - ✅ POST - Create payment schedule
  - ✅ POST `/{id}/cancel` - Cancel payment schedule

- **AutoPay Resource** (`/api/v1/autoPay`)
  - ✅ GET `/{id}` - Get AutoPay details
  - ✅ GET `/autoPays` - Search AutoPays (with filters)
  - ✅ POST - Create AutoPay
  - ✅ POST `/{id}/cancel` - Cancel AutoPay
  - ✅ POST `/{id}/restart` - Restart AutoPay

- **Transaction Fees Resource** (`/api/v1/transactionFees`)
  - ✅ GET - Calculate/retrieve transaction fees

### Infrastructure
- ✅ camelCase JSON serialization
- ✅ String enum serialization
- ✅ Verbose HTTP logging (optional)
- ✅ Empty response handling (201 with Location header)
- ✅ Integration tests with fuzzing helpers

---

## ✅ Phase 1 - Core Payment Features (COMPLETED)

### Transaction Fees Resource (`/api/v1/transactionFees`)
- ✅ GET - Calculate/retrieve transaction fees
- ✅ Create `TransactionFeesResource.cs`
- ✅ Create response models (`GetTransactionFeesResponse`)
- ✅ Write integration tests (`TransactionFeesResourceTests`)

---

## 📋 TODO: Phase 2 - Invoice Management

### Invoices Resource (`/api/v1/Invoices`)
- [ ] POST - Create invoice
- [ ] POST `/Email` - Email invoice
- [ ] POST `/Payments` - Process invoice payment
- [ ] Create `InvoicesResource.cs`
- [ ] Create request/response models:
  - [ ] `CreateInvoiceRequest`
  - [ ] `EmailInvoiceRequest`
  - [ ] `ProcessInvoicePaymentRequest`
  - [ ] Related models (InvoiceModel, InvoiceItemModel, etc.)
- [ ] Write integration tests

### Managed Invoices Resource (`/api/v1/managedInvoices`)
- [ ] GET `/{id}` - Get managed invoice
- [ ] GET - Search managed invoices
- [ ] POST - Create managed invoice
- [ ] PUT `/{id}` - Update managed invoice
- [ ] DELETE `/{id}` - Delete managed invoice
- [ ] POST `/{id}/void` - Void managed invoice
- [ ] POST `/{id}/finance` - Finance managed invoice
- [ ] Create `ManagedInvoicesResource.cs`
- [ ] Create request/response models:
  - [ ] `GetManagedInvoiceResponse`
  - [ ] `SearchManagedInvoicesRequest`/`Response`
  - [ ] `CreateManagedInvoiceRequest`
  - [ ] `UpdateManagedInvoiceRequest`
  - [ ] `FinanceManagedInvoiceRequest`
- [ ] Write integration tests

---

## 📋 TODO: Phase 3 - Supporting Features

### Batches Resource (`/api/v1/batches`)
- [ ] GET - Get batches with date filters
- [ ] Create `BatchesResource.cs`
- [ ] Create request/response models:
  - [ ] `GetBatchesRequest`
  - [ ] `GetBatchesResponse`
  - [ ] `BatchListItemModel`
- [ ] Write integration tests

### Token Page Sessions Resource (`/api/v1/tokenPageSessions`)
- [ ] POST - Create token page session (hosted payment page)
- [ ] Create `TokenPageSessionsResource.cs`
- [ ] Create request/response models:
  - [ ] `CreateTokenPageSessionRequest`
  - [ ] `CreateTokenPageSessionResponse`
- [ ] Write integration tests

### IVR Sessions Resource (`/api/v1/ivrSessions`)
- [ ] POST - Create IVR session (phone payments)
- [ ] Create `IvrSessionsResource.cs`
- [ ] Create request/response models:
  - [ ] `CreateIvrSessionRequest`
  - [ ] `CreateIvrSessionResponse`
- [ ] Write integration tests

---

## 📝 Notes

- **Priority levels are suggestions** - Implement in whatever order makes sense for your use case
- All new resources should follow existing patterns:
  - camelCase JSON serialization
  - String enum conversion
  - Fuzzing helpers in tests to avoid duplicate detection
  - Comprehensive XML documentation
  - Integration tests for all endpoints

## 🔧 Known Issues / Tech Debt

- None currently

## 🎯 Future Enhancements

- Consider adding retry logic for transient failures
- Add bulk operations support (if API supports it)
- Add webhook signature verification utilities
- Generate SDK from swagger automatically for future versions
