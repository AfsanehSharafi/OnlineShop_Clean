using Application.Catalogs.CatalogTypes;
using Application.Interfaces.Contexts;
using Application.Visitors.GetTodayReport;
using Infrastructure.MappingProfile;
using Microsoft.EntityFrameworkCore;
using Persistence.Contenxts;
using Persistence.Contenxts.MongoContext;
using static Application.Catalogs.CatalogTypes.ICatalogTypeService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IGetTodayReportService, GetTodayReportService>();
builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));


#region connection String SqlServer
builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
var connnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options =>
options.UseSqlServer(connnection));
#endregion



//mapper
builder.Services.AddAutoMapper(typeof(CatalogMappingProfile));
builder.Services.AddTransient<ICatalogTypeService , CatalogTypeService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
