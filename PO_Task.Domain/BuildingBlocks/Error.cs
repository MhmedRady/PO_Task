namespace PO_Task.Domain.BuildingBlocks;

public record Error(
    string Identifier,
    string Message)
{
    public static readonly Error None = new(
        string.Empty,
        string.Empty);

    public static readonly Error NullValue = new(
        "Error.NullValue",
        "Null value was provided");
}
