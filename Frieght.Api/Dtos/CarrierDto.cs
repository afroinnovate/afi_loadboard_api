namespace Frieght.Api.Dtos;

public record CarrierDto
(
        int Id,
        string UserId,
        string CompanyName,
        string CompanyEmail,
        string CompanyPhone,
        string MotorCarrierNumber,
        string USDOTNumber,
        string EquipmentType ,
        double AvailableCapacity
   
);

public record CreateCarrierDto
(

        string UserId,
        string CompanyName,
        string CompanyEmail,
        string CompanyPhone,
        string MotorCarrierNumber,
        string USDOTNumber,
        string EquipmentType,
        double AvailableCapacity
   
);

public record UpdateCarrierDto
(
        int Id,
        string UserId,
        string CompanyName,
        string CompanyEmail,
        string CompanyPhone,
        string MotorCarrierNumber,
        string USDOTNumber,
        string EquipmentType,
        double AvailableCapacity
    
);
