namespace Frieght.Api.Entities;

public class PaymentMethod
{
    public int Id { get; set; }
    public string PaymentType { get; set; } // Debit, Bank, Mobile Payment
    public string? BankName { get; set; }
    public string? BankAccount { get; set; }
    public string? AccountHolderName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CardMethod { get; set; } // Visa, MasterCard
    public string? PaymentMethodId { get; set; }
    public string? CardType { get; set; }
    public string? LastFourDigits { get; set; }
    public string? BillingAddress { get; set; }

    // Foreign Key
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
}
