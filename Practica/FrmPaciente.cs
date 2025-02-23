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
    public partial class FrmPaciente : Form
    {
        public FrmPaciente()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Quieres cerrar el sistema?", "",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.None)
               == DialogResult.Yes)
            {
                Environment.Exit(0);

            }
        }
    }
}
