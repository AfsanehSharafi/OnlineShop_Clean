using Microsoft.EntityFrameworkCore;
using Persistence.Contenxts;
using Infrastructure.IdentityConfigs;
using Application.Interfaces.Contexts;
using Persistence.Contenxts.MongoContext;
using Application.Visitors.SaveVisitorInfo;
using Web.Endpoint.Utilities.Filters;
using Web.Endpoint.Hubs;
using Rebus.Config;
using Application.Visitors.VisitorOnline;
using WebSite.EndPoint.Utilities.Middlewares;
using Application.Catalogs.GetMenuItem;
using Infrastructure.MappingProfile;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();



#region ConnectinString

builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
var connnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options =>
options.UseSqlServer(connnection));

builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    option.LoginPath = "/account/login";
    option.AccessDeniedPath = "/Account/AccessDenied";
    option.SlidingExpiration = true;
});
builder.Services.AddDbContext<IdentityDatabaseContext>(options => options.UseSqlServer(connnection));
#endregion

builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
builder.Services.AddTransient<ISaveVisitorInfoService, SaveVisitorInfoService>();
builder.Services.AddTransient<IIVisitorOnlineService, VisitorOnlineService>();
builder.Services.AddTransient<IGetMenuItemService, GetMenuItemService>();
builder.Services.AddScoped<SaveVisitorFilter>();
builder.Services.AddSignalR();

builder.Services.AddAutoMapper(typeof(CatalogMappingProfile));

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSetVisitorId();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.UseRouting();

app.MapRazorPages();
app.MapHub<OnlineVisitorHub>("/chathub");

app.UseAuthorization();



app.Run();

