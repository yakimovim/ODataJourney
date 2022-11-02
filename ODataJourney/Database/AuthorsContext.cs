using Bogus;
using Microsoft.EntityFrameworkCore;

namespace ODataJourney.Database;

public class AuthorsContext : DbContext
{
    public DbSet<Author> Authors { get; set; } = null!;

    public AuthorsContext(DbContextOptions<AuthorsContext> options)
        : base(options)
    { }
    
    public async Task Initialize()
    {
        await Database.EnsureDeletedAsync();
        await Database.EnsureCreatedAsync();

        var rnd = Random.Shared;

        Authors.AddRange(
            Enumerable
                .Range(0, 10)
                .Select(_ =>
                {
                    var faker = new Faker();

                    var person = faker.Person;

                    return new Author
                    {
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        ImageUrl = person.Avatar,
                        HomePageUrl = person.Website,
                        Articles = new List<Article>(
                            Enumerable
                                .Range(0, rnd.Next(1, 5))
                                .Select(_ => new Article
                                {
                                    Title = faker.Lorem.Slug(rnd.Next(3, 5))
                                })
                        )
                    };
                })
        );

        await SaveChangesAsync();
    }
}