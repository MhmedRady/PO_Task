using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;
using PO_Task.Domain.Users;

namespace PO_Task.Infrastructure.Configurations
{
    internal sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            ConfigureOrder(builder);
        }

        public static void ConfigureOrder(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.ToTable("PurchaseOrders", "PO");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
              .ValueGeneratedNever()
              .HasConversion(
                  id => id.Value,
                  value => PurchaseOrderId.Create(value));

            builder.HasIndex(o => o.PurchaserId);
            builder.Property(i => i.PurchaserId)
                    .ValueGeneratedNever() 
                    .HasConversion(
                        id => id.Value,
                        value => UserId.Create(value));

            // Unique index for PoNumber
            builder.HasIndex(po => po.PoNumber).IsUnique();

            // OwnsOne for Status (Value Object)
            builder.OwnsOne(o => o.Status, s =>
            {
                s.Property(p => p.Value)
                 .HasColumnName("status_value")
                 .IsRequired();

                s.Property(p => p.Name)
                 .HasColumnName("status_name")
                 .IsRequired();
            });

            // OwnsOne for TotalAmount (Money Value Object)
            builder.OwnsOne(p => p.TotalAmount, moneyBuilder =>
            {
                moneyBuilder.Property(m => m.Amount)
                            .HasColumnType("decimal(18,2)")
                            .IsRequired();

                moneyBuilder.Property(m => m.Currency)
                            .HasConversion(
                                currency => currency.Code,          // Convert Currency to string
                                code => Currency.FromCode(code))   // Convert string to Currency
                            .HasMaxLength(3)
                            .IsRequired();
            });

            // OwnsMany for PurchaseOrderItems
            builder.OwnsMany(o => o.PurchaseOrderItems, ol =>
            {
                ol.ToTable("OrderItems", "PO");
                ol.HasKey(i => i.Id);

                // Value object mapping for Id
                ol.Property(i => i.Id)
                  .ValueGeneratedNever()
                  .HasConversion(
                      id => id.Value,
                      value => ItemId.Create(value)
                    );



                // Index on PurchaseOrderId
                ol.HasIndex(l => l.PurchaseOrderId);

                ol.Property(i => i.PurchaseOrderId)
                    .ValueGeneratedNever()
                    .HasConversion(
                        purchaseOrderId => purchaseOrderId.Value,
                        value => PurchaseOrderId.Create(value)
                    );

                // Relationship to PurchaseOrder
                ol.HasOne<PurchaseOrder>()
                  .WithMany(po => po.PurchaseOrderItems)
                  .HasForeignKey(i => i.PurchaseOrderId);

                // Optional: Soft delete filter
                // ol.HasQueryFilter(q => !q.DeletedAt.HasValue);

                // Properties
                ol.Property(l => l.GoodCode)
                  .IsRequired()
                  .HasMaxLength(50);

                ol.Property(l => l.Quantity)
                  .HasColumnType("decimal(10,2)")
                  .IsRequired();

                ol.Property(l => l.SerialNumber)
                  .IsRequired();

                ol.Property(l => l.DeletedAt);

                // Value object mapping for Price (Money)
                ol.OwnsOne(
                    s => s.Price,
                    price =>
                    {
                        price.Property(m => m.Amount)
                             .HasColumnType("decimal(18,2)")
                             .IsRequired();

                        price.Property(m => m.Currency)
                             .HasConversion(
                                 currency => currency.Code,
                                 code => Currency.FromCode(code))
                             .HasMaxLength(3)
                             .IsRequired();
                    });
            });
        }
    }
}
