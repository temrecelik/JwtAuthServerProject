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
    /// <summary>
    ///  Role,User gibi tabloları için appDbContext oluştururken bu class'ın  IdentityContextDb classında miras alması gerekir. 
    ///  Ancak product,stock gibi üyelik işlemler için oluşturulacak tablolar için context oluşturmak istersek direkt olarak
    ///  Context class'ını miras almalıdır. Bu nedenle iki farklı context yani iki farklı veri tabanı gerekli olacaktır.
    ///  Ancak tek veritabanı kullanmak için IdentityDbContext user işlemleri dışındaki diğer entity'ler için de k
    ///  kullanılabiliriz.
    /// BirinciParamatre kullanıcılar için user entity'si ikinci parametre kullanıcı rolleri için oluşturulacak entity direkt
    /// olarak IdentityRole classını verebiliriz yada role için bir entity oluşturup IdentitRole class'ını miras almasını 
    /// sağlayabiliriz.Üçüncü parametrede ise oluşturan tablo için primary key'in veri tipini belirtiriz.
    /// </summary>
    public class AppDbContext : IdentityDbContext<UserApp,IdentityRole, string>
    {
        //program.cs'de yazılan yapılandırmaya göre migration'ı basar.
       
     

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //İdentity dışındaki dbSet'leri kendimiz oluşturduk.
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
