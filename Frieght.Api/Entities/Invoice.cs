namespace Frieght.Api.Entities;

public class Invoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int LoadId { get; set; }
    public DateTime IssueDate { get; set; }  // Required, defaults to current time
    public DateTime DueDate { get; set; }    // Required, defaults to current time + 30 days
    public string Status { get; set; } = string.Empty;
    public string CarrierId { get; set; } = string.Empty;
    public decimal AmountDue { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalVat { get; set; }
    public decimal Withholding { get; set; }
    public decimal ServiceFees { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? Note { get; set; }
    public string? TransactionId { get; set; }
    public DateTime? TransactionDate { get; set; }  // Optional
    public string? TransactionStatus { get; set; }
    public required string PaymentMethodId { get; set; }
    
    // Add carrier details
    public string? CarrierName { get; set; }
    public string? CarrierEmail { get; set; }
    public string? CarrierPhone { get; set; }
    public string? CarrierBusinessName { get; set; }
}
