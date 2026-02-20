using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrs_Informationen.Models
{
    class MainModel : ObservableObject
    {
        private Road _roads;
        public Road Roads
        {
           get => _roads;
           set
           {
               _roads = value;
               OnPropertyChanged();
           }
        }

    }
}
