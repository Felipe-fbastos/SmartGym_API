using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartGym.API.DTO.Roles;
using SmartGym.API.Service;

namespace SmartGym.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _service;

        public RoleController(RoleService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RoleGetResponseDTO>> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleGetResponseDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<RoleGetResponseDTO>> Create(RoleCreateRequestDTO dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }
    }
}
