using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BLL
{
    public class Citas
    {
        [Key]
        public int IdCita { get; set; }

        [Required]
        public int IdPaciente { get; set; }

        [Required]
        public int IdMedico { get; set; }

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
