namespace SmartGym.API.Execeptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message)
            :base(message, 404)
        {
            
        }
    }
}
