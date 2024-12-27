using AuthServer.CoreLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.DataAccessLayer.Configurations
{
    /*
     DataAcces katmanında yazdığımız Configuration Class'larında db'ye eklenecek entity'lerin property'lerinin 
     kuralları yazılır. İşte product tablosu için ProductId primary keydir, productName null olamaz gibi kurallar yazılır. 
     Her entity için tüm congifuration'lar eğer context'te yazılırsa context şişer. Bu nedenle her entity için ayrı ayrı 
     configuration class'ları oluşturulur.Ve bu class'lar context'e eklenir. Örneğin bir tablo için entity oluşturduğumuzda 
     ya entity'nin içinde primary key için [Key] attribute'nu kullanırız ya Id yada ProductID gibi bir property'i
     ismi tanımlayarak EntityFramework'e primary key olduğunu belirtiriz. ya da bu işlemi configuration class'ında 
     aşağıdaki gibi yaparız.
     */
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
           builder.HasKey(x => x.Id);
           builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
           builder.Property(x => x.stock).IsRequired();
           builder.Property(x => x.Price).HasColumnType("decimal(18,2)"); //1000000000000000,99
           builder.Property(x => x.UserId).IsRequired();
        }
    }
}
