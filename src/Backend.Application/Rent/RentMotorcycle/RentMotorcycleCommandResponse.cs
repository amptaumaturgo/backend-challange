namespace Backend.Application.Rent.RentMotorcycle;

public class RentMotorcycleCommandResponse(decimal total, decimal pricePerDay, int totalDays)
{
    public decimal Total { get; set; } = total;
    public decimal PricePerDay { get; set; } = pricePerDay;
    public int TotalDays { get; set; } = totalDays;
}