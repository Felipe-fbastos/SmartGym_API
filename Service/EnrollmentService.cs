using Microsoft.EntityFrameworkCore;
using SmartGym.API.Data;
using SmartGym.API.Execeptions;
using System.Runtime.InteropServices;

namespace SmartGym.API.Service
{
    public class EnrollmentService
    {
        private readonly AppDbContext _context;
        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task EnrollAsync(int memberId, int gymClassId)
        {
            var member = await _context.Member.FindAsync(memberId);

            if (member == null)
            {
                throw new NotFoundException("Member not found");
            }

            var gymclass = await _context.GymClasse.FindAsync(gymClassId);

            if (gymclass == null)
            {
                throw new NotFoundException("Class not found");
            }

            if(gymclass.Enrollments >= gymclass.Capacity)
            {
                throw new BadRequestException("This class is already full");
            }

            gymclass.Enrollments++;

            await _context.SaveChangesAsync();
        }
    }
}
