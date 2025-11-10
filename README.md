# Unemployment Insurance API

This project contains the Unemployment Insurance controllers for managing benefits, claims, and revenue.

## Project Structure

```
/workspace/
├── Batch.Service/                          # Main API service
│   └── Controllers/
│       └── UnemploymentInsurance/
│           ├── BenefitsController.cs       # Benefits management
│           ├── CommonController.cs         # Claims management
│           └── RevenueController.cs        # Revenue & tax management
├── Batch.BusinessLogic/                    # Business logic layer
├── Batch.BusinessLogic.Tests/              # Unit tests
├── Batch.DAL/                              # Data access layer
├── Batch.BatchProcess.Listener/            # Batch listener
├── Batch.BatchProcess.Producer/            # Batch producer
├── Batch.BatchProducer.Service/            # Batch producer service
└── Ui.Shared/                              # Shared models and DTOs
    ├── Models/
    │   └── UnemploymentInsuranceClaim.cs
    └── DTOs/
        └── UnemploymentInsuranceDTOs.cs
```

## Controllers

### 1. BenefitsController
**Route:** `/api/unemployment-insurance/benefits`

Manages unemployment insurance benefit payments and approvals.

**Endpoints:**
- `GET /claim/{claimId}` - Get benefits by claim ID
- `GET /{benefitId}` - Get specific benefit
- `POST /process` - Process new benefit payment
- `PUT /approve` - Approve/reject benefit
- `GET /pending` - Get pending benefits
- `GET /history/{claimantId}` - Get benefit payment history
- `DELETE /{benefitId}` - Cancel benefit

### 2. CommonController
**Route:** `/api/unemployment-insurance/common`

Manages unemployment insurance claims and common operations.

**Endpoints:**
- `GET /claims` - Get all claims (with pagination)
- `GET /claims/{claimId}` - Get specific claim
- `GET /claims/claimant/{claimantId}` - Get claims by claimant
- `POST /claims` - Create new claim
- `PUT /claims/{claimId}` - Update claim
- `DELETE /claims/{claimId}` - Delete claim
- `GET /statistics` - Get claim statistics
- `GET /search` - Search claims
- `GET /health` - Health check

### 3. RevenueController
**Route:** `/api/unemployment-insurance/revenue`

Manages unemployment insurance revenue and employer tax contributions.

**Endpoints:**
- `GET /` - Get all revenue records
- `GET /{revenueId}` - Get specific revenue record
- `GET /employer/{employerId}` - Get revenue by employer
- `POST /` - Record new revenue
- `PUT /payment` - Update payment status
- `GET /overdue` - Get overdue payments
- `GET /statistics` - Get revenue statistics
- `GET /fiscal-period/{fiscalPeriod}` - Get revenue by fiscal period
- `GET /calculate-tax` - Calculate tax amount
- `GET /report` - Generate revenue report
- `DELETE /{revenueId}` - Delete revenue record

## Models

### UnemploymentInsuranceClaim
- ClaimId, ClaimantId, FilingDate
- WeeklyBenefitAmount, MaximumBenefitAmount
- ClaimStatus, ClaimType, etc.

### UnemploymentInsuranceBenefit
- BenefitId, ClaimId, WeekEndingDate
- BenefitAmount, PaymentStatus
- IsApproved, ApprovedBy, etc.

### UnemploymentInsuranceRevenue
- RevenueId, EmployerId, EmployerName
- Quarter, Year, TaxableWages
- TaxRate, TaxAmount, PaymentStatus, etc.

## DTOs

### Request DTOs
- `CreateUnemploymentClaimDto`
- `UpdateUnemploymentClaimDto`
- `ProcessBenefitDto`
- `ApproveBenefitDto`
- `RecordRevenueDto`
- `UpdateRevenuePaymentDto`

### Response DTO
- `UnemploymentInsuranceResponseDto<T>` - Generic response wrapper

## Next Steps

1. **Implement Business Logic Services:**
   - Create service interfaces in `Batch.BusinessLogic`
   - Implement business rules and validation
   - Connect to data access layer

2. **Data Access Layer:**
   - Create repositories in `Batch.DAL`
   - Set up database context and migrations
   - Implement CRUD operations

3. **Testing:**
   - Write unit tests in `Batch.BusinessLogic.Tests`
   - Add integration tests for controllers
   - Test validation and error handling

4. **Configuration:**
   - Set up dependency injection in `Startup.cs` or `Program.cs`
   - Configure database connection strings
   - Add authentication and authorization

## Technologies

- .NET 6.0
- ASP.NET Core Web API
- Entity Framework Core (for DAL)
- Swagger/OpenAPI for API documentation

## Notes

All controllers have TODO comments indicating where business logic services should be injected and implemented. The current implementation provides the API structure with placeholder responses.
