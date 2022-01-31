using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebAPI.Entities
{
    public class Libro : IValidatableObject
    {

        public int ID { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "El ID de Autor debe ser de {1} caractéres o menos.")]
        public int AutorID { get; set; }
        public Autor Autor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Ésta implementación permite hacer varias validaciones, con lo que podemos devolver varios mensajes de error

            if (!string.IsNullOrEmpty(Titulo))  // Valido que el título no sea null
            {
                var primeraLetra = Titulo[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser con mayúscula.");
                    // Cuando se utiliza la palabra clave yield en una sentencia, indicamos que el método, operador o GET en el que aparece, es un iterador.
                    // Iterador: devuelve cada elemento de uno en uno.
                }
            }
        }
    }
}
