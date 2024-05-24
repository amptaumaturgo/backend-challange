namespace Backend.Application.Motorcycle.Queries.GetByPlate;

public class GetByPlateQueryResponse  
{
    public Guid Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
}