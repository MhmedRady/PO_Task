using System.Data;

namespace PO_Task.Application.Abstractions;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
