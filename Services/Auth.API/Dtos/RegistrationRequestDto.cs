using FluentValidation;

namespace Auth.API.Dtos
{
    public class RegistrationRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
    }

    public class RegistrationRequestDtoValidator : AbstractValidator<RegistrationRequestDto>
    {
        public RegistrationRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .Must(value => value != "string").WithMessage("Invalid Email value.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password is required.")
                .Must(value => value != "string").WithMessage("Invalid Password value.")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.PhoneNumber)
                .NotNull().WithMessage("PhoneNumber is required.")
                .Must(value => value != "string").WithMessage("Invalid PhoneNumber value.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.FirstName)
                .NotNull().WithMessage("FirstName is required.")
                .Must(value => value != "string").WithMessage("Invalid FirstName value.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.MiddleName)
                .Must(value => value != "string").WithMessage("Invalid MiddleName value.")
                .When(x => !string.IsNullOrEmpty(x.MiddleName));

            RuleFor(x => x.LastName)
                .NotNull().WithMessage("LastName is required.")
                .Must(value => value != "string").WithMessage("Invalid LastName value.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.UserName)
                .Must(value => value != "string").WithMessage("Invalid UserName value.")
                .When(x => !string.IsNullOrEmpty(x.UserName));
        }
    }
}