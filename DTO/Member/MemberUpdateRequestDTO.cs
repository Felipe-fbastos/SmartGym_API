using System.Text.Json.Serialization;

namespace SmartGym.API.DTO.Member
{
    public class MemberUpdateRequestDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        
    }
}
