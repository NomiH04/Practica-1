using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Driver para SQL Server ADO.net
using System.Data.SqlClient;

//Permite utilizar los objetos de la capa logica negocio
using BLL;
using System.Data;


namespace DAL
{
    public class Conexion
    {
        //Objeto para interactuar con el servidor de Base de Datos
        private string StringConexion;

        //Variable para manejar la referencia para la conexion
        private SqlConnection _connection;

        //Variable para ejecutar transac-sql del lado del servidor de Base de Datos
        private SqlCommand _command;

        //Variable que permite ejecutar transac-sql de consulta
        private SqlDataReader _reader;

        //Constructor con parámetros recibe el string de conexión
        public Conexion(string pStringCnx)
        {
            //Se asigna los datos de la conexion
            StringConexion = pStringCnx;

        }//Fin de constructor


        public void GuardarPaciente(Paciente paciente)
        {
            try
            {
                _connection = new SqlConnection(StringConexion);

                _connection.Open();

                _command = new SqlCommand();

                _command.Connection = _connection;

                _command.CommandType = CommandType.StoredProcedure;

                _command.CommandText = "[Sp_Ins_Paciente]";


                _command.Parameters.AddWithValue("@NombreCompleto", paciente.NombreCompleto);
                _command.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
                _command.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                _command.Parameters.AddWithValue("@Correo", paciente.CorreoElectronico);
                _command.Parameters.AddWithValue("@Direccion", paciente.Direccion);
                _command.Parameters.AddWithValue("@Foto", paciente.Foto);

                _command.ExecuteNonQuery();
                _connection.Close();
                _connection.Dispose();
                _command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ModificarPaciente(Paciente paciente)
        {
            try
            {
                _connection = new SqlConnection(StringConexion);

                _connection.Open();

                _command = new SqlCommand();

                _command.Connection = _connection;

                _command.CommandType = CommandType.StoredProcedure;


                _command.CommandText = "[Sp_Upd_Paciente]";

                _command.Parameters.AddWithValue("@IdPaciente", paciente.IdPaciente);
                _command.Parameters.AddWithValue("@NombreCompleto", paciente.NombreCompleto);
                _command.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
                _command.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                _command.Parameters.AddWithValue("@Correo", paciente.CorreoElectronico);
                _command.Parameters.AddWithValue("@Direccion", paciente.Direccion);
                _command.Parameters.AddWithValue("@Foto", paciente.Foto);

                _command.ExecuteNonQuery();
                _connection.Close();
                _connection.Dispose();
                _command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarPaciente(int idPaciente)
        {
            try
            {
                _connection = new SqlConnection(StringConexion);

                _connection.Open();

                _command = new SqlCommand();

                _command.Connection = _connection;

                _command.CommandType = CommandType.StoredProcedure;

                _command.CommandText = "[Sp_Del_Paciente]";
                _command.Parameters.AddWithValue("@IdPaciente", idPaciente);

                _command.ExecuteNonQuery();
                _connection.Close();
                _connection.Dispose();
                _command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ------------- MÉDICO -------------

        public void GuardarMedico(Medicos medico)
        {
            try
            {
                _connection = new SqlConnection(StringConexion);

                _connection.Open();

                _command = new SqlCommand();

                _command.Connection = _connection;

                _command.CommandType = CommandType.StoredProcedure;

                _command.CommandText = "[Sp_Ins_Medico]";

                _command.Parameters.AddWithValue("@NombreCompleto", medico.NombreCompleto);
                _command.Parameters.AddWithValue("@Especialidad", medico.Especialidad);
                _command.Parameters.AddWithValue("@Telefono", medico.Telefono);
                _command.Parameters.AddWithValue("@HorarioAtencion", medico.HorarioAtencion);
                _command.Parameters.AddWithValue("@Foto", medico.Foto);

                _command.ExecuteNonQuery();
                _connection.Close();
                _connection.Dispose();
                _command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ModificarMedico(Medicos medico)
        {
            try
            {
                _connection = new SqlConnection(StringConexion);

                _connection.Open();

                _command = new SqlCommand();

                _command.Connection = _connection;

                _command.CommandType = CommandType.StoredProcedure;

                _command.CommandText = "[Sp_Upd_Medico]";

                _command.Parameters.AddWithValue("@IdMedico", medico.IdMedico);
                _command.Parameters.AddWithValue("@NombreCompleto", medico.NombreCompleto);
                _command.Parameters.AddWithValue("@Especialidad", medico.Especialidad);
                _command.Parameters.AddWithValue("@Telefono", medico.Telefono);
                _command.Parameters.AddWithValue("@HorarioAtencion", medico.HorarioAtencion);
                _command.Parameters.AddWithValue("@Foto", medico.Foto);

                _command.ExecuteNonQuery();
                _connection.Close();
                _connection.Dispose();
                _command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarMedico(int idMedico)
        {
            try
            {
                _connection = new SqlConnection(StringConexion);

                _connection.Open();

                _command = new SqlCommand();

                _command.Connection = _connection;

                _command.CommandType = CommandType.StoredProcedure;

                _command.CommandText = "[Sp_Del_Medico]";
                _command.Parameters.AddWithValue("@IdMedico", idMedico);

                _command.ExecuteNonQuery();
                _connection.Close();
                _connection.Dispose();
                _command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ------------- CITA MÉDICA -------------

        public void GuardarCita(Citas cita)
        {
            try
            {
                _connection = new SqlConnection(StringConexion);

                _connection.Open();

                _command = new SqlCommand();

                _command.Connection = _connection;

                _command.CommandType = CommandType.StoredProcedure;

                _command.CommandText = "[Sp_Ins_CitaMedica]";

                _command.Parameters.AddWithValue("@IdPaciente", cita.IdPaciente);
                _command.Parameters.AddWithValue("@IdMedico", cita.IdMedico);
                _command.Parameters.AddWithValue("@FechaHora", cita.FechaHora);
                _command.Parameters.AddWithValue("@MotivoConsulta", cita.MotivoConsulta);
                _command.Parameters.AddWithValue("@Estado", cita.Estado);

                _command.ExecuteNonQuery();
                _connection.Close();
                _command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ModificarCita(Citas cita)
        {
            try
            {
                _connection = new SqlConnection(StringConexion);

                _connection.Open();

                _command = new SqlCommand();

                _command.Connection = _connection;

                _command.CommandType = CommandType.StoredProcedure;

                _command.CommandText = "[Sp_Upd_CitaMedica]";

                _command.Parameters.AddWithValue("@IdCita", cita.IdCita);
                _command.Parameters.AddWithValue("@IdPaciente", cita.IdPaciente);
                _command.Parameters.AddWithValue("@IdMedico", cita.IdMedico);
                _command.Parameters.AddWithValue("@FechaHora", cita.FechaHora);
                _command.Parameters.AddWithValue("@MotivoConsulta", cita.MotivoConsulta);
                _command.Parameters.AddWithValue("@Estado", cita.Estado);

                _command.ExecuteNonQuery();
                _connection.Close();
                _connection.Dispose();
                _command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarCita(int idCita)
        {
            try
            {
                _connection = new SqlConnection(StringConexion);

                _connection.Open();

                _command = new SqlCommand();

                _command.Connection = _connection;

                _command.CommandType = CommandType.StoredProcedure;

                _command.CommandText = "[Sp_Del_CitaMedica]";
                _command.Parameters.AddWithValue("@IdCita", idCita);

                _command.ExecuteNonQuery();
                _connection.Close();
                _connection.Dispose();
                _command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
