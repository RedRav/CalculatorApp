public class CalculatorRequest
{
    public double A { get; set; }
    public double B { get; set; }
    public string Operation { get; set; } = null!; // "+", "-", "*", "/", "pow", "root"
}