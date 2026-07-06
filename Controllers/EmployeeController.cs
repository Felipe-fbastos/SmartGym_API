using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartGym.API.DTO.Employee;
using SmartGym.API.Service;
using System.Collections;
using System.Security.Claims;

namespace SmartGym.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeGetResponseDTO>> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("me")]
        public async Task<ActionResult<EmployeeGetResponseDTO>> GetMe()
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(await _service.GetMeAsync(int.Parse(employeeId)));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeGetResponseDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeGetResponseDTO>> Create(EmployeePostRequestDTO dto)
        {
            var employee = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(EmployeeLoginRequestDTO dto)
        {
            return Ok(await _service.LoginAsync(dto));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, EmployeeUpdateRequestDTO dto)
        {
            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMe(EmployeeUpdateRequestDTO dto)
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _service.UpdateMeAsync(int.Parse(employeeId), dto);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
        
    }
}
