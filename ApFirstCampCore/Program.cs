using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IUnitOfWork,UnitOfWork>();

builder.Services.AddDbContext<ApplicationDbContext>(
    options=> {
        options.UseSqlServer(
                builder.Configuration.GetConnectionString("Default")
            ) ;

    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
