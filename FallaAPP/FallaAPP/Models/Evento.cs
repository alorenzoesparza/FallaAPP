using System;

namespace FallaAPP.Models
{
    public class Evento
    {
        public int IdEvento { get; set; }

        public string Titular { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaEvento { get; set; }

        public string StrFecha
        {
            get
            {
                return string.Format("{0:d}", FechaEvento);
            }
        }

        public string HoraEvento { get; set; }

        public decimal Precio { get; set; }

        public double? PrecioInfantiles { get; set; }

        public bool EventoOficial { get; set; }

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

        public bool Apuntado { get; set; }

        public int AsistenciaEventoId { get; set; }

        public override int GetHashCode()
        {
            return AsistenciaEventoId;
        }
    }
}
