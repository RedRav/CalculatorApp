namespace CalculatorApp.Domain.Entities;

public class CalculationLog
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public string Operation { get; set; } = null!;
    public double Operand1 { get; set; }
    public double Operand2 { get; set; }
    public double? Result { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Error { get; set; }
}