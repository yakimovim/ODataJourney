using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.OData.ModelBuilder;

namespace ODataJourney.Models;

public class ComplexAuthor
{
    [Key]
    public int Id { get; set; }

    // [JsonPropertyName("name")]
    public string FullName { get; set; }

    [NonFilterable]
    [Unsortable]
    public string NameHash { get; set; }
}