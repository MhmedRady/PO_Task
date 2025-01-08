using Microsoft.EntityFrameworkCore;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO_Task.Infrastructure
{
    public static class SeedingPO
    {
        public static async Task Populate1MillionPOsAsync(ApplicationDbContext db)
        {
            const int total = 10;
            const int batchSize = 2;
            int inserted = 0;
            try
            {
                /*if (await db.PurchaseOrders.AnyAsync())
                    return;*/

                while (inserted < total)
                {
                    var batch = new List<PurchaseOrder>();
                    for (int i = 0; i < batchSize && (inserted + i) < total; i++)
                    {
                        var poId = PurchaseOrderId.CreateUnique();
                        var orderItems = new List<PurchaseOrderItem>();
                        for (int j = 1; j < 5; j++)
                        {
                            var rPrice = new Money(new Random().Next(1000), Currency.FromCode("EGP"));

                            var poItem = PurchaseOrderItem.CreateInstance(poId, $"GC-{rPrice.Amount * j}", 100, rPrice);
                            orderItems.Add(poItem);
                        }

                        var createdDate = DateTime.Now;

                        var poNumber = (i % 2 == 0) ?
                            new NewPoNumberGenerator().GeneratePoNumber(createdDate)
                            : new OldPoNumberGenerator().GeneratePoNumber(createdDate);

                        var po = PurchaseOrder.CreateOrderInstance(
                            purchaseOrderId: poId,
                            createdDate,
                            poNumber,
                            orderItems
                            );
                        batch.Add(po);
                    }

                    db.PurchaseOrders.AddRange(batch);
                    await db.SaveChangesAsync();

                    inserted += batch.Count;
                    Console.WriteLine($"Inserted {inserted}/{total}...");
                }

                Console.WriteLine("Done seeding 1M records.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Populate1MillionPOsAsync Inserted {inserted}/{total} and get Error: \n { ex.Message }");
            }
        }
    }
}
