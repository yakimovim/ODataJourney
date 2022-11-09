using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.OData;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataJourney.Database;
using ODataJourney.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

    builder.Services
    .AddControllers()
    .AddOData(opts =>
    {
        opts.AddRouteComponents("api/v1/authors", GetAuthorsEdm());

        IEdmModel GetAuthorsEdm()
        {
            ODataConventionModelBuilder edmBuilder = new();

            edmBuilder.EnableLowerCamelCase();

            edmBuilder.EntitySet<Author>("edm");
            edmBuilder.EntitySet<AuthorDto>("manual");
            edmBuilder.EntitySet<ComplexAuthor>("nonsql");

            edmBuilder.EntityType<Author>()
                .Property(a => a.HomePageUrl).Name = "url_home";
            edmBuilder.EntityType<ComplexAuthor>()
                .Property(a => a.FullName).Name = "name";

            return edmBuilder.GetEdmModel();
        }

        opts
            .Select()
            .Expand()
            .Filter()
            .Count()
            .OrderBy()
            .SetMaxTop(1000);
    })
    .AddJsonOptions(configure =>
    {
        configure.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        configure.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var inMemoryDatabaseConnection = new SqliteConnection("DataSource=:memory:");
inMemoryDatabaseConnection.Open();

builder.Services.AddDbContext<AuthorsContext>(optionsBuilder =>
    {
        optionsBuilder.UseSqlite(inMemoryDatabaseConnection);
    }
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<AuthorsContext>().Initialize();
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseODataRouteDebug();
}

app.UseAuthorization();

app.MapControllers();

app.Run();