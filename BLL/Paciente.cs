using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using System.ComponentModel.DataAnnotations;

namespace BLL
{
    public class Paciente
    {
        [Key]
        public int IdPaciente { get; set; }

        public byte[] Foto { get; set; }

        [Required, MaxLength(250)]
        public string NombreCompleto { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required, MaxLength(15)]
        public string Telefono { get; set; }

        [Required, MaxLength(100)]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required, MaxLength(255)]
        public string Direccion { get; set; }

    }
}
