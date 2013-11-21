using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalAdministratie.ViewModel
{
    public interface IPage
    {
        string Name { get; }
    }

    public interface BeheerIPage : IPage
    {
        string NameEnkel { get; }
        bool IsAnItemSelected { get; }
    }
}
