namespace PO_Task.Domain.Common;

public sealed record Currency
{
    internal static readonly Currency None = new("_");
    public static readonly Currency Egp = new("EGP");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");

    public static readonly IReadOnlyCollection<Currency> All = new[] { Egp, Usd, Eur };

    private Currency() { }
    private Currency(string code)
    {
        Code = !string.IsNullOrWhiteSpace(code) 
            ? code
            : throw new ArgumentException("Currency code cannot be null or empty.");
    }

    public string Code { get; init; }

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c => code.Equals(c.Code, StringComparison.OrdinalIgnoreCase)) ??
               throw new ApplicationException("The currency code is invalid");
    }
}
