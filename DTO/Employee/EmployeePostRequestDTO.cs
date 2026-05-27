namespace SmartGym.API.DTO.Employee
{
    public class EmployeePostRequestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly Birthday { get; set; }
        public DateOnly HireDate { get; set; }
    }
}
