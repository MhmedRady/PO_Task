
using PO_Task.Application.Abstractions;

namespace PO_Task.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
