using System.Data.SqlClient;

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

    }
}
