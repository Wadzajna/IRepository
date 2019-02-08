using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace BoschAspApps.Utils.IDAL
{
    public interface IZakladniRepozitar2<T> where T : class
    {

        T GetPodlePK(params object[] primaryKey);

        T Select(dynamic key);

        dynamic Create(T entity);

        dynamic Save(T entity);

        void Insert(T entity);

        void Update(T entity);

        dynamic Delete(T entity);

    }
}
