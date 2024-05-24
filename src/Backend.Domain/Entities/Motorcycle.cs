using Backend.Shared.Domain.Base;
using Backend.Shared.Domain.Exceptions;

namespace Backend.Domain.Entities;

public class Motorcycle : Entity, IAggregateRoot
{
    public int Year { get; private set; }
    public string Model { get; private set; } = string.Empty;
    public string Plate { get; private set; } = string.Empty;

    public Motorcycle(int year, string model, string plate)
    {
        SetYear(year);
        SetPlate(plate);

        Model = model;
    }

    protected Motorcycle() { }

    public void SetYear(int year)
    {
        if (year < 2010)
        {
            throw new DomainException("The motorcycle's year must be equal or greater then 2010.", Id, nameof(Motorcycle));
        }

        Year = year;
    }

    public void SetPlate(string plate)
    {
        if (string.IsNullOrEmpty(plate) || plate.Length != 7)
        {
            throw new DomainException("Plate must be exactly 7 characters long.", Id, nameof(Motorcycle));
        }

        Plate = plate;
    }
}