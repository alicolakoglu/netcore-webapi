using System;
using ExampleProject.Data.Context;
using Microsoft.EntityFrameworkCore;
using ExampleProject.Data.Repository;

namespace ExampleProject.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectDBContext _context;

        public UnitOfWork(ProjectDBContext context)
        {
            //Database.SetInitializer<BudgetContext>(null);

            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_context);
        }

        public int SaveChanges()
        {
            try
            {
                if (_context == null)
                    throw new ArgumentNullException("_context");

                return _context.SaveChanges();
            }
            catch (DbUpdateException dbUpdateEx)
            {
                throw new Exception(dbUpdateEx.InnerException.Message/*, dbUpdateEx*/);
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
