using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNet.Business;
using RestWithASPNet.Data.VO;
using RestWithASPNet.Hypermedia.Filters;

namespace RestWithASPNet.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}/api/person")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private IPersonBusiness _personBusiness;

        public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        [HttpGet]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FindAll()
        {
            return Ok(_personBusiness.FindAll());
        }
        
        [HttpGet("{id}")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FindById(long id)
        {
            var person = _personBusiness.FindById(id);

            if (person == null) return NotFound();

            return Ok(person);
        }
        
        [HttpPost()]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult CreatePerson([FromBody] PersonVO person)
        {
            if (person == null) return NotFound();

            return Ok(_personBusiness.Create(person));
        }
        
        [HttpPut()]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult UpdatePerson([FromBody] PersonVO person)
        {
            if (person == null) return NotFound();

            return Ok(_personBusiness.Update(person));
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeletePerson(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }


    }
}
