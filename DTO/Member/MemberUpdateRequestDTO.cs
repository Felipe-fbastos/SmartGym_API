namespace SmartGym.API.DTO.Member
{
    public class MemberUpdateRequestDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateOnly Birthday { get; set; }
        public DateOnly EnrollmentDate { get; set; }
    }
}
