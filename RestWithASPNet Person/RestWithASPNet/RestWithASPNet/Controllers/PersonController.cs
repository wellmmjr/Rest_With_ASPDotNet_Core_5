﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNet.Model;
using RestWithASPNet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        [HttpGet]
        public IActionResult FindAll()
        {
            return Ok(_personService.FindAll());
        }
        
        [HttpGet("{id}")]
        public IActionResult FindById(long id)
        {
            var person = _personService.FindById(id);

            if (person == null) return NotFound();

            return Ok(person);
        }
        
        [HttpPost()]
        public IActionResult CreatePerson([FromBody] Person person)
        {
            if (person == null) return NotFound();

            return Ok(_personService.Create(person));
        }
        
        [HttpPut()]
        public IActionResult UpdatePerson([FromBody] Person person)
        {
            if (person == null) return NotFound();

            return Ok(_personService.Update(person));
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeletePerson(long id)
        {
            _personService.Delete(id);
            return NoContent();
        }


    }
}
