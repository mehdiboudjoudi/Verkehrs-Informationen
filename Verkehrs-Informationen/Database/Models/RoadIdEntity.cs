using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Verkehrs_Informationen.Database.Models

{
    public class RoadIdEntity

    {
        [Key]
        public int Id { get; set; }

        public string Road { get; set; } = "";
    }
}