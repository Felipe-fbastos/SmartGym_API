namespace SmartGym.API.Models
{
    public class MemberTrainer
    {
        public int Id { get; set; }
        public int IdMember { get; set; }
        public Member? Member { get; set; }
        public int IdTrainer { get; set; }
        public Employee? Employee { get; set; }
        public DateOnly AssignedAt { get; set; }
        public bool IsActive { get; set; }
        

        public void Active()
        {
            IsActive = true;
        }
    }
}
