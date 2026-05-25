namespace SmartGym.API.Execeptions
{
    public class BaseException : Exception
    {
        public int _statusCode { get; }
        public BaseException(string message, int statusCode)
            :base(message) 
        {
            _statusCode = statusCode;
        }
    }
}
