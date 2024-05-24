using Backend.Shared.Domain.Base;

namespace Backend.Domain.Entities;

public class Plan : Entity
{
    public int Days { get; private set; }
    public decimal PricePerDay { get; private set; }

    protected Plan() { }

    public Plan(int days, decimal pricePerDay)
    {
        Days = days;
        PricePerDay = pricePerDay;
    }
}