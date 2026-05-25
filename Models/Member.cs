using System.Text.Json.Serialization;

namespace SmartGym.API.Models
{
    public class Member : IUser
    {
        public int Id { get; set; }
        public int IdRole { get; set; }
        public Roles? Roles { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly Birthday { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime DeleteAt { get; set; }
        public bool IsDelete { get; private set; }
        public List<MemberTrainer> MemberTrainers { get; set; } = new List<MemberTrainer>();
        

        public void Delete()
        {
            IsDelete = true;
        }
        
    }
}
