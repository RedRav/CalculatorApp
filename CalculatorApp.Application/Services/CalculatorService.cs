using CalculatorApp.Domain.Entities;
using CalculatorApp.Persistence;

public class CalculatorService : ICalculatorService
{
    private readonly ApplicationDbContext _db;
    public CalculatorService(ApplicationDbContext db) => _db = db;

    public async Task<CalculatorResponse> CalculateAsync(CalculatorRequest request, string userId)
    {
        double result;
        try
        {
            result = request.Operation switch
            {
                "+" => request.A + request.B,
                "-" => request.A - request.B,
                "*" => request.A * request.B,
                "/" => request.B == 0 ? throw new DivideByZeroException("Нельзя делить на 0") : request.A / request.B,
                "pow" => Math.Pow(request.A, request.B),
                "root" => request.B == 0 ? throw new ArgumentException("Основание не может быть 0") : Math.Pow(request.A, 1.0 / request.B),
                _ => throw new ArgumentException("Недопустимая операция")
            };
        }
        catch (Exception ex)
        {
            await LogAsync(request, 0, userId, ex.Message);
            return new CalculatorResponse
            {
                Result = 0,
                Message = ex.Message
            };
        }

        await LogAsync(request, result, userId);
        return new CalculatorResponse
        {
            Result = result,
            Message = "OK"
        };
    }

    private async Task LogAsync(CalculatorRequest req, double result, string userId, string? error = null)
    {
        var log = new CalculationLog
        {
            Operand1 = req.A,
            Operand2 = req.B,
            Operation = req.Operation,
            Result = result,
            UserId = userId,
            Timestamp = DateTime.UtcNow,
            Error = error
        };
        _db.CalculationLogs.Add(log);
        await _db.SaveChangesAsync();
    }
}