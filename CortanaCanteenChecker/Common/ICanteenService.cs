using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Common
{
    public interface ICanteenService
    {
        IAsyncOperation<IEnumerable<Canteen>> GetCanteens(string nameFilter, string dishFilter);
    }
}
