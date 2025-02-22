using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Citas
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int PacienteID { get; set; }

        [Required]
        public int MedicoID { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required, MaxLength(255)]
        public string MotivoConsulta { get; set; }

        [Required, MaxLength(20)]
        public string Estado { get; set; }

        [ForeignKey("PacienteID")]
        public Paciente Paciente { get; set; }

        [ForeignKey("MedicoID")]
        public Medicos Medico { get; set; }
    }
}
