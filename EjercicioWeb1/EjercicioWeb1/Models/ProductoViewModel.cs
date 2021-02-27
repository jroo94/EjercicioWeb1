using System.Data;

namespace EjercicioWeb1.Models
{
    public class ProductoViewModel
    {
        public int nProducto { get; set; }
        public string cDescripcion { get; set; }
        public decimal nPrecio { get; set; }
        public int nPorcIVA { get; set; }
        public decimal nImporteIVA { get; set; }
        public decimal nPrecioConIVA { get; set; }
        public bool bActivo { get; set; }

        public void CargarProducto(int prmID)
        {
            DataTable vDt = new DataTable();
            Conexion conexion = Conexion.ObtenConexion();
            this.nProducto = prmID;
            System.Data.Common.DbParameter prmProducto = conexion.DbParameter("@Producto", this.nProducto);
            vDt =  conexion.ObtenDataTable("SELECT * FROM CTL_Productos WHERE nProducto = @Producto", prmProducto);

            this.cDescripcion = vDt.Rows[0]["cDescripcion"].ToString();
            this.nPrecio = (decimal)vDt.Rows[0]["nPrecio"];
            this.nPorcIVA = (int)vDt.Rows[0]["nPorcIVA"];
            this.nImporteIVA = (decimal)vDt.Rows[0]["nImporteIVA"];
            this.nPrecioConIVA = (decimal)vDt.Rows[0]["nPrecioConIVA"];
            this.bActivo = (bool)vDt.Rows[0]["bActivo"];
        }

        public DataSet ObtenProductos()
        {
            Conexion conexion = Conexion.ObtenConexion();
            return conexion.ObtenDataSet("SP_ArmaTabla_CTL_Productos");
        }

        public bool Guardar()
        {
            Conexion conexion = Conexion.ObtenConexion();

            System.Data.Common.DbParameter prmDescripcion = conexion.DbParameter("@Descripcion", this.cDescripcion);
            System.Data.Common.DbParameter prmPrecio = conexion.DbParameter("@Precio", this.nPrecio);
            System.Data.Common.DbParameter prmPorcIva = conexion.DbParameter("@PorcIva", this.nPorcIVA);
            System.Data.Common.DbParameter prmImporteIva = conexion.DbParameter("@ImporteIva", this.nImporteIVA);
            System.Data.Common.DbParameter prmPrecioIva = conexion.DbParameter("@PrecioIva", this.nPrecioConIVA);
            System.Data.Common.DbParameter prmActivo = conexion.DbParameter("@Activo", this.bActivo);

           if(conexion.Ejecutar("INSERT INTO CTL_Productos SELECT @Descripcion, @Precio, @PorcIva, @ImporteIva, @PrecioIva, @Activo", prmDescripcion, prmPrecio,
                prmPorcIva, prmImporteIva, prmPrecioIva, prmActivo) != 1)
            {
                return false;
            }

            return true;
        }

        public bool Actualizar()
        {
            Conexion conexion = Conexion.ObtenConexion();

            System.Data.Common.DbParameter prmID = conexion.DbParameter("@ID", this.nProducto);
            System.Data.Common.DbParameter prmDescripcion = conexion.DbParameter("@Descripcion", this.cDescripcion);
            System.Data.Common.DbParameter prmPrecio = conexion.DbParameter("@Precio", this.nPrecio);
            System.Data.Common.DbParameter prmPorcIva = conexion.DbParameter("@PorcIva", this.nPorcIVA);
            System.Data.Common.DbParameter prmImporteIva = conexion.DbParameter("@ImporteIva", this.nImporteIVA);
            System.Data.Common.DbParameter prmPrecioIva = conexion.DbParameter("@PrecioIva", this.nPrecioConIVA);

            if (conexion.Ejecutar("UPDATE CTL_Productos SET cDescripcion = @Descripcion, nPrecio = @Precio, nPorcIva = @PorcIva, nImporteIva = @ImporteIva, nPrecioConIva = @PrecioIva WHERE nProducto = @ID", prmDescripcion, prmPrecio,
                prmPorcIva, prmImporteIva, prmPrecioIva, prmID) != 1)
            {
                return false;
            }

            return true;
        }

        public bool Eliminar(int prmID)
        {
            Conexion conexion = Conexion.ObtenConexion();
            System.Data.Common.DbParameter prmProducto = conexion.DbParameter("@Producto", prmID);
            if(conexion.Ejecutar("UPDATE CTL_Productos SET bActivo = 0 WHERE nProducto = @Producto", prmProducto) != 1)
            {
                return false;
            }

            return true;
        }

    }
}
