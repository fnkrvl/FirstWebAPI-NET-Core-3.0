using CsvHelper.Configuration.Attributes;
using FirstWebAPI.Context;
using FirstWebAPI.Entities;
using FirstWebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebAPI.Controllers
{

    [Route("api/[controller]")]  // Permite indicar la base del endpoint de las acciones del controlador
    [ApiController]  // Con esto no es necesario que se esté revisando el ModelState, porque cuando hay errores de validación (en las propiedades de la clase)
                     // automaticamente podemos retornar un 400 BAD REQUEST
                     // 
    // [Authorize] -> Si se lo pone a anivel del controlador, todas las acciones se ven restringidas al mismo tiempo.
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [ResponseCache(Duration = 15)] //-> Durante 15 segundos se va a seguir devolviendo la misma respuesta en ésta petición
        [Authorize] //-> Sólo personas que estén autenticadas en el Web API van a poder acceder al mismo
        // public ActionResult<string> Get()
        // {   
        //     return DateTime.Now.Second().ToString();
        // }

        [HttpGet("/listado")]
        [HttpGet("listado")]
        [HttpGet]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult<IEnumerable<Autor>> Get()
        {           
            return context.Autores.Include(x => x.Libros).ToList();
        }


        [HttpGet("/Primer")]                      
        [HttpGet("Primer")]
        // Para acceder a ésta acción, se añade a lo ultimo del endpoint el STRING entre comillas ya que ya existe una acción [HttpGet]
        // api/autores/Primer
        // NO pueden existir dos acciones con el mismo endpoint
        // Esto se llama regla de ruteo (ruteo por atributo)
        // Se pueden hacer ruteos en las acciones de las controladores (ruteo por atributo), o en la clase StartUp (ruteo convencional)
        // Es una forma de personalizar el endpoint
        // A esto se llama combinación, entre el endpoint base y el string de la regla de ruteo
        // Si se añade una / al principio del string, se omite la base del endpoint, y en el navegador sólo aparece el string
        // Ésta acción va a responder a ambos endpoints, tanto Primer como /Primer
        public ActionResult<Autor> GetPrimerAutor()
        {
            return context.Autores.FirstOrDefault();  // Devuelve el primer autor que encuentra
        }

         
        [HttpGet("{id}/{param2?}", Name = "ObtenerAutor")]
        // Nombre de la ruta | Name = "ObtenerAutor"
        // Si se define un parámetro para pasar en el endpoint, obligatoriamente se debe pasarlo a través de la URL
        // Si queremos que el segundo parámetro sea opcional, se agrega un ? a lo último
        // En el caso de si quiero que el segundo tenga un valor por defecto, hago una asignacion simple | param2="STRING"
        // Igualmente puedo definir un parámetro diferente a través del navegador y funciona igual
        public async Task<ActionResult<Autor>> Get(int id, [BindRequired]string param2)
        {
            var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.ID == id);
            // Ésta línea es la que busca el o los recursos externos a la aplicación y es la que podría tardar más, y por ello se hace la acción asíncrona

            if (autor ==null)
            {
                return NotFound();
            }

            return autor;
        }
        // Se usa Task para definir una acción asíncrona, es decir una acción que se ejecuta externa a nuestra aplicación, para que el servidor pueda 
        // seguir ejecutando otras tareas

        [HttpPost]                          
        public ActionResult Post([FromBody] Autor autor)
        {                      // FromBody -> Indica que la información del autor viene en el cuerpo de la petición HTTP 
            TryValidateModel(autor);  // Si se requiere volver a hacer las validaciones a un modelo (cumple con las validaciones de atributos) 
            context.Autores.Add(autor); // Add -> EF
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.ID }, autor); // 201 Created | Objeto creado
        }
        

        [HttpPut("{id}")]  
        public ActionResult Put(int id, [FromBody] Autor value)
        {
            if (id != value.ID) // Validación de ID porque se está pasando por parámetro una entidad                                
            {                   // Hago ésto para evitar que se actualice un recurso incorrecto
                return BadRequest();
            }

            context.Entry(value).State = EntityState.Modified;
            context.SaveChanges();
            return Ok(); // Código 200 OK - Valor modificado | No devuelve nada
        } 
        

        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var autor = context.Autores.FirstOrDefault(x => x.ID == id);

            if (autor == null)
            {
                return NotFound();  // Hereda de ActionResult
            }

            context.Autores.Remove(autor);
            context.SaveChanges();
            return autor; // Devuelve el autor que se eliminó
        }

        // Cada acción está determinada por un endpoint.
        // No pueden estar definidas dos acciones con exactamente el mismo [HttpXXX], sin diferenciarlas por una regla de ruteo
        // Cada acción puede devolver un ActionResult (codigos de status HTTP (1XX, 2XX, 3XX, 4XX, 5XX)), o el tipo de objeto que contega el ActionResult<TIPO<>>
        // La desventaja de usar IActionResult es que permite devolver cualquier tipo de dato a través de Ok(); | Ok(45) ó Ok("hola");
        // Si quiero devolver un dato que no sea un status http o una clase, por ejemplo, un archivo JSon, voy a usar IActionResult

    }
}
