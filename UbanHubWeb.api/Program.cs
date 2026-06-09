using Microsoft.EntityFrameworkCore;
using UrbanHub.Data;
using UrbanHubManagement.repo;
using UrbanHub.shared;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UrbanHubDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UrbanHubDB")
        , x => x.UseNetTopologySuite()
    ));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(UrbanHub.shared.mapper));

// Register dependencies
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<UserCard>();
builder.Services.AddScoped<ParkinHome>();
builder.Services.AddScoped<AdminUserManagement>();
builder.Services.AddScoped<PlatformServices>();
builder.Services.AddScoped<ParkinViewDetails>();
builder.Services.AddAuthentication("UrbanAuth").AddCookie("UrbanAuth",
    opt =>
    {
        opt.AccessDeniedPath = "/Denied";
        opt.LoginPath = "/Login";
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(300);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
