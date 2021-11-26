using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNet.Business;
using RestWithASPNet.Data.VO;
using RestWithASPNet.Hypermedia.Filters;
using RestWithASPNet.Model;
using System.Collections.Generic;

namespace RestWithASPNet.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("v{version:apiVersion}/api/book")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private IBookBusiness _bookBusiness;

        public BookController(ILogger<BookController> logger, IBookBusiness bookBusiness)
        {
            _logger = logger;
            _bookBusiness = bookBusiness;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<BookVO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FindAll()
        {
            return Ok(_bookBusiness.FindAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(BookVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FindById(long id)
        {
            var book = _bookBusiness.FindById(id);
            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpPost()]
        [ProducesResponseType(200, Type = typeof(BookVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult CreateBook([FromBody] BookVO book)
        {
            if (book == null) return NotFound();

            return Ok(_bookBusiness.Create(book));
        }
        
        [HttpPut()]
        [ProducesResponseType(200, Type = typeof(BookVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult UpdateBook([FromBody] BookVO book)
        {
            if (book == null) return NotFound();

            return Ok(_bookBusiness.Update(book));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(long id)
        {
            _bookBusiness.Delete(id);
            return NoContent();
        }
    }
}
