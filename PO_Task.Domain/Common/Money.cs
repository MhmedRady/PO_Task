using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.Common;

public sealed record Money
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }

    private Money() { } // For EF Core

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.");

        Amount = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }

    public static Money operator +(
        Money first,
        Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new InvalidOperationException("Currencies have to be equal");
        }

        return new Money(
            first.Amount + second.Amount,
            first.Currency);
    }

    public static Money Zero()
    {
        return new Money(
            0,
            Currency.None);
    }

    public static Money Zero(Currency currency)
    {
        return new Money(
            0,
            currency);
    }

    public bool IsZero()
    {
        return this == Zero(Currency);
    }

    public Money Add(Money other)
{
    if (Currency != other.Currency)
        throw new InvalidOperationException("Cannot add Money with different currencies.");

    return new Money(Amount + other.Amount, Currency);
}
}
