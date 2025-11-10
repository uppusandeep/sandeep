using System;
using System.Collections.Generic;

namespace Ui.Shared.Models
{
    /// <summary>
    /// Represents an unemployment insurance claim
    /// </summary>
    public class UnemploymentInsuranceClaim
    {
        public int ClaimId { get; set; }
        public string ClaimantId { get; set; }
        public DateTime FilingDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string ClaimStatus { get; set; }
        public decimal WeeklyBenefitAmount { get; set; }
        public decimal MaximumBenefitAmount { get; set; }
        public int BenefitYearWeeks { get; set; }
        public string ClaimType { get; set; }
        public string ReasonForUnemployment { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    /// <summary>
    /// Represents unemployment insurance benefits information
    /// </summary>
    public class UnemploymentInsuranceBenefit
    {
        public int BenefitId { get; set; }
        public int ClaimId { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public decimal BenefitAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// Represents revenue/tax information related to unemployment insurance
    /// </summary>
    public class UnemploymentInsuranceRevenue
    {
        public int RevenueId { get; set; }
        public string EmployerId { get; set; }
        public string EmployerName { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
        public decimal TaxableWages { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string PaymentStatus { get; set; }
        public string FiscalPeriod { get; set; }
    }
}
