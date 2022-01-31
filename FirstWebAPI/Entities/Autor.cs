using FirstWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebAPI.Entities
{
    public class Autor : IValidatableObject
    {
        public int ID { get; set; }
        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "El nombre debe tener {1} como mínimo.")]
        public string Nombre { get; set; }

        public List<Libro> Libros { get; set; }

        // [Required]
        // [Range(12, 100)]
        // public int Edad { get; set; }

        // [Url(ErrorMessage = "La URL ingresada no es válida.")]
        // [CreditCard(ErrorMessage = "El número de tarjeta no es válido.")]       


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Ésta implementación permite hacer varias validaciones, con lo que podemos devolver varios mensajes de error
            
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser con mayúscula.", new string[] { nameof(Nombre) });
                }
            }
        }

        // Primero se verifican las validaciones por atributo, y después se verifican las validaciones por modelo.
        // Si una validación por atributo da error, no se llega a las validaciones por modelo.

    }
}
