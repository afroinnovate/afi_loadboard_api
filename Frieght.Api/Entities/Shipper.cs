

using System.ComponentModel.DataAnnotations;

namespace Frieght.Api.Entities;

public class Shipper
{ 
    [Key]
    public string UserId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CompanyName { get; set; }
    public string? DOTNumber { get; set; }
    public IEnumerable<Load> Loads { get; set; }
};