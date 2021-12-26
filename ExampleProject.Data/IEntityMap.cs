using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Data
{
    public interface IEntityMap
    {
        void Map(ModelBuilder entityBuilder);
    }
}
