using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNet.Business;
using RestWithASPNet.Data.VO;
using RestWithASPNet.Model;

namespace RestWithASPNet.Controllers
{
    [ApiVersion("1")]
    [ApiController]
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
        public IActionResult FindAll()
        {
            return Ok(_bookBusiness.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult FindById(long id)
        {
            var book = _bookBusiness.FindById(id);
            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpPost()]
        public IActionResult CreateBook([FromBody] BookVO book)
        {
            if (book == null) return NotFound();

            return Ok(_bookBusiness.Create(book));
        }
        
        [HttpPut()]
        public IActionResult UpdateBook([FromBody] BookVO book)
        {
            if (book == null) return NotFound();

            return Ok(_bookBusiness.Update(book));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _bookBusiness.Delete(id);
            return NoContent();
        }
    }
}
