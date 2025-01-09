using PO_Task.Domain;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Items;

namespace PO_Task.Domain.PurchaseOrders;

public sealed class NewPoNumberGenerator : IPoNumberGenerator
{
    public string GeneratePoNumber(DateTime createdDate)
    {
        var timeStamps = Int64.Parse(createdDate.ToString("yyyyMMddHHmmss")) + new Random().Next(1000);
        return $"PO-{timeStamps}";
    }
}

public sealed class OldPoNumberGenerator : IPoNumberGenerator
{
    public string GeneratePoNumber(DateTime createdDate)
    {
        return $"PO{createdDate:yyyyMMddHHmmssfff}";
    }
}

public enum PoNumberGeneratorType
{
    NewPoNumberGenerator,
    OldPoNumberGenerator
}