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
        public string HoraActo { get; set; }
        public string Precio { get; set; }
        public string PrecioInfantiles { get; set; }
        public bool ActoOficial { get; set; }
        public string Imagen { get; set; }
        public string ImagenFullPath
        {
            get
            {
                return string.Format(
                    "http://antoniole.com/Falla/{0}",
                    Imagen500.Substring(1));
            }
        }
        public object ImagenFile { get; set; }
        public string Imagen500 { get; set; }
        public bool PagInicio { get; set; }
        public bool YaEfectuado { get; set; }
    }
}
