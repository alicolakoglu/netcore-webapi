using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ExampleProject.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        void Update(TEntity entity, Expression<Func<TEntity, object>>[] properties);
        void Delete(int id);
        void Delete(TEntity entityToDelete);

        IQueryable<TEntity> GetQuery(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> SqlResult(string sqlCmd);

        DataTable GetData(string sqlCmd);
        DataSet GetDataSet(string sqlCmd);
        void ExecuteCommand(string sqlCmd);
    }
}
