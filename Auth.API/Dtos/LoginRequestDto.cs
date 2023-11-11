using FluentValidation;

namespace Auth.API.Dtos
{
    public class LoginRequestDto
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email)
               .Must(value => value != "string").WithMessage("Invalid Email value.")
               .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password cannot be null.")
                .Must(value => value != "string").WithMessage("Invalid Password value.")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.UserName)
                .Must(value => value != "string").WithMessage("Invalid UserName value.")
                .When(x => !string.IsNullOrEmpty(x.UserName));
        }
    }

}