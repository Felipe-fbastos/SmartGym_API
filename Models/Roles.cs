namespace SmartGym.API.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Member> Members { get; set; } = new List<Member>();
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
