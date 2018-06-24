using SQLite.Net.Attributes;

namespace FallaAPP.Models
{
    public class ComponenteLocal
    {
        [PrimaryKey]
        public int ComponenteId { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public string Foto { get; set; }

        public string Foto500 { get; set; }

        public string Password { get; set; }

        public string FotoFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Foto))
                {
                    return "No hay Foto";
                }

                return string.Format(
                    "http://antoniole.com/{0}",
                    Foto.Substring(1));
            }
        }

        public string NombreCompleto
        {
            get
            {
                return string.Format("{0}, {1}", Apellidos, Nombre);
            }
        }

        public override int GetHashCode()
        {
            return ComponenteId;
        }
    }
}

