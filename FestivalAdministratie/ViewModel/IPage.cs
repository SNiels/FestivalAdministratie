using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalAdministratie.ViewModel
{
    public interface IPage//interace for navigation
    {
        string Name { get; }
    }

    public interface BeheerIPage : IPage//interface for navigation for some more specific pages
    {
        string NameEnkel { get; }
        bool IsAnItemSelected { get; }
    }
}
