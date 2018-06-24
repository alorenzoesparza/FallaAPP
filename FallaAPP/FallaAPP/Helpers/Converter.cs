using FallaAPP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FallaAPP.Helpers
{
    public static class Converter
    {
        public static ComponenteLocal ToComponenteLocal(Componente componente)
        {
            return new ComponenteLocal
            {
                Apellidos = componente.Apellidos,
                ComponenteId = componente.ComponenteId,
                Email = componente.Email,
                Foto = componente.Foto,
                Foto500 = componente.Foto500,
                Nombre = componente.Nombre,
                Password = componente.Password,
                Telefono = componente.Telefono,
            };
        }

        public static Componente ToComponenteDomain(ComponenteLocal componente, byte[] imagenArray)
        {
            return new Componente
            {
                Apellidos = componente.Apellidos,
                ComponenteId = componente.ComponenteId,
                Email = componente.Email,
                Foto = componente.Foto,
                Foto500 = componente.Foto500,
                ImageArray = imagenArray,
                Nombre = componente.Nombre,
                Password = null,
                Telefono = componente.Telefono,
            };
        }
    }
}
