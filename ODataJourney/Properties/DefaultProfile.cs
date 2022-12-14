using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ODataJourney.Database;
using ODataJourney.Models;

namespace ODataJourney.Properties;

public class DefaultProfile : Profile
{
    public DefaultProfile()
    {
        CreateMap<Article, ArticleDto>();
        CreateMap<Author, AuthorDto>()
            .ForMember(a => a.Articles, o => o.ExplicitExpansion());

        CreateMap<Author, ComplexAuthor>()
            .ForMember(a => a.Articles, o => o.ExplicitExpansion())
            .ForMember(d => d.FullName,
                opt => opt.MapFrom(s => s.FirstName + " " + s.LastName))
            .ForMember(
                d => d.NameHash,
                opt => opt.MapFrom(a => string.Join(",", SHA256.HashData(Encoding.UTF32.GetBytes(a.FirstName + " " + a.LastName))))
            );
    }
}