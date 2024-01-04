namespace Auth.Min.API.Dtos;

public class EmailRequestDto
{
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public required string To { get; set; }
}

