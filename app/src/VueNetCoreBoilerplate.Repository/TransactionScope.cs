using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace VueNetCoreBoilerplate.Repository {
    public interface ITransactionScope : IRepo {
        void Rollback();
        void Commit();
        Task SaveAndCommit(CancellationToken cancellationToken = default(CancellationToken));
    }

    public class TransactionScope : Repo, ITransactionScope {
        private readonly IDbContextTransaction _trx;

        public TransactionScope(Context context) : base(new Context(context.Options)) {
            _trx = Context.Database.BeginTransaction();
        }

        public void Rollback() {
            _trx.Rollback();
        }

        public void Commit() {
            _trx.Commit();
        }

        public Task SaveAndCommit(CancellationToken cancellationToken = default(CancellationToken)) {
            Context.SaveChangesAsync(cancellationToken).Wait(cancellationToken);
            Commit();
            return Task.CompletedTask;
        }

        public new void Dispose() {
            _trx.Dispose();
            base.Dispose();
        }
    }
}