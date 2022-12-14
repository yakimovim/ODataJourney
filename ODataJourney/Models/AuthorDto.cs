using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ODataJourney.Models;

public class AuthorDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string? ImageUrl { get; set; }

    [JsonPropertyName("@url.home")]
    public string? HomePageUrl { get; set; }

    public ICollection<ArticleDto> Articles { get; set; }
}