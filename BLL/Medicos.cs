using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL
{
    public class Medicos
    {
        public int IdMedico { get; set; }

        public string Foto { get; set; }

        public string NombreCompleto { get; set; }

        public string Especialidad { get; set; }

        public string Telefono { get; set; }

        public string HorarioAtencion { get; set; }
    }
}
