namespace SmartGym.API.Models
{
    public class MemberTrainer
    {
        public int Id { get; set; }
        public int IdMember { get; set; }
        public Member? Member { get; set; }
        public int IdTrainer { get; set; }
        public Employee? Employee { get; set; }
        public DateOnly AssignedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly DissolvedAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        public void Deactivate()
        {
            IsActive = false;
            DissolvedAt = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
