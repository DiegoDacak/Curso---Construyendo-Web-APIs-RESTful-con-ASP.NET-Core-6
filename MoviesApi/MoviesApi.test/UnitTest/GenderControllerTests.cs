using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesApi.Controllers;
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
    
}