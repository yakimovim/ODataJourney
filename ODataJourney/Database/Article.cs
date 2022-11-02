using System.ComponentModel.DataAnnotations;

namespace ODataJourney.Database;

public class Article
{
    [Key]
    public int Id { get; set; }

    public int AuthorId { get; set; }

    [Required]
    public string Title { get; set; }
}