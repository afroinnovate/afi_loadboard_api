namespace Auth.Min.API.Dtos;

public class UserDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IEnumerable<string>? Roles { get; set; }
    public string CompanyName { get; set; }
    public string? DOTNumber { get; set; }
}
