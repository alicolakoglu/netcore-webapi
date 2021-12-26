using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Data.Mapping.Users
{
    public class UserMap : IEntityMap
    {
        public void Map(ModelBuilder entityBuilder)
        {
            var builder = entityBuilder.Entity<Core.Domain.Users.User>();
            builder.ToTable("Users_User");
            builder.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
            builder.Property(x => x.IsAdministrator).HasDefaultValueSql("0");
            builder.Property(x => x.IsActive).HasDefaultValueSql("1");
            builder.Property(x => x.IsDeleted).HasDefaultValueSql("0");
            builder.Property(x => x.Key).HasDefaultValueSql("newsequentialid()");
        }
    }
}
