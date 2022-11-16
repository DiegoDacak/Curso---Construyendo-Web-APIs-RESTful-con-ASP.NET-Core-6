using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using WebAPIAutores.Controllers;
using WebAPIAutories.test.Mocks;

namespace WebAPIAutories.test.PruebasUnitarias
{
    public class RootControllerTest
    {
        [Test]
        public async Task SiUsuarioEsAdmin_Obtenemos4Links()
        {
            //Preparacion
            var authorizationService = new AuthorisationServicesMock();
            authorizationService.Resultado = AuthorizationResult.Success();
            var rootController = new RootController(authorizationService);
            rootController.Url = new UrlHelperMock();

            //Ejecucion
            var resultado = await rootController.Get();

            //Verificacion
            Assert.AreEqual(4, resultado.Value.Count());
        }
        
        [Test]
        public async Task SiUsuarioNoEsAdmin_Obtenemos2Links()
        {
            //Preparacion
            var authorizationService = new AuthorisationServicesMock();
            authorizationService.Resultado = AuthorizationResult.Failed();
            var rootController = new RootController(authorizationService);
            rootController.Url = new UrlHelperMock();

            //Ejecucion
            var resultado = await rootController.Get();

            //Verificacion
            Assert.AreEqual(2, resultado.Value.Count());
        }
        
        [Test]
        public async Task SiUsuarioNoEsAdmin_Obtenemos2Links_UsandoMoq()
        {
            //Preparacion
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));
            
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<string>()
            )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(String.Empty);
            
            var rootController = new RootController(mockAuthorizationService.Object);
            rootController.Url = mockUrlHelper.Object;
            
            //Ejecucion
            var resultado = await rootController.Get();

            //Verificacion
            Assert.AreEqual(2, resultado.Value.Count());
        }
    }
}