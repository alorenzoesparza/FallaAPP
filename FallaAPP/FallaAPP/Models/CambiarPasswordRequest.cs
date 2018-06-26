using System;
using System.Collections.Generic;
using System.Text;

namespace FallaAPP.Models
{
    public class CambiarPasswordRequest
    {
        public string ActualPassword { get; set; }

        public string Email { get; set; }

        public string NuevoPassword { get; set; }
    }
}
