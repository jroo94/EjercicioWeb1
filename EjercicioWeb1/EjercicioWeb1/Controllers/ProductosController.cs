using EjercicioWeb1.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjercicioWeb1.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index()
        {
            Conexion conexion = Conexion.ObtenConexion();
            return View();
        }
    }
}
