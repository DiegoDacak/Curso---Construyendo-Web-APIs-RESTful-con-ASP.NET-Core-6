using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using WebAPIAutores.Validations;

namespace WebAPIAutories.test.PruebasUnitarias
{
    public class PrimerLetraMayuscuaAttributeTest
    {
        [Test]
        public void PrimeraLetraMinuscula_DevuelveError()
        {
            // Preparacion
            var primeraLetraMayuscula = new PrimerLetraMayuscuaAttribute();
            var valor = "felipe";
            var valContext = new ValidationContext(new {Nombre = valor});
            
            // Ejecucion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);
            
            // Verificacion
            Assert.AreEqual("La primera letra debe ser mayuscula", resultado.ErrorMessage);
        }
        
        [Test]
        public void ValorNulo_NoDevuelveError()
        {
            // Preparacion
            var primeraLetraMayuscula = new PrimerLetraMayuscuaAttribute();
            string valor = null;
            var valContext = new ValidationContext(new {Nombre = valor});
            
            // Ejecucion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);
            
            // Verificacion
            Assert.IsNull(resultado);
        }
        
        [Test]
        public void ValorConPrimeraLetraMayuscula_NoDevuelveError()
        {
            // Preparacion
            var primeraLetraMayuscula = new PrimerLetraMayuscuaAttribute();
            var valor = "Felipe";
            var valContext = new ValidationContext(new {Nombre = valor});
            
            // Ejecucion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);
            
            // Verificacion
            Assert.IsNull(resultado);
        }
    }
}