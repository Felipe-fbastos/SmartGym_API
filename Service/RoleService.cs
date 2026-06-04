using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartGym.API.Data;
using SmartGym.API.DTO.Roles;
using SmartGym.API.Execeptions;
using SmartGym.API.Models;

namespace SmartGym.API.Service
{
    public class RoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RoleGetResponseDTO> GetByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                throw new NotFoundException("Id not found");
            }

            return role.Adapt<RoleGetResponseDTO>();
        }

        public async Task<IEnumerable<RoleGetResponseDTO>> GetAllAsync()
        {
            var role = await _context.Roles
                .AsTracking()
                .ToListAsync();

            return role.Adapt<IEnumerable<RoleGetResponseDTO>>();
        }

        public async Task<RoleGetResponseDTO> CreateAsync(RoleCreateRequestDTO dto)
        {
            bool existRole = await _context.Roles.AnyAsync(e => e.Name  == dto.Name);

            if (existRole)
            {
                throw new ConflictException("Name already register");
            }

            var role = dto.Adapt<Roles>();

            await _context.AddAsync(role);

            await _context.SaveChangesAsync();

            return role.Adapt<RoleGetResponseDTO>();
        }

        
    }
}
