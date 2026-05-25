using SmartGym.API.Models;

namespace SmartGym.API.DTO.Member
{
    public class MemberPostRequestDTO
    {
        public int IdRole { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly Birthday { get; set; }
        public DateOnly EnrollmentDate { get; set; }
      
    }
}
