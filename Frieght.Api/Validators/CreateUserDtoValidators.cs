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
      RuleFor(x => x.UserType).NotEmpty().Must(x => x.Equals("Carrier", StringComparison.OrdinalIgnoreCase) || x.Equals("Shipper", StringComparison.OrdinalIgnoreCase))
          .WithMessage("UserType must be either 'Carrier' or 'Shipper'.");

      When(x => x.UserType.Equals("Carrier", StringComparison.OrdinalIgnoreCase), () =>
      {
        RuleFor(x => x.MotorCarrierNumber).NotEmpty().WithMessage("Motor Carrier Number is required for Carriers");
        RuleFor(x => x.DOTNumber).NotEmpty().WithMessage("DOT Number is required for Carriers");
        RuleFor(x => x.EquipmentType).NotEmpty().WithMessage("Equipment Type is required for Carriers");
        RuleFor(x => x.CarrierRole).NotNull().WithMessage("Carrier Role is required for Carriers");
      });

      When(x => x.UserType.Equals("Shipper", StringComparison.OrdinalIgnoreCase), () =>
      {
        RuleFor(x => x.ShipperRole).NotNull().WithMessage("Shipper Role is required for Shippers");
        // No need to validate VehicleTypes for Shippers
      });
    }
  }
}
