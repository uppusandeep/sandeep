using System;
using System.ComponentModel.DataAnnotations;

namespace Ui.Shared.DTOs
{
    /// <summary>
    /// DTO for creating a new unemployment insurance claim
    /// </summary>
    public class CreateUnemploymentClaimDto
    {
        [Required]
        public string ClaimantId { get; set; }
        
        [Required]
        public DateTime FilingDate { get; set; }
        
        public DateTime? EffectiveDate { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal WeeklyBenefitAmount { get; set; }
        
        [Required]
        public string ClaimType { get; set; }
        
        [Required]
        public string ReasonForUnemployment { get; set; }
    }

    /// <summary>
    /// DTO for updating unemployment insurance claim
    /// </summary>
    public class UpdateUnemploymentClaimDto
    {
        public string ClaimStatus { get; set; }
        public decimal? WeeklyBenefitAmount { get; set; }
        public decimal? MaximumBenefitAmount { get; set; }
        public int? BenefitYearWeeks { get; set; }
    }

    /// <summary>
    /// DTO for processing unemployment benefits
    /// </summary>
    public class ProcessBenefitDto
    {
        [Required]
        public int ClaimId { get; set; }
        
        [Required]
        public DateTime WeekEndingDate { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal BenefitAmount { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; }
        
        public string Notes { get; set; }
    }

    /// <summary>
    /// DTO for approving benefits
    /// </summary>
    public class ApproveBenefitDto
    {
        [Required]
        public int BenefitId { get; set; }
        
        [Required]
        public bool IsApproved { get; set; }
        
        [Required]
        public string ApprovedBy { get; set; }
        
        public string Notes { get; set; }
    }

    /// <summary>
    /// DTO for recording unemployment insurance revenue
    /// </summary>
    public class RecordRevenueDto
    {
        [Required]
        public string EmployerId { get; set; }
        
        [Required]
        public string EmployerName { get; set; }
        
        [Required]
        [Range(1, 4)]
        public int Quarter { get; set; }
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal TaxableWages { get; set; }
        
        [Required]
        [Range(0, 100)]
        public decimal TaxRate { get; set; }
    }

    /// <summary>
    /// DTO for updating revenue payment status
    /// </summary>
    public class UpdateRevenuePaymentDto
    {
        [Required]
        public int RevenueId { get; set; }
        
        [Required]
        public DateTime PaidDate { get; set; }
        
        [Required]
        public string PaymentStatus { get; set; }
    }

    /// <summary>
    /// Common response DTO
    /// </summary>
    public class UnemploymentInsuranceResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
