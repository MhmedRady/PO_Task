//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using PO_Task.Domain.Common;
//using PO_Task.Domain.Items;
//using PO_Task.Domain.PurchaseOrders;

//namespace PO_Task.Infrastructure.Configurations;

//internal sealed class ItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
//{
//    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
//    {
//        ConfigureItem(builder);
//    }

//    private static void ConfigureItem(EntityTypeBuilder<PurchaseOrderItem> builder)
//    {
//        builder.ToTable("PurchaseOrderItems");

//        builder.HasKey(i => i.Id);

//        builder.Property(i => i.Id)
//            .ValueGeneratedNever()
//            .HasConversion(
//                id => id.Value,
//                value => ItemId.Create(value));

//        // Remove Unique Index on PurchaseOrderId to allow multiple items per PO
//        builder.HasIndex(l => l.PurchaseOrderId);

//        // Soft delete query filter
//        builder.HasQueryFilter(q => !q.DeletedAt.HasValue);

//        // Relationship to PurchaseOrder
//        builder.HasOne<PurchaseOrder>()
//            .WithMany(po => po.PurchaseOrderItems)
//            .HasForeignKey(i => i.PurchaseOrderId);

//        // Map properties
//        builder.Property(l => l.GoodCode).IsRequired().HasMaxLength(50);
//        builder.Property(l => l.Quantity).HasColumnType("decimal(10,2)");
//        builder.Property(l => l.SerialNumber);
//        builder.Property(l => l.DeletedAt);

//        // Value object mapping for Price
//        builder.OwnsOne(
//            s => s.Price,
//            price =>
//            {
//                price.Property(m => m.Currency)
//                     .HasConversion(
//                         currency => currency.Code,
//                         code => Currency.FromCode(code))
//                     .HasMaxLength(3)
//                     .IsRequired();

//                price.Property(m => m.Amount)
//                     .HasColumnType("decimal(18,2)")
//                     .IsRequired();
//            });
//    }

//}
