using CalculatorApp.Domain.Entities;

public interface ICalculatorService
{
    Task<CalculatorResponse> CalculateAsync(CalculatorRequest request, string userId);
    Task LogAsync(List<CalculationLog> logs);
}