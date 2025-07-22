using AuthServer.CoreLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.DataAccessLayer
{
   
    public class AppDbContext : IdentityDbContext<UserApp,IdentityRole, string>
    {
      
     

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

    
        public DbSet<Product> Products { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }


        /*
         Separation Of Concerns prensibiyle dbcontext oluşturma
       
         * Entity'lerin oluştururken her property ve entity için belirli başla kurallar ve ilişkiler vardır. Bu ilişkiler
          her entity için ayrı ayrı configuration class'ı oluşturularak yazılır ve aşağıdaki koddaki gibi dbContext'e eklenir.
          Assembly configuration dosyalarını entity'lerden bulur. Yani context'e dbset olarak verdiğimiz tüm entity'lerin
          için yazılmış configuration class'larını bulur ve onları çalıştırır.
        */
        protected override void OnModelCreating(ModelBuilder builder)
        {
           
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
            base.OnModelCreating(builder);
        }

    }
}
