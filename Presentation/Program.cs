
using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AccountDbConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(x => {

    x.User.RequireUniqueEmail = true;
    x.Password.RequiredLength = 8;

}).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();



var app = builder.Build();


app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = string.Empty;
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Servic Api");
});
app.UseAuthorization();

app.MapControllers();

app.Run();
