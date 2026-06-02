using Mapster;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using SmartGym.API.Data;
using SmartGym.API.DTO.MemberTrainer;
using SmartGym.API.Execeptions;
using SmartGym.API.Models;

namespace SmartGym.API.Service
{
    public class MemberTrainerService
    {
        private readonly AppDbContext _context;


        public MemberTrainerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MemberTrainerGetResponseDTO> GetByIdAsync(int id)
        {
            var memberTrainer = await _context.MemberTrainers.FindAsync(id);

            if (memberTrainer == null)
            {
                throw new NotFoundException("MemberTrainer not found");
            }

            if (!memberTrainer.IsActive)
            {
                throw new BadRequestException("MemberTrainer is not active");
            }

            return memberTrainer.Adapt<MemberTrainerGetResponseDTO>();
        }

        public async Task<IEnumerable<MemberTrainerGetResponseDTO>> GetAllAsync()
        {
            var memberTrainers = await _context.MemberTrainers
                .AsNoTracking()
                .Where(mt => mt.IsActive)
                .ToListAsync();

            return memberTrainers.Adapt<IEnumerable<MemberTrainerGetResponseDTO>>();
        }

        public async Task<MemberTrainerGetResponseDTO> CreateAsync(MemberTrainerPostRequestDTO dto)
        {
            var member = await _context.Member.FindAsync(dto.IdMember);

            if (member == null)
            {
                throw new NotFoundException("Member not found");
            }

            var trainer = await _context.Employee.FindAsync(dto.IdTrainer);

            if (trainer == null)
            {
                throw new NotFoundException("Trainer not found");
            }

            var exist = await _context.MemberTrainers
                .AnyAsync(mt => mt.IdMember == dto.IdMember 
                           && mt.IdTrainer == dto.IdTrainer && mt.IsActive);

            if (exist)
            {
                throw new BadRequestException("This member already has this trainer");
            }

            var memberTrainer = dto.Adapt<MemberTrainer>();

            await _context.AddAsync(memberTrainer);

            await _context.SaveChangesAsync();

            return memberTrainer.Adapt<MemberTrainerGetResponseDTO>();
        }

        public async Task DeactivateAsync(int id)
        {
            var memberTrainer = await _context.MemberTrainers.FindAsync(id);

            if (memberTrainer == null)
            {
                throw new NotFoundException("Member Trainer not found");
            }
            if (memberTrainer.IsActive) 
            { 
                throw new NotFoundException("Member Trainer is already inactive");
            }

            memberTrainer.Deactivate();

            await _context.SaveChangesAsync();
        }

    }
}
