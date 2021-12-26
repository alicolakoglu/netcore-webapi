using ExampleProject.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Data.Mapping.Common
{
    public class AppMenuMap : IEntityMap
    {
        public void Map(ModelBuilder entityBuilder)
        {
            var builder = entityBuilder.Entity<AppMenu>();
            builder.ToTable("Common_AppMenu");
        }
    }
}
