public interface ICalculatorService
{
    Task<CalculatorResponse> CalculateAsync(CalculatorRequest request, string userId);
}