namespace Frieght.Api.Entities;

public class Invoice
{
    public int Id { get; set; }
    public required string InvoiceNumber { get; set; }
    public int LoadId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Today;
    public string Status { get; set; } = "Pending"; // Pending, Paid, Overdue
    public required string ShipperId { get; set; }
    public decimal AmountDue { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalVat { get; set; }
    public decimal Withholding { get; set; }
    public decimal ServiceFees { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? Note { get; set; }
    public string TransactionId { get; set; } = Guid.NewGuid().ToString();

    // Navigation Property
    public required PaymentMethod PaymentMethod { get; set; }
}
