using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartGym.API.DTO.Member;
using SmartGym.API.DTO.Roles;
using SmartGym.API.Execeptions;
using SmartGym.API.Service;
using System.Security.Claims;
using System.Threading.Tasks.Sources;

namespace SmartGym.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MemberService _service;

        public MemberController(MemberService service)
        {
            _service = service;   
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberGetResponseDTO>> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberGetResponseDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("me")]
        public async Task<ActionResult<MemberGetResponseDTO>> GetMe()
        {
            var memberId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(await _service.GetMeAsync(int.Parse(memberId)));

        }

        [HttpPost("signup")]
        public async Task<ActionResult<MemberGetResponseDTO>> Create(MemberPostRequestDTO dto)
        {
            var member = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, member);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(MemberLoginRequestDTO dto)
        {
            return Ok(await _service.LoginAsync(dto));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAsync(MemberUpdateRequestDTO dto, int id)
        {
            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateMeAsync(MemberUpdateRequestDTO dto)
        {
            var memberId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _service.UpdateMeAsync(int.Parse(memberId), dto);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }

    }
}
