using SmartGym.API.Execeptions;

namespace SmartGym.API.Middleware
{
    public class ExceptionMiddlerware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddlerware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseException ex)
            {
                context.Response.StatusCode = ex._statusCode;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                await context.Response.WriteAsJsonAsync(new
                {
                    massage = "Internal server erros"
                });

            }

    }   }        
}
