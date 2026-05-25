namespace SmartGym.API.Execeptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message) 
            : base(message, 409)
        {
            
        }
    }
}
