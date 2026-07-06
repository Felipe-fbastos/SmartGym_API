namespace SmartGym.API.DTO.MemberTrainer
{
    public class MemberTrainerPostRequestDTO
    {
        public int Id { get; set; }
        public int IdMember { get; set; }
        public int IdTrainer { get; set; }
        public DateOnly AssignedAt { get; set; }
    }
}
