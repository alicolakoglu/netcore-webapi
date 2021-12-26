using ExampleProject.Core.Domain.Todo;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Data.Mapping.Todo
{
    public class TodoItemMap : IEntityMap
    {
        public void Map(ModelBuilder entityBuilder)
        {
            var builder = entityBuilder.Entity<TodoItem>();
            builder.ToTable("Todo_Item");
            builder.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
            builder.Property(x => x.Key).HasDefaultValueSql("newsequentialid()");

            builder.HasOne(x => x.CreatorUser).WithMany().HasForeignKey(x => x.CreatorUserKey).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
