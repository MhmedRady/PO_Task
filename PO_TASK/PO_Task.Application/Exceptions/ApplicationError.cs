namespace PO_Task.Application.Exceptions;

public sealed record ApplicationError(
    string Key,
    string ErrorMessage);
