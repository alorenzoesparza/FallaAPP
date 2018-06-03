using System;
using System.Collections.Generic;
using System.Text;

namespace FallaAPP.Models
{
    public class Act
    {
        public int IdAct { get; set; }
        public string Titular { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaActo { get; set; }
        public string StrFecha
        {
            get
            {
                return string.Format("{0:d}", FechaActo);
            }
        }
        public string HoraActo { get; set; }
        public double? Precio { get; set; }
        public double? PrecioInfantiles { get; set; }
        public bool ActoOficial { get; set; }
        public string Imagen { get; set; }
        public string ImagenFullPath
        {
            get
            {
                return string.Format(
                    "http://antoniole.com/{0}",
                    Imagen.Substring(1));
            }
        }
        public string Imagen500 { get; set; }
        public bool PagInicio { get; set; }
        public bool YaEfectuado { get; set; }
    }
}
