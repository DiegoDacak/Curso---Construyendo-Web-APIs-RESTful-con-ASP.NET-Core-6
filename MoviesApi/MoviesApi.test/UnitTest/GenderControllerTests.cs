using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesApi.Controllers;
using MoviesApi.DTOs.Gender;
using MoviesApi.Entities;

namespace MoviesApi.test.UnitTest;

[TestClass]
public class GenderControllerTests : TestsBase
{
    [TestMethod]
    public async Task GetAllGender()
    {
        // Preparation
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        context.Genders.Add(new Gender() {Name = "Gender 1"});
        context.Genders.Add(new Gender() {Name = "Gender 2"});
        await context.SaveChangesAsync(token);

        var context2 = BuildContext(dbName);
        
        // Test
        var controller = new GenderController(context2, mapper);
        var result = await controller.Get(token);

        // Verify
        var gender = result.Value;
        Assert.AreEqual(2, gender?.Count);
    }

    [TestMethod]
    public async Task GedGenderByNonExistentId()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        var controller = new GenderController(context, mapper);
        var response = await controller.Get(1, token);

        var result = response.Result as StatusCodeResult;
        
        Assert.AreEqual(404, result?.StatusCode);
    }

    [TestMethod]
    public async Task GetGenderByExistingId()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        context.Genders.Add(new Gender() {Name = "Gender 1"});
        context.Genders.Add(new Gender() {Name = "Gender 2"});
        await context.SaveChangesAsync(token);

        var context2 = BuildContext(dbName);
        var controller = new GenderController(context2, mapper);

        var response = await controller.Get(1, token);
        var result = response.Value;
        Assert.AreEqual(1, result?.Id);
    }

    [TestMethod]
    public async Task CreateGender()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        var newGender = new CreateGenderDto { Name = "new gender"};
        var controller = new GenderController(context, mapper);

        var response = await controller.Post(newGender, token);
        var result = response as CreatedAtRouteResult;
        Assert.IsNotNull(result);

        var context2 = BuildContext(dbName);
        var quantity = await context2.Genders.CountAsync(token);
        Assert.AreEqual(1, quantity);
    }

    [TestMethod]
    public async Task UpdateGender()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();
        
        context.Genders.Add(new Gender() {Name = "Gender 1"});
        context.Genders.Add(new Gender() {Name = "Gender 2"});
        await context.SaveChangesAsync(token);

        var context2 = BuildContext(dbName);
        var controller = new GenderController(context2, mapper);
        var createGenderDto = new CreateGenderDto {Name = "New gender"};
        var response = await controller.Put(1, createGenderDto, token);
        var result = response as StatusCodeResult;
        Assert.AreEqual(200, result?.StatusCode);

        var context3 = BuildContext(dbName);
        var exist = await context3.Genders.AnyAsync(x => x.Name == "New gender", token);
        Assert.IsTrue(exist);
    }

    [TestMethod]
    public async Task TryToDeleteNonExistentGender()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();

        var controller = new GenderController(context, mapper);
        var response = await controller.Delete(1, token);
        var result = response as StatusCodeResult;
        Assert.AreEqual(404, result?.StatusCode);
    }

    [TestMethod]
    public async Task RemoveGender()
    {
        var token = new CancellationToken();
        var dbName = Guid.NewGuid().ToString();
        var context = BuildContext(dbName);
        var mapper = ConfigureAutoMapper();
        
        context.Genders.Add(new Gender() {Name = "Gender 1"});
        context.Genders.Add(new Gender() {Name = "Gender 2"});
        await context.SaveChangesAsync(token);

        var context2 = BuildContext(dbName);
        var controller = new GenderController(context2, mapper);

        var response = await controller.Delete(1, token);
        var result = response as StatusCodeResult;
        Assert.AreEqual(200, result?.StatusCode);
        
        response = await controller.Delete(2, token);
        result = response as StatusCodeResult;
        Assert.AreEqual(200, result?.StatusCode);

        var context3 = BuildContext(dbName);
        var exist = await context3.Genders.AnyAsync(token);
        Assert.IsFalse(exist);
    }
}