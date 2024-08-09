using FluentValidation;
using Frieght.Api.Dtos;

namespace Frieght.Api.Validators
{
  public class VehicleTypeDtoValidator : AbstractValidator<VehicleTypeDto>
  {
    public VehicleTypeDtoValidator()
    {
      RuleFor(x => x.Name).NotEmpty().WithMessage("Vehicle Name is required");
    }
  }
}
