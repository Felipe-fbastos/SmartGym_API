namespace SmartGym.API.Execeptions
{
    public class UnAuthorizedException : BaseException
    {
        public UnAuthorizedException(string message) 
            : base(message, 401)
        {
            
        }
    }
}
