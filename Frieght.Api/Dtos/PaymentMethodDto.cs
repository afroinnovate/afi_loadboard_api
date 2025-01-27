using System.Text.Json.Serialization;

namespace Frieght.Api.Dtos;

public class PaymentMethodDto
{
    [JsonPropertyName("paymentType")]
    public required string PaymentType { get; set; }

    [JsonPropertyName("carrierId")]
    public required string CarrierId { get; set; }

    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }

    [JsonPropertyName("bankAccount")]
    public string? BankAccount { get; set; }

    [JsonPropertyName("accountHolderName")]
    public string? AccountHolderName { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("cardMethod")]
    public string? CardMethod { get; set; }

    [JsonPropertyName("cardType")]
    public string? CardType { get; set; }

    [JsonPropertyName("lastFourDigits")]
    public string? LastFourDigits { get; set; }

    [JsonPropertyName("billingAddress")]
    public string? BillingAddress { get; set; }
}
