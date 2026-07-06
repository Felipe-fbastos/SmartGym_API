namespace SmartGym.API.DTO.GymClass
{
    public class GymClassGetResponseDTO
    {
        public int Id { get; set; }
        public int IdTrainer { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
    }
}
