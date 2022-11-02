using System.ComponentModel.DataAnnotations;

namespace ODataJourney.Database;

public class Author
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string? ImageUrl { get; set; }

    public string? HomePageUrl { get; set; }

    public ICollection<Article> Articles { get; set; }
}