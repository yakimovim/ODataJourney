using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.OData.ModelBuilder;

namespace ODataJourney.Models;

public class ComplexAuthor
{
    [Key]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [NonFilterable]
    [NotFilterable]
    [NotSortable]
    [Unsortable]
    [JsonPropertyName("hash")]
    public string NameHash { get; set; }
}