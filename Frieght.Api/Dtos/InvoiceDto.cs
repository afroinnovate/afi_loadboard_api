namespace Frieght.Api.Dtos;

public class InvoiceDto
{
    public int? Id { get; set; }
    public required string InvoiceNumber { get; set; }
    public int LoadId { get; set; }
    public string IssueDate { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public required string CarrierId { get; set; }
    
    // Add carrier details
    public string? CarrierName { get; set; }
    public string? CarrierEmail { get; set; }
    public string? CarrierPhone { get; set; }
    public string? CarrierBusinessName { get; set; }

    // Keep the carrier reference for response mapping
    public decimal AmountDue { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalVat { get; set; }
    public decimal Withholding { get; set; }
    public decimal ServiceFees { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Note { get; set; }
    public string? TransactionId { get; set; }
    public string? TransactionDate { get; set; }
    public string? TransactionStatus { get; set; }
    public string? PaymentMethodId { get; set; }
    public PaymentMethodDto? PaymentMethod { get; set; }
}
