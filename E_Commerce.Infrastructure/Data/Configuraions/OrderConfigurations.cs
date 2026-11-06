using E_Commerce.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configuraions
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShipToAdress, adress =>
            {
                adress.Property(a => a.FirstName).HasColumnName("FirstName").HasMaxLength(50);
                adress.Property(a => a.LastName).HasColumnName("LastName").HasMaxLength(50);
                adress.Property(a => a.Street).HasColumnName("Street").HasMaxLength(50);
                adress.Property(a => a.City).HasColumnName("City").HasMaxLength(50);
                adress.Property(a => a.Country).HasColumnName("Country").HasMaxLength(50);
            });

            builder.HasMany(O => O.OrderItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(O => O.SubTotal).HasColumnType("decimal(8,2)");

            builder.Property(O => O.Status).HasConversion<string>().HasMaxLength(50);
        }
    }
}