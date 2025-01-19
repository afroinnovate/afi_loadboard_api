namespace Frieght.Api.Dtos;

public class PaymentMethodDto
{
    public string Method { get; set; }
    public string Type { get; set; }
    public string? BankName { get; set; }
    public string? BankAccount { get; set; }
    public string? AccountHolderName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CardMethod { get; set; }
    public string? CardType { get; set; }
    public string? LastFourDigits { get; set; }
    public string? BillingAddress { get; set; }
}
