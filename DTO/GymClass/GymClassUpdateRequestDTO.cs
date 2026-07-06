namespace SmartGym.API.DTO.GymClass
{
    public class GymClassUpdateRequestDTO
    {
        public int IdTrainer { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
    }
}
