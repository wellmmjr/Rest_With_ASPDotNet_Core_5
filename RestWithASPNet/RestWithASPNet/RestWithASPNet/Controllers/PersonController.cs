using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNet.Business;
using RestWithASPNet.Data.VO;
using RestWithASPNet.Hypermedia.Filters;
using RestWithASPNet.Hypermedia.Utils;
using System.Collections.Generic;

namespace RestWithASPNet.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
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
        [ProducesResponseType(200, Type = typeof(List<PersonVO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FindAll()
        {
            return Ok(_personBusiness.FindAll());
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FindById(long id)
        {
            var person = _personBusiness.FindById(id);

            if (person == null) return NotFound();

            return Ok(person);
        }

        [HttpGet("findByName")]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FindById([FromQuery] string firstName, [FromQuery] string secondName)
        {
            var person = _personBusiness.FindByName(firstName, secondName);

            if (person == null) return NotFound();

            return Ok(person);
        }

        [HttpGet("{sortDirection}/{pageSize}/{currentPage}")]
        [ProducesResponseType(200, Type = typeof(PagedSearchVO<PersonVO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FindByPagedSearch([FromQuery] string name, string sortDirection, int pageSize, int currentPage)
        {
            return Ok(_personBusiness.FindWithPagedSearch(name, sortDirection, pageSize, currentPage));
        }

        [HttpPost()]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult CreatePerson([FromBody] PersonVO person)
        {
            if (person == null) return NotFound();

            return Ok(_personBusiness.Create(person));
        }
        
        [HttpPut()]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult UpdatePerson([FromBody] PersonVO person)
        {
            if (person == null) return NotFound();

            return Ok(_personBusiness.Update(person));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(List<PersonVO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult ActiveUser(long id)
        {
            return Ok(_personBusiness.ActiveUser(id));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult DeletePerson(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }


    }
}
