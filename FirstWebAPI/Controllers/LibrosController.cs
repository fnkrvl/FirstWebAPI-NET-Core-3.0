using FirstWebAPI.Context;
using FirstWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Libro>> Get()
        {
            return context.Libros.Include(x => x.Autor).ToList();
        }

        
        [HttpGet("{id}", Name = "ObtenerLibro")]  // Nombre de la ruta | Name = "ObtenerLibro"
        public ActionResult<Libro> Get(int id)
        {
            var libro = context.Libros.Include(x => x.Autor).FirstOrDefault(x => x.ID == id);

            if (libro == null)
            {
                return NotFound();
            }

            return libro;  // Devuelve el libro creado
        }


        [HttpPost]
        public ActionResult Post([FromBody] Autor libro)
        {
            context.Autores.Add(libro);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerLibro", new { id = libro.ID }, libro);   // 201 Created | Objeto creado
        } 


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor libro)
        {
            if (id != libro.ID)
            {
                return BadRequest();
            }

            context.Entry(libro).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();   // Código 200 OK
        }

        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var libro = context.Autores.FirstOrDefault(x => x.ID == id);

            if (libro == null)
            {
                return NotFound();
            }

            context.Autores.Remove(libro);
            context.SaveChanges();
            return libro; // Devuelve el libro que se eliminó
        }

    }
}
