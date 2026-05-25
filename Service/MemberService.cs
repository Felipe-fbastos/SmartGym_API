using Azure;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartGym.API.Data;
using SmartGym.API.DTO.Member;
using SmartGym.API.Execeptions;
using SmartGym.API.Models;

namespace SmartGym.API.Service
{
    public class MemberService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<string> _passwordHasher;
        private readonly TokenService _tokenService;

        public MemberService(AppDbContext context, PasswordHasher<string> passwordHasher, TokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<MemberGetResponseDTO> GetByIdAsync(int id)
        {
            var member = await _context.Member.FindAsync(id);

            if (member == null)
            {
                throw new NotFoundException("Member not found");
            }

            return member.Adapt<MemberGetResponseDTO>();
                       
        }

        public async Task<IEnumerable<MemberGetResponseDTO>> GetAllAsync()
        {
            var members = await _context.Member.ToListAsync();

            if (!members.Any())
            {
                throw new NotFoundException("Members not found");
            }

            return members.Adapt<List<MemberGetResponseDTO>>();
        }

        public async Task<MemberGetResponseDTO> CreateAsync(MemberPostRequestDTO dto)
        {
            bool existEmail = await _context.Member.AnyAsync(m  => m.Email == dto.Email);

            if (existEmail) 
            {
                throw new ConflictException("Email already exist");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - dto.Birthday.Year;

            if(dto.Birthday > today.AddYears(-age))
            {
                age--;
            }
            if (age <= 6)
            {
                throw new BadRequestException("Clients under 6 are not allowed");
            }

            var member = dto.Adapt<Member>();

            member.Password = _passwordHasher.HashPassword(string.Empty, dto.Password);

            member.CreatedAt = DateTime.Now;
            member.UpdateAt = DateTime.Now;

            _context.Member.Add(member);

            _context.SaveChangesAsync();

            return member.Adapt<MemberGetResponseDTO>();
        }

        public async Task<string> LoginAsync(MemberLoginRequestDTO dto)
        {
            var member = await _context.Member
                .AsNoTracking()
                .Include(r => r.Roles)
                .FirstOrDefaultAsync(e => e.Email == dto.Email && !e.IsDelete);
            
            if(member == null)
            {
                throw new NotFoundException("Email or password invalid");
            }            

            var result = _passwordHasher.VerifyHashedPassword(string.Empty, member.Password, dto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Email or password invalid");
            }

            return _tokenService.GenerateToken(member);
        }

        public async Task<MemberGetResponseDTO> UpdateAsync(int id, MemberUpdateRequestDTO dto)
        {
            var member = await _context.Member.FindAsync(id);

            if(member == null || member.IsDelete)
            {
                throw new NotFoundException("Client not found or not exist");
            }

            dto.Adapt(member);

            member.UpdateAt = DateTime.Now; 

            await _context.SaveChangesAsync();

            return dto.Adapt<MemberGetResponseDTO>();

        }
        
        public async Task DeleteAsync(int id)
        {
            var member = await _context.Member.FindAsync(id);

            if(member == null || member.IsDelete)
            {
                throw new BadRequestException("Member not exist or delete");
            }           
            
            member.Delete(); 

            member.DeleteAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
