using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartGym.API.Data;
using SmartGym.API.DTO.Employee;
using SmartGym.API.Execeptions;
using SmartGym.API.Models;

namespace SmartGym.API.Service
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordHasher<string> _passwordHasher;

        public EmployeeService(AppDbContext context, TokenService tokenService, PasswordHasher<string> passwordHasher)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<EmployeeGetResponseDTO> GetByIdAsync(int id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null || employee.IsDelete)
            {
                throw new NotFoundException("Employee not found");
            }

            return employee.Adapt<EmployeeGetResponseDTO>();
        }

        public async Task<EmployeeGetResponseDTO> GetMeAsync(int id)
        {
            var employee = await _context.Employee.FirstOrDefaultAsync(e  => e.Id == id);

            if(employee == null || employee.IsDelete)
            {
                throw new NotFoundException("Id not found");
            }

            return employee.Adapt<EmployeeGetResponseDTO>();
        }

        public async Task<IEnumerable<EmployeeGetResponseDTO>> GetAllAsync()
        {
            var employees = await _context.Employee.ToListAsync();

            if (!employees.Any())
            {
                throw new NotFoundException("Employee not found");
            }

            return employees.Adapt<IEnumerable<EmployeeGetResponseDTO>>();
        }

       public async Task<EmployeeGetResponseDTO> CreateAsync(EmployeePostRequestDTO dto)
       {
            
            bool existEmail = await _context.Employee.AnyAsync(e => e.Email == dto.Email);

            if (existEmail)
            {
                throw new ConflictException("Email or password incorret");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - dto.Birthday.Year;

            if(dto.Birthday > today.AddYears(-age))
            {
                --age;
            }
            if(age <= 18)
            {
                throw new BadRequestException("Clients under 18 are not allowed");
            }

            var member = dto.Adapt<Employee>();

            member.Password = _passwordHasher.HashPassword(null, dto.Password);

            member.CreatedAt = DateTime.Now;
            member.UpdateAt = DateTime.Now;

            _context.Add(member);

            await _context.SaveChangesAsync();

            return member.Adapt<EmployeeGetResponseDTO>();

        }

        public async Task<string> LoginAsync(EmployeeLoginRequestDTO dto)
        {
            var employee = await _context.Employee
                .AsNoTracking()
                .Include(r => r.Roles)
                .FirstOrDefaultAsync(e => e.Email == dto.Email);

            if(employee == null || employee.IsDelete)
            {
                throw new UnAuthorizedException("Email or password invalid");
            }

            var result = _passwordHasher.VerifyHashedPassword(null, employee.Password, dto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Email or password invalid");
            }

            return _tokenService.GenerateToken(employee);
        }

        public async Task UpdateAsync(int id, EmployeeUpdateRequestDTO dto)
        {
            var employee = await _context.Employee.FindAsync(id);

            if(employee == null || employee.IsDelete)
            {
                throw new NotFoundException("Id not not found");
            }

            dto.Adapt(employee);

            employee.UpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateMeAsync(int id, EmployeeUpdateRequestDTO dto)
        {
            var employee = await _context.Employee.FirstOrDefaultAsync(e => e.Id == id);

            if(employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            dto.Adapt(employee);

            employee.UpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = _context.Employee.FirstOrDefault(e => e.Id == id);

            if (employee == null || employee.IsDelete)
            {
                throw new NotFoundException("Employee not found or deleted");
            }

            employee.Delete();

            await _context.SaveChangesAsync();
        }
    }
}
