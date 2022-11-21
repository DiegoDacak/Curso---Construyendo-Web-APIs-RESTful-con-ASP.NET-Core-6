using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MoviesApi.Common.Strings;
using MoviesApi.Controllers;
using MoviesApi.DTOs.Actor;
using MoviesApi.DTOs.Pagination;
using MoviesApi.Entities;
using MoviesApi.Services.ServicesInterface;

namespace MoviesApi.test.UnitTest;

[TestClass]
public class ActorControllerTests : TestsBase
{
    [TestMethod]
    public async Task GetPaginatedActors()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        context.Actors.Add(new Actor {Name = "Actor 1"});
        context.Actors.Add(new Actor {Name = "Actor 2"});
        context.Actors.Add(new Actor {Name = "Actor 3"});

        var context2 = BuildContext(dbName);

        var controller = new ActorController(context2, mapper, null)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        
        var page1 = await controller
            .Get(new PaginationDto { Page = 1, RegisterQuantityPerPage = 2}, token);
        var actorsPage1 = page1.Value;
        Assert.AreEqual(2, actorsPage1?.Count);
        
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        
        var page2 = await controller
            .Get(new PaginationDto { Page = 2, RegisterQuantityPerPage = 2}, token);
        var actorsPage2 = page2.Value;
        Assert.AreEqual(1, actorsPage2?.Count);
        
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        
        var page3 = await controller
            .Get(new PaginationDto { Page = 3, RegisterQuantityPerPage = 2}, token);
        var actorsPage3 = page3.Value;
        Assert.AreEqual(0, actorsPage3?.Count);
    }

    [TestMethod]
    public async Task CreateActorWithoutPhoto()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        var actor = new CreateActorDto { Name = "Diego", BirthDate = DateTime.Now};

        var mock = new Mock<IFileStorage>();
        mock.Setup(x => x.SaveFile(null, null, null, null))
            .Returns(Task.FromResult("url"));

        var controller = new ActorController(context, mapper, mock.Object);
        var response = await controller.Post(actor, token);
        var result = response as CreatedAtRouteResult;
        Assert.AreEqual(201, result?.StatusCode);

        var context2 = BuildContext(dbName);
        var list = await context2.Actors.ToListAsync(token);
        Assert.AreEqual(1, list.Count);
        Assert.IsNull(list[0].Photo);
        
        Assert.AreEqual(0, mock.Invocations.Count);
    }

    [TestMethod]
    public async Task CreateActorWithPhoto()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        var content = Encoding.UTF8.GetBytes("test image");
        var file = new FormFile(new MemoryStream(content), 0, content.Length, "Data", "image.jph")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpg"
        };
        var actor = new CreateActorDto
        {
            Name = "Diego",
            BirthDate = DateTime.Now,
            Photo = file
        };

        var mock = new Mock<IFileStorage>();
        mock.Setup(x => x.SaveFile(content, ".jpg", Container.ActorContainer, file.ContentType))
            .Returns(Task.FromResult("url"));

        var controller = new ActorController(context, mapper, mock.Object);
        var response = await controller.Post(actor, token);
        var result = response as CreatedAtRouteResult;
        Assert.AreEqual(201, result?.StatusCode);

        var context2 = BuildContext(dbName);
        var list = await context2.Actors.ToListAsync(token);
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual("url", list[0].Photo);
        
        Assert.AreEqual(1, mock.Invocations.Count);
    }

    [TestMethod]
    public async Task PatchReturn404IsActorNotExit()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        var controller = new ActorController(context, mapper, null);
        var patchDoc = new JsonPatchDocument<ActorPatchDto>();
        var response = await controller.Patch(1, patchDoc, token);
        var result = response as StatusCodeResult;
        Assert.AreEqual(404, result?.StatusCode);
    }

    [TestMethod]
    public async Task PatchUpdateOneField()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        var birthDate = DateTime.Now;
        var actor = new Actor { Name = "Diego", BirthDate = birthDate};
        context.Add(actor);
        await context.SaveChangesAsync(token);

        var context2 = BuildContext(dbName);
        var controller = new ActorController(context2, mapper, null);

        var objectValidator = new Mock<ObjectModelValidator>();
        objectValidator.Setup(x => x.Validate(
            It.IsAny<ActionContext>(),
            It.IsAny<ValidationStateDictionary>(),
            It.IsAny<string>(),
            It.IsAny<object>()));
        controller.ObjectValidator = objectValidator.Object;
        var patchDoc = new JsonPatchDocument<ActorPatchDto>();
        patchDoc.Operations.Add(new Operation<ActorPatchDto>("replace", "/name", null, "Jorge"));
        var response = await controller.Patch(1, patchDoc, token);
        var result = response as StatusCodeResult;
        Assert.AreEqual(204, result?.StatusCode);

        var context3 = BuildContext(dbName);
        var actorDb = await context3.Actors.FirstAsync(token);
        Assert.AreEqual("Jorge", actorDb.Name);
        Assert.AreEqual(birthDate, actorDb.BirthDate);
    }
}