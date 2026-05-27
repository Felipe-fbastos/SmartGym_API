namespace SmartGym.API.Models
{
    public class Employee : IUser
    {
        public int Id { get; set; }
        public int IdRole { get; set; }
        public Roles? Roles { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal Salary { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly Birthday { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; private set; }
        public List<MemberTrainer> MemberTrainers { get; set; } = new List<MemberTrainer>();
        public List<GymClass> GymClasses { get; set; } = new List<GymClass>();

        public void Delete()
        {
            IsDelete = true;
        }
    }
}
