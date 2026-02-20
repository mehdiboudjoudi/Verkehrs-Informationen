using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Verkehrs_Informationen.Database.Models

{
    public class ClosureEntity
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Subtitle { get; set; } = "";

        public string IsBlocked { get; set; } = "";

        public string FullDescription { get; set; } = "";
    }
}