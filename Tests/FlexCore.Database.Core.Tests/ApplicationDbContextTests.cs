namespace FlexCore.Database.Core.Tests
{
    using Microsoft.EntityFrameworkCore;
    using System;

    public class ApplicationDbContextTest : DbContext
    {
        public ApplicationDbContextTest(DbContextOptions<ApplicationDbContextTest> options) : base(options) { }

        public void BeginTransaction() => Database.BeginTransaction();

        public void CommitTransaction()
        {
            if (Database.CurrentTransaction != null)
            {
                Database.CurrentTransaction.Commit();
                Database.CurrentTransaction.Dispose();
            }
        }

        public void RollbackTransaction()
        {
            if (Database.CurrentTransaction != null)
            {
                Database.CurrentTransaction.Rollback();
                Database.CurrentTransaction.Dispose();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}