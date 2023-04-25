using Microsoft.AspNetCore.Mvc;
using MvcCoreCacheRedisProductos.Models;
using MvcCoreCacheRedisProductos.Repositories;
using MvcCoreCacheRedisProductos.Services;

namespace MvcCoreCacheRedisProductos.Controllers
{
    public class ProductosController : Controller
    {
        private RepositoryProductos repo;
        private ServiceCacheRedis service;
        public ProductosController(RepositoryProductos repo
            , ServiceCacheRedis service) 
        {
            this.repo = repo;
            this.service = service;
        }

        public IActionResult Favoritos()
        {
            List<Producto> productosFavoritos =
                this.service.GetProductosFavoritos();
            return View(productosFavoritos);
        }

        public IActionResult SeleccionarFavorito(int idproducto)
        {
            //BUSCAMOS EL PRODUCTO DENTRO DEL XML PARA RECUPERARLO
            Producto producto = this.repo.FindProducto(idproducto);
            //ALMACENAMOS EL PRODUCTO EN CACHE REDIS
            this.service.AddProductoFavorito(producto);
            ViewData["MENSAJE"] = "Producto almacenado en Favoritos";
            return RedirectToAction("Favoritos");


        }

        public IActionResult DeleteFavorito(int idproducto)
        {
            this.service.DeleteProductoFavorito(idproducto);
            return RedirectToAction("Favoritos");
        }



        public IActionResult Index()
        {
            List<Producto> products = this.repo.GetProductos();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            Producto producto = this.repo.FindProducto(id);
            return View(producto);
        }


    }
}
