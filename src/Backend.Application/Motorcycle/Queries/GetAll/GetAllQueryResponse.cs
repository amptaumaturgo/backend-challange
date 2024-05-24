namespace Backend.Application.Motorcycle.Queries.GetAll;

public class GetAllQueryResponse
{
    public Guid Id { get; set; }
    public string Model { get; set; }  = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public int Year { get; set; }
}