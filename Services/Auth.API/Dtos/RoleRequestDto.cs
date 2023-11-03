using System.Threading;
using FluentValidation;

namespace Auth.API.Dtos 
{
    public class RoleRequestDto
    {
        public string? Email { get; set; }
        public string? RoleName { get; set; }
    }

    public class RoleRequestValidator: AbstractValidator<RoleRequestDto>
    {
        List<string> roles = new List<string>()
        {
            "CUS_ADMIN", "ADMIN", "CARRIER", "SHIPPER"
        };

        public RoleRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .Must(value => value != "string").WithMessage("Invalid Email value.")
                .When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.RoleName)
                .NotNull().WithMessage("Role name cannot be null, can be CUS_ADMIN, ADMIN, CARRIER, SHIPPER")
                .Must(value => value != "string").WithMessage("Invalid value")
                .Must(value => roles.Contains(value.ToUpper())).WithMessage("Role name can only be CUS_ADMIN, ADMIN, CARRIER, SHIPPER")
                .When(x => !String.IsNullOrEmpty(x.RoleName));
        }
    }
}