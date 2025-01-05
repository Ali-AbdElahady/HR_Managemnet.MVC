using HR_Managemnet.PL.ErrorsHandle;
using System.Net;
using System.Text.Json;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleWare> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleWare(RequestDelegate Next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
        {
            next = Next;
            this.logger = logger;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);

            }catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                //if(env.IsDevelopment())
                //{
                //    var Response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message,ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                //}
                var Response = env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var JsonResponse = JsonSerializer.Serialize(Response);
                context.Response.WriteAsync(JsonResponse);
            }
        }
    }
}
