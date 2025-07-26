using CalculatorApp.Domain.Entities;
using CalculatorApp.Persistence;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class CalculatorService : ICalculatorService
{
    private readonly ApplicationDbContext _db;
    public CalculatorService(ApplicationDbContext db) => _db = db;

    public async Task<CalculatorResponse> CalculateAsync(CalculatorRequest request, string userId)
    {
        double result;
        var log = new CalculationLog
        {
            Operand1 = request.A,
            Operand2 = request.B,
            Operation = request.Operation,
            UserId = userId,
            Timestamp = DateTime.UtcNow,
        };
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
            log.Result = result;
        }
        catch (Exception ex)
        {
            log.Error = ex.Message;
            log.Result = 0;
            await LogAsync(log);
            return new CalculatorResponse
            {
                Result = 0,
                Message = ex.Message
            };
        }

        await LogAsync(log);
        return new CalculatorResponse
        {
            Result = result,
            Message = "OK"
        };
    }

    private async Task LogAsync(CalculationLog log)
    {
        _db.CalculationLogs.Add(log);
        await _db.SaveChangesAsync();
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

    public async Task LogAsync(List<CalculationLog> logs)
    {
        _db.CalculationLogs.AddRange(logs);
        await _db.SaveChangesAsync();
    }
}