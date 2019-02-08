using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoschAspApps.Utils.IDAL
{
    public interface IDbRepozitar<T> : IZakladniRepozitar2<T> where T : class
    {

    }
}
