using Core.Domian;
using Core.Repository;
using Core.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IRepository<Currency>, Repository<Currency>>();
builder.Services.AddTransient<IRepository<ExchageHistory>, Repository<ExchageHistory>>();
builder.Services.AddTransient<ICurrencyServices, CurrencyServices> ();
builder.Services.AddTransient<ILoginServices, LoginServices>();

builder.Services.AddTransient<IExchangHistoryServices, ExchangeHistoryServices>();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CurrencyDb")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
               {
                   options.SlidingExpiration = true;
                   options.ExpireTimeSpan = new TimeSpan(0, 1, 0);
               });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
