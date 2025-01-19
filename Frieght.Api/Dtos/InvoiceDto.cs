namespace Frieght.Api.Dtos;

public class InvoiceDto
{
    public int LoadId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Today;
    public string Status { get; set; }
    public int ShipperId { get; set; }
    public decimal AmountDue { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalVat { get; set; }
    public decimal Withholding { get; set; }
    public decimal ServiceFees { get; set; }
    public string? Note { get; set; }
    public string TransactionId { get; set; }

    public PaymentMethodDto PaymentMethod { get; set; }
}
