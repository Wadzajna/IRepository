using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoschAspApps.Utils.IDAL
{
    public interface IDbsetControl<T> where T : class
    {
        IQueryable<T> GetSet();
        List<T> GetList();
        T GetView(int id);
    }
}
