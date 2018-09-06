namespace FallaAPP.Models
{
    public class AsistenciaEvento
    {
        public int AsistenciaEventoId { get; set; }

        public int IdEvento { get; set; }

        public int ComponenteId { get; set; }

        public bool EsInfantil { get; set; }

        public decimal Precio { get; set; }

        public override int GetHashCode()
        {
            return AsistenciaEventoId;
        }
    }
}
