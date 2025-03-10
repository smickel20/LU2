using LU2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectMap.WebApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LU2.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class Object2dController : ControllerBase
    {
        private readonly ILogger<Object2dController> _logger;
        private readonly Object2dRepository _object2dRepository;

        public Object2dController(ILogger<Object2dController> logger, Object2dRepository object2dRepository)
        {
            _logger = logger;
            _object2dRepository = object2dRepository;
        }

        // GET: /Object2d
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object2d>>> Get()
        {
            var objects = await _object2dRepository.ReadAsync();
            return Ok(objects);
        }

        // GET: /Object2d/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Object2d>> GetById(string id)
        {
            var object2d = await _object2dRepository.GetByIdAsync(id);
            if (object2d == null)
                return NotFound($"Object with ID {id} not found.");

            return Ok(object2d);
        }
        [HttpGet("total/{enviromentid}")]
        public async Task<ActionResult<IEnumerable<Object2d>>> GetByEnviroment(string enviromentid)
        {
            var objects = await _object2dRepository.GetByEnvironmentAsync(enviromentid);
            return Ok(objects);
        }

        // POST: /Object2d
        [HttpPost]
        public async Task<ActionResult<Object2d>> Create([FromBody] Object2d object2d)
        {
            if (object2d == null)
                return BadRequest("Invalid data.");

            var createdObject = await _object2dRepository.CreateAsync(object2d);
            return CreatedAtAction(nameof(GetById), new { id = createdObject.Id }, createdObject);
        }

        // PUT: /Object2d/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Object2d object2d)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format.");

            if (object2d == null || guidId != object2d.Id)
                return BadRequest("Invalid data.");

            var existingObject = await _object2dRepository.GetByIdAsync(id);
            if (existingObject == null)
                return NotFound($"Object with ID {id} not found.");

            await _object2dRepository.UpdateAsync(object2d);
            return NoContent();
        }


        // DELETE: /Object2d/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingObject = await _object2dRepository.GetByIdAsync(id);
            if (existingObject == null)
                return NotFound($"Object with ID {id} not found.");

            await _object2dRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
