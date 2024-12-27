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
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
          builder.HasKey(x => x.UserId);
          builder.Property(x => x.RefreshTokenCode).IsRequired();
          builder.Property(x => x.Expiration).IsRequired();
        }
    }
}
