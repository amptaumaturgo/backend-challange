using Backend.Domain.Entities.Enums;
using Backend.Domain.Entities.ValueObject;
using Backend.Shared.Domain.Base;

namespace Backend.Domain.Entities;

public class Driver : Entity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public Document Cnpj { get; private set; } = new();
    public Document DriverLicense { get; private set; } = new();
    public DateTime BirthDate { get; private set; }
    public DriverLicenseType DriverLicenseType { get; private set; }
    public Guid UserId { get; private set; }

    public Driver(string name, string cnpj, string driverLicense, DateTime birthDate, DriverLicenseType driverLicenseType, Guid userId)
    {
        Name = name;
        Cnpj = new Document(cnpj, "Cnpj");
        DriverLicense = new Document(driverLicense, "Driver License");
        BirthDate = birthDate;
        DriverLicenseType = driverLicenseType;
        UserId = userId;
    }

    protected Driver() { }

}