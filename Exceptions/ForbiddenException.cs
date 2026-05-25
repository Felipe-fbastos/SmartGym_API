namespace SmartGym.API.Execeptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message)
            :base(message, 403)
        {
            
        }
    }
}
