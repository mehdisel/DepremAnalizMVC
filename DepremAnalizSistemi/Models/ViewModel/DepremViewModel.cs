using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DepremAnalizSistemi.Models.ViewModel
{
    public class DepremViewModel
    {
        private string _depremYeri = "seciniz";
        private decimal? _enlem = null;
        private decimal? _boylam = null;
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string DepremYeri { get { return _depremYeri; } set { _depremYeri = value; } }
        public decimal? MinSiddet { get; set; }
        public decimal? MaxSiddet { get; set; }
        public decimal? MinDerinlik { get; set; }
        public decimal? MaxDerinlik { get; set; }
        public decimal? Enlem
        {
            get
            {
                return _enlem;
            }
            set
            {
                if (value>0)
                {
                    _enlem = value;

                }
                else
                {
                    _enlem = null;
                }
            }
        }
        public decimal? Boylam
        {
            get
            {
                return _boylam;
            }
            set
            {
                if (value > 0)
                {
                    _boylam = value;

                }
                else
                {
                    _boylam = null;
                }
            }
        }
    }
}