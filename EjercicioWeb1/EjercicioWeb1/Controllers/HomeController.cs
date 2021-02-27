using Microsoft.AspNetCore.Mvc;
using EjercicioWeb1.Models;
using System.Data;

namespace EjercicioWeb1.Controllers
{
    public class HomeController : Controller
    { 
        public IActionResult Index()
        {
            DataSet vDs = new DataSet();
            ProductoViewModel objProducto = new ProductoViewModel();
            vDs = objProducto.ObtenProductos();
            return View(vDs);
        }

        public ViewResult Registrar()
        {
            return View();
        }

        public ViewResult Editar(int prmID)
        {
            ProductoViewModel objProducto = new ProductoViewModel();
            objProducto.CargarProducto(prmID);

            return View(objProducto);
        }

        [HttpPost]
        public JsonResult Guardar(ProductoViewModel objProducto)
        {
            objProducto.bActivo = true;
            if (!objProducto.Guardar())
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult EditarRegistro(ProductoViewModel objProducto)
        {
            objProducto.bActivo = true;
            if (!objProducto.Actualizar())
            {
                return Json(new { success = false });
            }

            return Json(new { success = true});
        }

        [HttpPost]
        public JsonResult Eliminar(int prmID)
        {
            ProductoViewModel objProducto = new ProductoViewModel();
            objProducto.nProducto = prmID;

           if(!objProducto.Eliminar(prmID))
           {
                return Json(new { success = false});
            }

            return Json(new { success = true});
        }

    }
}
