using inotebookApi.Helpers.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using inotebookApi.Helpers.Responses;
using inotebookApi.DTOs;


namespace inotebookApi.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string? token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")[1];
            if (token == null)
            {
                // check incoming request
                if (IsEnabledUnauthorizedRoute(httpContext))
                {
                    return _next(httpContext);
                }
                BaseResponse response = new BaseResponse(StatusCodes.Status401Unauthorized, new MessageDTO("Unauthorized"));
                httpContext.Response.StatusCode = response.StatusCode;
                httpContext.Response.ContentType = "application/json";
                return httpContext.Response.WriteAsJsonAsync(response);
            }
            else
            {
                if (JwtUtils.ValidateJwtToken(token)){
                    return _next(httpContext);
                }
                else
                {
                    BaseResponse response = new BaseResponse(StatusCodes.Status401Unauthorized, new MessageDTO("Unauthorized"));
                    httpContext.Response.StatusCode = response.StatusCode;
                    httpContext.Response.ContentType = "application/json";
                    return httpContext.Response.WriteAsJsonAsync(response);
                }
            }
            //return _next(httpContext);
            //<summary>
            //</summary>
        }
        private bool IsEnabledUnauthorizedRoute(HttpContent httpContext)
        {
            bool IsEnabledUnauthorizedRoute = false;

            List<string> enabledRoutes = new List<string>
            {
                "/api/auth/CreateUser",
                "/api/auth/LoginUser" ,
                "/api/auth/GetUser"   ,
                "/api/notes/GetNotes",
                "/api/notes/CreateNotes",
                "/api/notes/UpdateNotes",
                "/api/notes/DeleteNote",
            };
            //if(httpContext.ReadAsStream.Path.Value is not null)
            //{

            //}
            return IsEnabledUnauthorizedRoute;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
