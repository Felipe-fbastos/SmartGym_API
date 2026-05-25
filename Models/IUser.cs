namespace SmartGym.API.Models
{
    public interface IUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int IdRole { get; set; }
        public Roles Roles { get; set; }
    }
}
