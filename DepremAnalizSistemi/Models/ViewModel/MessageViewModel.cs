using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DepremAnalizSistemi.Models.ViewModel
{
    public class MessageViewModel
    {
        public string MesajBasligi { get; set; }
        public string MesajIcerik { get; set; }
        public int AliciID { get; set; }
        public int DepremID { get; set; }
    }
}