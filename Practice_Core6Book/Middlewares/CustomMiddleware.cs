namespace Practice_Core6Book.Middlewares
{
    public class CustomMiddleware
    {
        RequestDelegate _next;
        public CustomMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("app.Use start\r\n");
            await _next.Invoke(context);
            await context.Response.WriteAsync("\r\napp.Use end");
        }
    }

    public static class CustomMiddlewareExtensions
    {
        public static void UseCustom(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomMiddleware>();
        }
    }
}
