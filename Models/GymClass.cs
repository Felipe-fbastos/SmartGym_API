namespace SmartGym.API.Models
{
    public class GymClass
    {
        public int Id { get; set; }
        public int IdTrainer { get; set; }
        public Employee? Employee { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        
    }
}
