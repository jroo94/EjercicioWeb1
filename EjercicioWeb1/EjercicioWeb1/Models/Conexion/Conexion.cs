using System.Data.Common;

namespace EjercicioWeb1.Models
{
    public class Conexion : Conn
    {
        public static string ConnectionString;
        public static Conexion  ObtenConexion()
        {
            Conexion connn = new Conexion();
            System.Data.Common.DbConnection c = new System.Data.SqlClient.SqlConnection();
            connn.InicializaClase(ref c);

            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);

            connn.EstablecerConnectionString(builder.ConnectionString);
            connn.AbreConexion();
            return connn;

        }

        public override DbParameter DbParameter(string parametro, object valor)
        {
            return new System.Data.SqlClient.SqlParameter(parametro, valor);
        }

        protected override DbCommandBuilder ObtenCommandBuilder(ref DbDataAdapter adapter)
        {
            return new System.Data.SqlClient.SqlCommandBuilder((System.Data.SqlClient.SqlDataAdapter)adapter);
        }

        protected override DbDataAdapter ObtenerAdapter(string consultaSQL)
        {
            return new System.Data.SqlClient.SqlDataAdapter(consultaSQL, (System.Data.SqlClient.SqlConnection)this.Conexion);
        }
    }
}
