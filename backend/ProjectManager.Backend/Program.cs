using Microsoft.EntityFrameworkCore;
using ProjectManager.Backend.Data;;

var builder = WebApplication.CreateBuilder(args);

// Подключение EF Core
if (!builder.Environment.IsEnvironment("Migration"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
}


// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

}

builder.Services.AddAutoMapper(typeof(Program));


app.UseHttpsRedirection();

app.Run();
