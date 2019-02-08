using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace BoschAspApps.Utils.IDAL
{

    /// <summary>
    /// Obsahuje zakladni funkcionalitu nad danou tabulkou v databazi.
    /// </summary>
    /// <typeparam name="T">tabulka (trida se vygeneruje z edmx modelu</typeparam>
    public abstract class ZakladniProcedura<T> where T : class
    {

        public readonly IJednotkaPrace _jednotkaPrace;
        public DbSet<T> dbSet;

        public ZakladniProcedura(IJednotkaPrace jednotkaPrace)
        {
            if (jednotkaPrace == null) throw new ArgumentNullException("jednotkaPrace");
            this._jednotkaPrace = jednotkaPrace;
            this.dbSet = _jednotkaPrace.Db.Set<T>();
            return;
        }

    }
}

