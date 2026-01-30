using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrs_Informationen.Models.APIModels
{
    public class WarningAPIModel
    {
        public List<WarningItemAPIModel> Warning { get; set; }
    }

    public class WarningItemAPIModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
    }
}
