using Practice_Core6Book.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//app.Map("/map1", Map1);
//app.Map("/map2", Map2);



// Use
//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("app.Use start\r\n");
//    await next.Invoke();
//    await context.Response.WriteAsync("\r\napp.Use end");
//});

// Use 整進程式檔
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