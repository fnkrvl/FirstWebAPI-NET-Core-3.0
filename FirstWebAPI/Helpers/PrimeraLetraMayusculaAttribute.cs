using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebAPI.Helpers
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null | string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var firstLetter = value.ToString()[0].ToString();  // [0] -> En la primer posición está el id. 

            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser con mayúscula.");
                    // Nueva instancia de error de validación, por eso va el new, creando un mensaje de error.
            }

            return ValidationResult.Success;
        }

        // value trae el valor de la propiedad donde se ha colocado el atributo
        // validationContext trae datos acerca del contexto donde se está ejecutando la validación

    }
}
 