using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class FrmPaciente : Form
    {
        private readonly Conexion _conexion; // Conexión a la base de datos
        private Paciente _paciente; // Datos del paciente

        private bool mostrandoClave = true;

        public FrmPaciente()
        {
            InitializeComponent();
            _conexion = new Conexion(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamamos a verificar los campos antes de guardar
                if (verificar_Campos())
                {
                    // Crear un nuevo objeto paciente con los datos del formulario
                    Paciente paciente = new Paciente()
                    {
                        NombreCompleto = txtNombre.Text,
                        FechaNacimiento = dtpFechaNacimiento.Value,
                        Telefono = txtTelefono.Text,
                        CorreoElectronico = txtCorreo.Text,
                        Direccion = txtDireccion.Text,
                        Foto = Guardar_Imagen(pbFoto.Image) // Guardar la foto
                    };

                    // Llamar al método para guardar el paciente en la base de datos
                    _conexion.GuardarPaciente(paciente);

                    MessageBox.Show("Paciente guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario(); // Limpiar los campos después de guardar
                }
            }
            catch (Exception ex)
            {
                // Si se detecta algún error en la validación, se muestra el mensaje
                MessageBox.Show("Error al guardar el paciente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LimpiarFormulario()
        {
            txtIdPaciente.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
            dtpFechaNacimiento.Value = DateTime.Now;
            pbFoto.Image = null; // Limpiar la foto
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamamos a verificar los campos antes de modificar
                if (verificar_Campos())
                {
                    // Crear un objeto paciente con los datos actualizados del formulario
                    Paciente paciente = new Paciente()
                    {
                        IdPaciente = Convert.ToInt32(txtIdPaciente.Text),
                        NombreCompleto = txtNombre.Text,
                        FechaNacimiento = dtpFechaNacimiento.Value,
                        Telefono = txtTelefono.Text,
                        CorreoElectronico = txtCorreo.Text,
                        Direccion = txtDireccion.Text,
                        Foto = Guardar_Imagen(pbFoto.Image) // Guardar la foto
                    };

                    // Llamar al método para modificar el paciente en la base de datos
                    _conexion.ModificarPaciente(paciente);

                    MessageBox.Show("Paciente modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario(); // Limpiar los campos después de modificar
                }
            }
            catch (Exception ex)
            {
                // Si se detecta algún error en la validación, se muestra el mensaje
                MessageBox.Show("Error al modificar el paciente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificamos que el campo de IdPaciente esté completo
                if (string.IsNullOrWhiteSpace(txtIdPaciente.Text))
                {
                    MessageBox.Show("Por favor, ingrese un ID de paciente para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idPaciente = Convert.ToInt32(txtIdPaciente.Text);

                // Confirmación de eliminación
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar al paciente?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Llamar al método para eliminar el paciente en la base de datos
                    _conexion.EliminarPaciente(idPaciente);

                    MessageBox.Show("Paciente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario(); // Limpiar los campos después de eliminar
                }
            }
            catch (Exception ex)
            {
                // Si se detecta algún error, se muestra el mensaje
                MessageBox.Show("Error al eliminar el paciente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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



        private void Modificar_Paciente()
        {
            try
            {
                // Asignar los valores de los controles a las propiedades del objeto Paciente
                this._paciente.IdPaciente = Convert.ToInt32(txtIdPaciente.Text.Trim()); // Asignar el ID del paciente
                this._paciente.NombreCompleto = txtNombre.Text.Trim();
                this._paciente.FechaNacimiento = dtpFechaNacimiento.Value;
                this._paciente.Telefono = txtTelefono.Text.Trim();
                this._paciente.CorreoElectronico = txtCorreo.Text.Trim();
                this._paciente.Direccion = txtDireccion.Text.Trim();
                this._paciente.Foto = Guardar_Imagen(pbFoto.Image); // Guardar la imagen del paciente

                // Realizar la modificación en la base de datos
                _conexion.ModificarPaciente(this._paciente);

                // Mostrar mensaje de éxito
                MessageBox.Show("Paciente modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar formulario después de la modificación
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                MessageBox.Show("Error al modificar el paciente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool verificar_Campos()
        {
            List<string> mensajesError = new List<string>();

            // Validar que el campo de Nombre no esté vacío
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                mensajesError.Add("El campo de nombre no puede estar vacío.");
            }

            

            // Validar que el campo de Correo no esté vacío
            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                mensajesError.Add("El campo de correo electrónico no puede estar vacío.");
            }
            else
            {
                // Validar formato del correo electrónico
                try
                {
                    var mail = new System.Net.Mail.MailAddress(txtCorreo.Text.Trim());
                }
                catch
                {
                    mensajesError.Add("El correo electrónico no tiene un formato válido.");
                }
            }

            // Validar que el campo de Teléfono no esté vacío
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                mensajesError.Add("El campo de teléfono no puede estar vacío.");
            }

            // Validar que la Fecha de Nacimiento sea válida (es decir, no sea una fecha futura)
            if (dtpFechaNacimiento.Value > DateTime.Now)
            {
                mensajesError.Add("La fecha de nacimiento no puede ser una fecha futura.");
            }

            // Validar que la Dirección no esté vacía
            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                mensajesError.Add("El campo de dirección no puede estar vacío.");
            }

            // Validación de Foto (si es obligatorio)
            if (pbFoto.Image == null)
            {
                mensajesError.Add("Debe seleccionar una foto para el paciente.");
            }

            // Si hay errores, lanzamos una excepción con los mensajes de error
            if (mensajesError.Count > 0)
            {
                throw new Exception("Errores de validación: \n" + string.Join("\n", mensajesError));
            }

            return true;
        }

    }
}
