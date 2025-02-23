using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practica
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmPaciente pantallaPaciente = new FrmPaciente();
            pantallaPaciente.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmMedicos pantallaMedicos = new FrmMedicos();
            pantallaMedicos.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmCitas pantallaCitas = new FrmCitas();
            pantallaCitas.Show();
            this.Hide();
        }
    }
}
