using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VueNetCoreBoilerplate.Repository {
    public interface IRepo : IDisposable {
        Task AddAsync<T>(T entityToCreate) where T : class;
        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        IQueryable<T> Read<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;
        IQueryable<T> Track<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;
        Task<T> FindAsync<T>(object id, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
        void Remove<T>(T entityToDelete) where T : class;        
        ITransactionScope Transaction();
        IRepo Scope();
    }

    public class Repo : IRepo, IDisposable {
        protected readonly Context Context;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Repo(Context context) {
            Context = context;
        }        

        public Task AddAsync<T>(T entityToCreate) where T : class {
            return Task.FromResult(Context.Set<T>().AddAsync(entityToCreate));
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.FromResult(Context.SaveChangesAsync(cancellationToken));
        }

        public IQueryable<T> Read<T>(params Expression<Func<T, object>>[] includeProperties) where T : class {            
            var query = Context.Set<T>().AsNoTracking().AsQueryable();
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<T> Track<T>(params Expression<Func<T, object>>[] includeProperties) where T : class {
            var query = Context.Set<T>().AsQueryable();
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Task<T> FindAsync<T>(object id, CancellationToken cancellationToken = default(CancellationToken))
            where T : class {
            return Task.FromResult(Context.FindAsync<T>(new[] {id}, cancellationToken).Result);
        }

        public void Remove<T>(T entityToDelete) where T : class {
            Context.Remove(entityToDelete);
        }

        public ITransactionScope Transaction() {
            return new TransactionScope(Context);
        }

        public IRepo Scope() {
            return new Repo(new Context(Context.Options));
        }

        public void Dispose() {
            Context.Dispose();
        }
    }
}