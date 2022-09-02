using Microsoft.AspNetCore.Authentication.Cookies;
using Practice_Core6Book.Middlewares;
using Serilog;


Log.Logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    options.LoginPath = new PathString("/Home/Login");
    options.LogoutPath = new PathString("/Home/Logout");
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Host.UseSerilog();

var app = builder.Build();

app.Map("/map", mapApp =>
{
    mapApp.Run(async context =>
    {
        await context.Response.WriteAsync("map. \r\n");
    });
});
app.Map("/v1", v1 =>
{
    v1.Map("/map", mapApp =>
    {
        mapApp.Run(async context =>
        {
            await context.Response.WriteAsync("v1 map. \r\n");
        });
    });
});



// Use
//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("app.Use start\r\n");
//    await next.Invoke();
//    await context.Response.WriteAsync("\r\napp.Use end");
//});

// Use ��i�{����
//app.UseCustom();

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("app.Run");
//});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//static void Map1(IApplicationBuilder app)
//{
//    app.Run(async (context) =>
//    {
//        await context.Response.WriteAsync("Map 1");
//    });
//}

//static void Map2(IApplicationBuilder app)
//{
//    app.Run(async (context) =>
//    {
//        await context.Response.WriteAsync("Map 2");
//    });
//}