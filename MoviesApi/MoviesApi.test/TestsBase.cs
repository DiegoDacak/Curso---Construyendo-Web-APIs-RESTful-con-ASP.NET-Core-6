using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.Services.AutoMapper;
using NetTopologySuite;

namespace MoviesApi.test;

public class TestsBase
{
    protected static ApplicationDbContext BuildContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName).Options;

        var dbContext = new ApplicationDbContext(options);
        
        return dbContext;
    }

    protected IMapper ConfigureAutoMapper()
    {
        var config = new MapperConfiguration(opt =>
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            opt.AddProfile(new AutoMapperProfile(geometryFactory));
        });
        return config.CreateMapper();
    }
}