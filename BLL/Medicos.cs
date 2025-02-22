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
        [Key]
        public int IdMedico { get; set; }

        public byte[] Foto { get; set; }

        [Required, MaxLength(250)]
        public string NombreCompleto { get; set; }

        [Required, MaxLength(100)]
        public string Especialidad { get; set; }

        [Required, MaxLength(15)]
        public string Telefono { get; set; }

        [Required, MaxLength(100)]
        public string HorarioAtencion { get; set; }
    }
}
