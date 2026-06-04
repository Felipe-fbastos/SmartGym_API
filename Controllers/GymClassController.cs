using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartGym.API.DTO.GymClass;
using SmartGym.API.Service;
using System.Security.Claims;

namespace SmartGym.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymClassController : ControllerBase
    {
        private readonly GymClassService _service;
        private readonly EnrollmentService _enrollmentService;

        public GymClassController(GymClassService service, EnrollmentService enrollmentService)
        {
            _service = service;
            _enrollmentService = enrollmentService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GymClassGetResponseDTO>> GetById(int id)
        {
            return Ok (await _service.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GymClassGetResponseDTO>>> GetAll()
        {
            return Ok (await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<GymClassGetResponseDTO>> Create(GymClassPostRequestDTO dto)
        {
            var gymClass = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, gymClass);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, GymClassUpdateRequestDTO dto)
        {
            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Enrollement(GymClassEnrollmentPostDTO dto)
        {
            var memberId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (memberId == null)
            {
                return Unauthorized();
            }

            await _enrollmentService.EnrollAsync(dto,int.Parse(memberId));

            return NoContent();
        } 
    }
}
