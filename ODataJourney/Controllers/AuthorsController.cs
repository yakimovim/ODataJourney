using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.OData;
using ODataJourney.Database;
using ODataJourney.Models;

namespace ODataJourney.Controllers
{
    [ApiController]
    [Route("/api/v1/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AuthorsContext _db;

        public AuthorsController(
            IMapper mapper,
            AuthorsContext db
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet("links")]
        public ActionResult GetAllLinks()
        {
            return Content(
                @"<html>
    <head>
        <title>Links</title>
    </head>
    <body>
        <ul>
            <li>
                <a href=""no-odata"">No Odata</a>
            </li>
            <li>
                <a href=""odata"">Odata</a>
            </li>
            <li>
                <a href=""edm"">Edm</a>
            </li>
            <li>
                <a href=""mapping"">Mapping</a>
            </li>
            <li>
                <a href=""manual"">Manual</a>
            </li>
            <li>
                <a href=""nonsql"">Non SQL</a>
            </li>
            <li>
                <a href=""add"">Apply additional data</a>
            </li>
        </ul>
    </body>
</html>
",
                "text/html");
        }

        [HttpGet("no-odata")]
        public ActionResult GetWithoutOData()
        {
            return Ok(_db.Authors);
        }

        [HttpGet("odata")]
        [EnableQuery]
        public IQueryable<Author> GetWithOData()
        {
            return _db.Authors;
        }

        [HttpGet("edm")]
        [ODataAttributeRouting]
        [EnableQuery]
        public IQueryable<Author> GetWithEdm()
        {
            return _db.Authors;
        }

        [HttpGet("mapping")]
        [ODataAttributeRouting]
        [EnableQuery]
        public IQueryable<AuthorDto> GetWithMapping()
        {
            return _db.Authors.ProjectTo<AuthorDto>(_mapper.ConfigurationProvider);
        }

        [HttpGet("manual")]
        [ODataAttributeRouting] // We need EDM for correct JSON serialization.
        public IActionResult GetManually(ODataQueryOptions<AuthorDto> options)
        {
            var query = _db.Authors.ProjectTo<AuthorDto>(_mapper.ConfigurationProvider);

            var result = options.ApplyTo(query);

            return Ok(result);
        }

        [HttpGet("nonsql")]
        public IActionResult GetNonSqlConvertible(ODataQueryOptions<ComplexAuthor> options)
        {
            try
            {
                options.Validator.Validate(options, new ODataValidationSettings());
            }
            catch (ODataException e)
            {
                return BadRequest(e.Message);
            }

            var query = _db.Authors.ProjectTo<ComplexAuthor>(_mapper.ConfigurationProvider);

            var result = options.ApplyTo(query);

            return Ok(result);
        }

        [HttpGet("add")]
        public IActionResult ApplyAdditionalData(ODataQueryOptions<ComplexAuthor> options)
        {
            try
            {
                options.Validator.Validate(options, new ODataValidationSettings());
            }
            catch (ODataException e)
            {
                return BadRequest(e.Message);
            }

            var query = _db.Authors.ProjectTo<ComplexAuthor>(_mapper.ConfigurationProvider);

            var authors = options
                .ApplyTo(query, AllowedQueryOptions.Select)
                .Cast<ComplexAuthor>()
                .ToArray();

            foreach (var author in authors)
            {
                author.LastName += " (Mr)";
            }

            var result = options.ApplyTo(authors.AsQueryable(), AllowedQueryOptions.All & ~AllowedQueryOptions.Select);

            return Ok(result);
        }
    }
}