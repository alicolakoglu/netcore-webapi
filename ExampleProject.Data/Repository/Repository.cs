using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace ExampleProject.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly Context.ProjectDBContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(Context.ProjectDBContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet as IQueryable<TEntity>;
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Update(TEntity entity, Expression<Func<TEntity, object>>[] properties)
        {
            EntityEntry<TEntity> entry = _context.Entry(entity);
            _context.Set<TEntity>().Attach(entity);
            entry.State = EntityState.Unchanged;

            foreach (var property in properties)
            {
                string propertyName = "";
                Expression bodyExpression = property.Body;
                if (bodyExpression.NodeType == ExpressionType.Convert && bodyExpression is UnaryExpression)
                {
                    Expression operand = ((UnaryExpression)property.Body).Operand;
                    propertyName = ((MemberExpression)operand).Member.Name;
                }
                else
                {
                    propertyName = ExpressionHelper.GetExpressionText(property);
                }
                entry.Property(propertyName).IsModified = true;
            }
        }

        public virtual void Delete(int id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public IQueryable<TEntity> SqlResult(string sqlCmd)
        {
            return _dbSet.FromSqlRaw(sqlCmd).AsQueryable();
        }

        public DataTable GetData(string sqlCmd)
        {
            var table = new DataTable();
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sqlCmd;
            cmd.Connection.Open();
            table.Load(cmd.ExecuteReader());
            cmd.Connection.Close();
            return table;
        }

        public DataSet GetDataSet(string sqlCmd)
        {
            try
            {
                DataSet dSet = new DataSet();
                SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection();
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(dSet);
                conn.Close();
                return dSet;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual void ExecuteCommand(string sqlCmd)
        {
            _context.Database.ExecuteSqlRaw(sqlCmd);
        }
    }
}
