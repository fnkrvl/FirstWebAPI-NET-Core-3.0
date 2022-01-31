using FirstWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebAPI.Context
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        // Propiedades
        public DbSet<Autor> Autores { get; set; } 
        public DbSet<Libro> Libros { get; set; }  // Permite hacer queries directamente hacia la tabla de libros

        // Dbset representa una colección de todas las instancias en el contexto, 
        // o que puede ser consultado desde la base de datos

        // Una vez creado el DbSet de cada clase, hago  el Add-Migration
        // Limpiar solución, y compilar (no sé por qué se hace esto, no debería ser necesario (I think))
        // Update-Database

        // *****************************************************************************************
        // Instalar previamente el paquete Install-Package Microsoft.EntityFrameworkCore.Tools desde
        // la Consola de Administración de Paquetes
        // ***************************************************************************************** 
        
    }
}
