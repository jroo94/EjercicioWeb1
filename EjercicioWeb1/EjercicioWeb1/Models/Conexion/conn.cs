using System;
using System.Data;
using System.IO;

namespace EjercicioWeb1.Models
{
    public abstract class Conn
    {
        protected System.Data.Common.DbConnection Conexion;
        protected System.Data.Common.DbTransaction Trans;
        private System.Data.Common.DbCommand Command;
        public Exception UltimaExcepcion;
        public string UltimaConsulta;
        #region "Inicializadores"
        protected Conn()
        {
        }
        protected void InicializaClase(ref System.Data.Common.DbConnection conn)
        {
            Conexion = conn;
        }

        protected Conn(ref System.Data.Common.DbConnection conn)
        {
            InicializaClase(ref conn);
        }

        protected void EstablecerConnectionString(string connection)
        {
            Conexion.ConnectionString = connection;
        }
        #endregion


        #region "Funciones Obtener"
        public virtual DataSet ObtenDataSet(string Query, params System.Data.Common.DbParameter[] parametros)
        {
            try
            {
                System.Data.DataSet ds = new DataSet();
                this.AbreConexion();
                this.UltimaConsulta = Query;
                System.Data.Common.DbDataAdapter adapter = this.ObtenerAdapter();
                this.ObtenComando().CommandText = Query;
                adapter.SelectCommand = this.ObtenComando();
                this.ObtenComando().Parameters.Clear();
                this.ObtenComando().Parameters.AddRange(parametros);
                if (Query.Contains(" "))
                    this.ObtenComando().CommandType = CommandType.Text;
                else
                    this.ObtenComando().CommandType = CommandType.StoredProcedure;


                adapter.Fill(ds);

                return ds;
            }
            catch (IOException ex)
            {
                this.UltimaExcepcion = ex;
                this.CerrarConexion();
                return null;
            }
            catch (System.Data.Common.DbException ex)
            {
                this.UltimaExcepcion = ex;
                this.CerrarConexion();
                return null;
            }
            catch (Exception ex)
            {
                this.UltimaExcepcion = ex;
                return null;
            }
            finally
            {
                if (!this.TieneTransaccion())
                {
                    this.CerrarConexion();
                }
            }
        }

        public virtual DataTable ObtenDataTable(string Query, params System.Data.Common.DbParameter[] parametros)
        {
            DataSet ds = ObtenDataSet(Query, parametros);
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }

        #endregion
        public virtual int Ejecutar(string Query, params System.Data.Common.DbParameter[] parametros)
        {
            this.AbreConexion();
            this.UltimaConsulta = Query;
            this.ObtenComando().CommandText = Query;
            this.ObtenComando().Parameters.Clear();
            foreach (System.Data.Common.DbParameter par in parametros)
                this.ObtenComando().Parameters.Add(par);
            if (Query.Contains(" "))
                this.ObtenComando().CommandType = CommandType.Text;
            else
                this.ObtenComando().CommandType = CommandType.StoredProcedure;
            int rest = this.ObtenComando().ExecuteNonQuery();
            this.ObtenComando().Parameters.Clear();

            if (!this.TieneTransaccion())
            {
                this.CerrarConexion();
            }

            return rest;
        }
        protected System.Data.Common.DbCommand ObtenComando()
        {
            this.AbreConexion();
            if (Command == null)
                Command = Conexion.CreateCommand();
            return Command;
        }
        protected System.Data.Common.DbDataAdapter ObtenerAdapter()
        {
            return this.ObtenerAdapter("");
        }
        protected abstract System.Data.Common.DbDataAdapter ObtenerAdapter(string consultaSQL);
        public abstract System.Data.Common.DbParameter DbParameter(string parametro, object valor);
        protected abstract System.Data.Common.DbCommandBuilder ObtenCommandBuilder(ref System.Data.Common.DbDataAdapter adapter);

        public void CerrarConexion()
        {
            if (this.Conexion.State == ConnectionState.Open)
            {
                if (this.TieneTransaccion())
                {
                    this.CierraTransaccion();
                }
                this.Conexion.Close();
            }
        }

        public void AbreConexion()
        {
            if (this.Conexion.State != ConnectionState.Open && this.Conexion.State != ConnectionState.Connecting)
                this.Conexion.Open();
        }
        #region "Transacciones"
        public void IniciaTransaccion()
        {
            try
            {
                this.AbreConexion();
                if (this.Trans == null)
                {
                    this.Trans = Conexion.BeginTransaction();
                    this.ObtenComando().Transaction = this.Trans;
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void DeshaceTransaccion()
        {
            try
            {
                if (this.Trans != null)
                {
                    this.Trans.Rollback();
                    this.Trans = null;
                    this.CerrarConexion();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void CierraTransaccion()
        {
            try
            {
                if (this.Trans != null)
                {
                    this.Trans.Commit();
                    this.Trans = null;
                    this.CerrarConexion();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool TieneTransaccion()
        {
            return !(this.Trans == null);
        }
        #endregion

        public virtual DateTime FgObtenFecha()
        {
            return DateTime.Now;
        }

    }
}

