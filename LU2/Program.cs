using Microsoft.AspNetCore.Identity;
using ProjectMap.WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

string connStr = builder.Configuration["ConnectionStr"];

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 10;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = connStr;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register UserRepository in the DI container
builder.Services.AddScoped<EnviromentRepository>(provider => new EnviromentRepository(connStr));
builder.Services.AddScoped<Object2dRepository>(provider => new Object2dRepository(connStr));

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
app.MapGroup("/account").MapIdentityApi<IdentityUser>();

app.Run();
