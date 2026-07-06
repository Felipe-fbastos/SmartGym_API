using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartGym.API.DTO.MemberTrainer;
using SmartGym.API.Service;

namespace SmartGym.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberTrainerController : ControllerBase
    {
        private readonly MemberTrainerService _service;

        public MemberTrainerController(MemberTrainerService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberTrainerGetResponseDTO>> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberTrainerGetResponseDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<MemberTrainerGetResponseDTO>> Create(MemberTrainerPostRequestDTO dto)
        {
            var memberTrainer = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, memberTrainer);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Deactivate(int id)
        {
            await _service.DeactivateAsync(id);

            return NoContent();
        }



    }
}
