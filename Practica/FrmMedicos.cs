using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;
using System.IO;
using System.Drawing.Drawing2D;

namespace Practica
{
    public partial class FrmMedicos : Form
    {

        private readonly Conexion _conexion; // Conexión a la base de datos
        private Medicos _medico; // Datos del medico

        public FrmMedicos()
        {
            InitializeComponent();
            _conexion = new Conexion(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamamos a verificar los campos antes de guardar
                if (verificar_Campos())
                {
                    // Crear un nuevo objeto medico con los datos del formulario
                    Medicos medico = new Medicos()
                    {
                        NombreCompleto = txtNombre.Text,
                        Especialidad = txtEspecialidad.Text,
                        Telefono = txtTelefono.Text,
                        HorarioAtencion = txtHorarioAtencion.Text,
                        Foto = Guardar_Imagen(pbFoto.Image) // Guardar la foto
                    };

                    // Llamar al método para guardar el medico en la base de datos
                    _conexion.GuardarMedico(medico);

                    MessageBox.Show("Medico guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario(); // Limpiar los campos después de guardar
                }
            }
            catch (Exception ex)
            {
                // Si se detecta algún error en la validación, se muestra el mensaje
                MessageBox.Show("Error al guardar el medico: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            txtIdMedico.Clear();
            txtNombre.Clear();
            txtEspecialidad.Clear();
            txtTelefono.Clear();
            txtHorarioAtencion.Clear();
            pbFoto.Image = null; // Limpiar la foto
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamamos a verificar los campos antes de modificar
                if (verificar_Campos())
                {
                    // Crear un objeto medico con los datos actualizados del formulario
                    Medicos medico = new Medicos()
                    {
                        IdMedico = Convert.ToInt32(txtIdMedico.Text),
                        NombreCompleto = txtNombre.Text,
                        Especialidad = txtEspecialidad.Text,
                        Telefono = txtTelefono.Text,
                        HorarioAtencion = txtHorarioAtencion.Text,
                        Foto = Guardar_Imagen(pbFoto.Image) // Guardar la foto
                    };

                    // Llamar al método para modificar el medico en la base de datos
                    _conexion.ModificarMedico(medico);

                    MessageBox.Show("Medico modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario(); // Limpiar los campos después de modificar
                }
            }
            catch (Exception ex)
            {
                // Si se detecta algún error en la validación, se muestra el mensaje
                MessageBox.Show("Error al modificar el medico: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificamos que el campo de IdMedico esté completo
                if (string.IsNullOrWhiteSpace(txtIdMedico.Text))
                {
                    MessageBox.Show("Por favor, ingrese un ID de medico para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idMedico = Convert.ToInt32(txtIdMedico.Text);

                // Confirmación de eliminación
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar al medico?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Llamar al método para eliminar el medico en la base de datos
                    _conexion.EliminarMedico(idMedico);

                    MessageBox.Show("Medico eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario(); // Limpiar los campos después de eliminar
                }
            }
            catch (Exception ex)
            {
                // Si se detecta algún error, se muestra el mensaje
                MessageBox.Show("Error al eliminar el medico: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Función para guardar la imagen
        private string Guardar_Imagen(Image imagen)
        {
            if (imagen == null)
            {
                return null;
            }

            try
            {
                string carpetaFotos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fotos");

                if (!Directory.Exists(carpetaFotos))
                {
                    Directory.CreateDirectory(carpetaFotos);
                }

                string ruta = Path.Combine(carpetaFotos, $"{Guid.NewGuid()}.png");

                imagen.Save(ruta, System.Drawing.Imaging.ImageFormat.Png);

                return ruta;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Función para seleccionar una imagen
        private void pbEditarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Imágenes|*.jpg;*.png;*.bmp";
                openFileDialog.Title = "Seleccionar imagen";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image imagenSeleccionada = Image.FromFile(openFileDialog.FileName);

                        // Redimensionar la imagen si es necesario
                        pbFoto.Image = Redimensionar_Y_Recortar_Circular(imagenSeleccionada, 50, 50);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Redimensionar y recortar la imagen circularmente
        private Image Redimensionar_Y_Recortar_Circular(Image imagen, int ancho, int alto)
        {
            Bitmap bitmapRedimensionado = new Bitmap(ancho, alto);

            using (Graphics g = Graphics.FromImage(bitmapRedimensionado))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, ancho, alto);
                    g.SetClip(path);

                    float ratioImagen = (float)imagen.Width / imagen.Height;
                    float ratioDestino = (float)ancho / alto;

                    RectangleF destino;
                    if (ratioImagen > ratioDestino)
                    {
                        float escala = (float)alto / imagen.Height;
                        float nuevoAncho = imagen.Width * escala;
                        float offsetX = (nuevoAncho - ancho) / 2;
                        destino = new RectangleF(-offsetX, 0, nuevoAncho, alto);
                    }
                    else
                    {
                        float escala = (float)ancho / imagen.Width;
                        float nuevoAlto = imagen.Height * escala;
                        float offsetY = (nuevoAlto - alto) / 2;
                        destino = new RectangleF(0, -offsetY, ancho, nuevoAlto);
                    }

                    g.DrawImage(imagen, destino);
                }
            }

            return bitmapRedimensionado;
        }

        public bool verificar_Campos()
        {
            List<string> mensajesError = new List<string>();

            // Validar que el campo de Nombre no esté vacío
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                mensajesError.Add("El campo de nombre no puede estar vacío.");
            }

            // Validar que el campo de Especialidad no esté vacío
            if (string.IsNullOrWhiteSpace(txtEspecialidad.Text))
            {
                mensajesError.Add("El campo de especialidad no puede estar vacío.");
            }

            // Validar que el campo de Teléfono no esté vacío
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                mensajesError.Add("El campo de teléfono no puede estar vacío.");
            }

            // Validar que el campo de Horario de Atención no esté vacío
            if (string.IsNullOrWhiteSpace(txtHorarioAtencion.Text))
            {
                mensajesError.Add("El campo de horario de atención no puede estar vacío.");
            }

            // Validación de Foto (si es obligatorio)
            if (pbFoto.Image == null)
            {
                mensajesError.Add("Debe seleccionar una foto para el medico.");
            }

            // Si hay errores, lanzamos una excepción con los mensajes de error
            if (mensajesError.Count > 0)
            {
                throw new Exception("Errores de validación: \n" + string.Join("\n", mensajesError));
            }

            return true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Quieres regresar a la pantalla principal?", "",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.None)
               == DialogResult.Yes)
            {
                FrmPrincipal pantallaPrincipal = new FrmPrincipal();
                pantallaPrincipal.Show();
                this.Close();
            }
        }
    }
}
