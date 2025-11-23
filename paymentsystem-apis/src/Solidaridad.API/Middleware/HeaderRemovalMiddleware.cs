namespace Solidaridad.API.Middleware
{
    public class HeaderRemovalMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderRemovalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(state =>
             {
                 var response = (HttpContext)state;
                 response.Response.Headers.Remove("Server");
                 response.Response.Headers.Remove("X-Powered-By");
                 return Task.CompletedTask;
             }, context);

            await _next(context);
        }
    }
}
