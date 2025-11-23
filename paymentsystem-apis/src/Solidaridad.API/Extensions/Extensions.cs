using Solidaridad.API.Middleware;

namespace Solidaridad.API.Extensions
{
    public static class ExtensionMethods
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerBasicAuthMiddleware>();
        }
    }
}
