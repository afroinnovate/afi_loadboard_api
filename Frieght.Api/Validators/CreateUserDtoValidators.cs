using FluentValidation;
using Frieght.Api.Dtos;

namespace Frieght.Api.Validators
{
  public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
  {
    public CreateUserDtoValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Email).NotEmpty().EmailAddress();
      RuleFor(x => x.FirstName).NotEmpty();
      RuleFor(x => x.LastName).NotEmpty();
      RuleFor(x => x.UserType).NotEmpty().Must(x => x != null && (x.Equals("carrier", StringComparison.OrdinalIgnoreCase) || x.Equals("Shipper", StringComparison.OrdinalIgnoreCase)))
          .WithMessage("UserType must be either 'Carrier' or 'Shipper'.");

      When(x => x.UserType != null && x.UserType.Equals("Carrier", StringComparison.OrdinalIgnoreCase), () =>
      {
        RuleFor(x => x.BusinessProfile).NotNull().WithMessage("Business Profile is required for Carriers");
        RuleFor(x => x.BusinessProfile.CarrierRole).NotNull().WithMessage("Carrier Role is required for Carriers");
      });

      When(x => x.UserType != null && x.UserType.Equals("Shipper", StringComparison.OrdinalIgnoreCase), () =>
      {
        RuleFor(x => x.BusinessProfile).NotNull().WithMessage("Business Profile is required for Shippers");
        RuleFor(x => x.BusinessProfile.ShipperRole).NotNull().WithMessage("Shipper Role is required for Shippers");
      });
    }
  }
}
