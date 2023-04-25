using MvcCoreCacheRedisProductos.Helpers;
using MvcCoreCacheRedisProductos.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreCacheRedisProductos.Services
{
    public class ServiceCacheRedis
    {
        private IDatabase database;

        public ServiceCacheRedis()
        {
            this.database =
                HelperCacheMultiplexer.GetConnection.GetDatabase();
        }

        //TENDREMOS UN METODO PARA AÑADIR PRODUCTOS A FAVORITOS
        public void AddProductoFavorito(Producto producto)
        {
            //CACHE REDIS FUNCIONA CON CLAVES/VALUES PARA 
            //TRABAJAR CON CADA USUARIO
            //DICHA CLAVE ES PARA TODOS LOS USUARIOS
            //DEBERIAMOS TENER ALGUN METODO PARA SABER EL USUARIO
            //POR EJEMPLO, UTILIZANDO UN LOGIN
            //PARA ALMACENAR UN VALUE, SE UTILIZA STRING EN FORMATO
            //JSON
            //LO QUE ALMACENAREMOS SERA UNA COLECCION DE PRODUCTOS
            //RECUPERAMOS LOS PRODUCTOS SI YA EXISTEN
            string jsonProductos = this.database.StringGet("favoritos");
            List<Producto> productosList;
            if (jsonProductos == null)
            {
                //NO HEMOS ALMACENADO FAVORITOS, CREAMOS LA COLECCION
                productosList = new List<Producto>();
            }
            else
            {
                //CONVERTIMOS EL JSON DE PRODUCTOS A COLECCION List<>
                productosList =
                    JsonConvert.DeserializeObject<List<Producto>>(jsonProductos);
            }
            //AÑADIMOS EL NUEVO PRODUCTO A LA COLECCION
            productosList.Add(producto);
            //SERIALIZAMOS LA COLECCION A JSON PARA ALMACENARLA EN CACHE
            jsonProductos =
                JsonConvert.SerializeObject(productosList);
            this.database.StringSet("favoritos", jsonProductos);
        }

        //METODO PARA RECUPERAR TODOS LOS FAVORITOS
        public List<Producto> GetProductosFavoritos()
        {
            string jsonProductos = this.database.StringGet("favoritos");
            if (jsonProductos == null)
            {
                return null;
            }
            else
            {
                List<Producto> favoritos =
                    JsonConvert.DeserializeObject<List<Producto>>(jsonProductos);
                return favoritos;
            }
        }

        //METODO PARA ELIMINAR UN PRODUCTO FAVORITO
        public void DeleteProductoFavorito(int idproducto)
        {
            List<Producto> favoritos = this.GetProductosFavoritos();
            if (favoritos != null)
            {
                //BUSCAMOS EL PRODUCTO A ELIMINAR
                Producto productoDelete =
                    favoritos.FirstOrDefault(z => z.IdProducto == idproducto);
                favoritos.Remove(productoDelete);
                //SI NO TENEMOS FAVORITOS, ELIMINAMOS LA CLAVE DE CACHE REDIS
                if (favoritos.Count == 0)
                {
                    this.database.KeyDelete("favoritos");
                }
                else
                {
                    string jsonProductos =
                        JsonConvert.SerializeObject(favoritos);
                    //SI NO PONEMOS TIEMPO EN EL MOMENTO DE ALMACENAR
                    //ELEMENTOS DENTRO DE CACHE REDIS, EL TIEMPO DE 
                    //ALMACENAMIENTO ES DE 24 HORAS
                    //TAMBIEN PODEMOS INDICARLO DENTRO DEL PORTAL DE AZURE
                    //PODEMOS INDICAR DE FORMA EXPLICITA EL TIEMPO
                    //QUE DESEAMOS ALMACENAR LOS DATOS HASTA QUE SON 
                    //ELIMINADOS DE FORMA AUTOMATICA
                    this.database.StringSet("favoritos", jsonProductos
                        , TimeSpan.FromMinutes(30));
                }
            }
        }

    }
}
