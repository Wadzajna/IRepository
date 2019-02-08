using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Transactions;

namespace BoschAspApps.Utils.IDAL
{

    public abstract class JednotkaPrace<T> : IJednotkaPrace where T : DbContext, new()
    {

        private readonly T _db;

        public JednotkaPrace(bool debug = true)
        {

            _db = new T();

            if (debug)
            {
                 _db.Database.Log = s => Debug.WriteLine(s);
            }

            return;
        }

        public DbConnection Connection()
        {
            return this.Db.Database.Connection;
        }


        public DbContext Db
        {
            get { return _db; }
        }

        DbContext IJednotkaPrace.Db
        {
            get
            {
                return Db;
            }
        }

        public DbContextTransaction Transakce_New()
        {
            return this.Db.Database.BeginTransaction();
        }


        public DbContextTransaction Transakce_Current()
        {
            return this.Db.Database.CurrentTransaction;
        }


        public DbContextTransaction Transakce_CurrentNew()
        {
            var t = Transakce_Current();
            if (t == null) t = Transakce_New();
            return t;
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Db.Dispose();
                }
                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }

}
