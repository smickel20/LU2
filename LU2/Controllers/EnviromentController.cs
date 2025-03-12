using LU2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectMap.WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LU2.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class EnvironmentController : ControllerBase
    {
//test
        private readonly ILogger<EnvironmentController> _logger;
        private readonly EnviromentRepository _enviromentRepository;

        public EnvironmentController(ILogger<EnvironmentController> logger, EnviromentRepository enviromentRepository)
        {
            _logger = logger;
            _enviromentRepository = enviromentRepository;
        }

        // GET: /Environment
        [HttpGet("total/{email}")]
        public async Task<ActionResult<Environment2D>> GetAllEnviroments(string email)
        {
            var enviroments = await _enviromentRepository.GetAllEnvironments(email);
            return Ok(enviroments);
        }

        // GET: /Environment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Environment2D>> GetById(string id)
        {
            var environment = await _enviromentRepository.GetByIdAsync(id);
            if (environment == null)
                return NotFound($"Environment with ID {id} not found.");

            return Ok(environment);
        }

        // POST: /Environment
        [HttpPost("{email}")]
        public async Task<ActionResult<Environment2D>> Create([FromBody] Environment2D environment, string email)
        {
            if (environment == null)
                return BadRequest("Invalid data.");

            var createdEnvironment = await _enviromentRepository.CreateAsync(environment, email);
            return CreatedAtAction(nameof(GetById), new { id = createdEnvironment.Id }, createdEnvironment);
        }
        
        // PUT: /Environment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Environment2D environment)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format.");

            if (environment == null || guidId != environment.Id)
                return BadRequest("Invalid data.");

            var existingEnvironment = await _enviromentRepository.GetByIdAsync(id);
            if (existingEnvironment == null)
                return NotFound($"Environment with ID {id} not found.");

            await _enviromentRepository.UpdateAsync(environment);
            return NoContent();
        }

        // DELETE: /Environment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingEnvironment = await _enviromentRepository.GetByIdAsync(id);
            if (existingEnvironment == null)
                return NotFound($"Environment with ID {id} not found.");

            await _enviromentRepository.DeleteAsync(id);

            return Ok(new { message = $"Environment with ID {id} has been successfully deleted." });
        }



    }
}
