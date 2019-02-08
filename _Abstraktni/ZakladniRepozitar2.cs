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
    public abstract class ZakladniRepozitar2<T> : IZakladniRepozitar2<T> where T : class
    {

        public readonly IJednotkaPrace _jednotkaPrace;
        public DbSet<T> dbSet;
        private readonly bool READ_ONLY;


        public ZakladniRepozitar2(IJednotkaPrace jednotkaPrace, DbPristup dbPristup = DbPristup.READ_WRITE)
        {
            if (jednotkaPrace == null) throw new ArgumentNullException("jednotkaPrace");
            this._jednotkaPrace = jednotkaPrace;
            this.dbSet = _jednotkaPrace.Db.Set<T>();
            if (dbPristup == DbPristup.READ_ONLY)
            {
                this.READ_ONLY = true;
            }
            else
            {
                this.READ_ONLY = false;
            }
            return;
        }


        /// <summary>
        /// Vrati element podle PK.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public T GetPodlePK(params object[] primaryKey)
        {
            T result = dbSet.Find(primaryKey);

            return result;
        }


        /// <summary>
        /// Jak vypada primarni klic v dane tabulce.
        /// </summary>
        /// <param name="entita"></param>
        /// <returns></returns>
        public abstract dynamic GetKey(T entita);


        /// <summary>
        /// Vrati element podle PK.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual T Select(dynamic key)
        {
            T item = dbSet.Find(key);
            return item;
        }


        /// <summary>
        /// Ulozi novy element do DB.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual dynamic Save(T entity)
        {
            if (READ_ONLY) throw new InvalidOperationException("THIS TABLE IS READ-ONLY!");

            T obj = dbSet.Add(entity);
            ulozZmeny();
            return GetKey(obj);
        }


        /// <summary>
        /// Ulozi zmeny v EF a pokud najde specifickou chybu (pri kontrole dat),
        /// tak ji i spravne zobrazi.
        /// </summary>
        private void ulozZmeny()
        {
            try
            {
                this._jednotkaPrace.Db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }


        public void Insert(T entity)
        {
            if (READ_ONLY) throw new InvalidOperationException("THIS TABLE IS READ-ONLY!");

            T obj = dbSet.Add(entity);
            ulozZmeny();
        }


        /// <summary>
        /// Ulozi novy element do DB.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual dynamic Create(T entity)
        {
            return Save(entity);
        }


        /// <summary>
        /// Ulozi zmeny jiz existujiciho elementu v DB.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            if (READ_ONLY) throw new InvalidOperationException("THIS TABLE IS READ-ONLY!");

            dbSet.Attach(entity);
            _jednotkaPrace.Db.Entry(entity).State = EntityState.Modified;
            ulozZmeny();
            return;
        }


        /// <summary>
        /// Smaze existujici element z DB.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual dynamic Delete(T entity)
        {
            if (READ_ONLY) throw new InvalidOperationException("THIS TABLE IS READ-ONLY!");

            if (_jednotkaPrace.Db.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            T obj = dbSet.Remove(entity);
            ulozZmeny();
            return GetKey(obj);
        }


        public IEnumerable<T> GetVsechnyKde(Expression<Func<T, bool>> wherePredicate, params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                dbSet.Include(property);
            }
            return dbSet.Where(wherePredicate).ToList<T>();
        }


    }
}

