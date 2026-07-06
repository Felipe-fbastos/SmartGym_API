using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using SmartGym.API.Data;
using SmartGym.API.DTO.GymClass;
using SmartGym.API.Execeptions;
using SmartGym.API.Models;

namespace SmartGym.API.Service
{
    public class GymClassService
    {
        private readonly AppDbContext _context;

        public GymClassService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GymClassGetResponseDTO> GetByIdAsync(int id)
        {
            var gymClass = await _context.GymClasse.FindAsync(id);

            if (gymClass == null)
            {
                throw new NotFoundException("Id not found");
            }

            return gymClass.Adapt<GymClassGetResponseDTO>();
        }

        public async Task<IEnumerable<GymClassGetResponseDTO>> GetAllAsync()
        {
            var gymClass = await _context.GymClasse.ToListAsync();

            return gymClass.Adapt<IEnumerable<GymClassGetResponseDTO>>();
        }

        public async Task<GymClassGetResponseDTO> CreateAsync(GymClassPostRequestDTO dto)
        {
            var existTrainer = await _context.Employee.FindAsync(dto.IdTrainer);

            if (existTrainer == null)
            {
                throw new NotFoundException("Trainer not found");
            }

            if (dto.EndTime < dto.StartTime)
            {
                throw new BadRequestException("End date cannot be before start date. ");
            }
            if (dto.StartTime < DateTime.Today)
            {
                throw new BadRequestException("Start date cannot be in the past.");
            }

            TimeSpan duration = dto.EndTime - dto.StartTime;

            if (duration.TotalHours > 24)
            {
                throw new BadRequestException("A class cannot last more than 1 day");
            }

            var gymClass = dto.Adapt<GymClass>();

            await _context.AddAsync(gymClass);

            await _context.SaveChangesAsync();

            return gymClass.Adapt<GymClassGetResponseDTO>();
        }

        public async Task UpdateAsync(int id,GymClassUpdateRequestDTO dto)
        {
            var gymclass = await _context.GymClasse.FindAsync(id);

            if(gymclass == null)
            {
                throw new NotFoundException("Class not found");
            }

            dto.Adapt(gymclass);

            await _context.SaveChangesAsync();

        }        
    }
}
