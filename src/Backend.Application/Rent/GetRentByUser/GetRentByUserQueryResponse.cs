namespace Backend.Application.Rent.GetRentByUser;

public class GetRentByUserQueryResponse
{
    public Guid RentId { get; set; }
    public int PlanDays { get; set; }   
    public decimal PricePerDay { get; set; }
    public decimal Total { get; set; }
    public DateTime StartDate { get; set; } 
    public DateTime ExpectedEndDate { get; set; }
}