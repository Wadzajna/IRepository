using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace BoschAspApps.Utils.IDAL
{
    public interface IJednotkaPrace : IDisposable
    {

        DbContext Db { get; }

        /// <summary>
        /// Vytvori novou transakci. Tu na konci bud:
        /// Commit()
        /// RollBack()
        /// </summary>
        /// <returns></returns>
        DbContextTransaction Transakce_New();

        /// <summary>
        /// Vraci zalozenou transakci nebo NULL, pokud zadna neni.
        /// </summary>
        /// <returns></returns>
        DbContextTransaction Transakce_Current();

        /// <summary>
        /// Vytvori zalozenou transakci nebo zalozi novou.
        /// </summary>
        /// <returns></returns>
        DbContextTransaction Transakce_CurrentNew();


        /// <summary>
        /// Vrati pripojeni pro rucni dotazovani (SqlConnection / SqlCommand)
        /// </summary>
        /// <returns></returns>
        DbConnection Connection();


    }
}
