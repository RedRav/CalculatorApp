namespace CalculatorApp.Domain.Entities;

public class CalculationLog
{
    public int Id { get; set; }
    public string Operation { get; set; } = null!;
    public string Expression { get; set; } = null!;
    public string? Result { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? UserId { get; set; }
}