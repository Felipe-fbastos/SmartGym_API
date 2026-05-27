namespace SmartGym.API.DTO.Employee
{
    public class EmployeeUpdateRequestDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}
