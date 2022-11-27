using KingMarvel.CrossCutting.IoC;
using KingMarvel.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using static KingMarvel.API.Controllers.CharacterController;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
NativeInjectorBootStrapper.InjectContext(builder.Services, builder.Configuration);
NativeInjectorBootStrapper.RegisterAutoMapper(builder.Services);
NativeInjectorBootStrapper.RegistereApiBehaviors(builder.Services);
NativeInjectorBootStrapper.RegisterServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sp = builder.Services.BuildServiceProvider();

using (var scope = sp.CreateScope())
{
    var scopedServices = scope.ServiceProvider;

    var databaseManager = scope?.ServiceProvider.GetService<IDatabaseManager>();

    if (databaseManager != null)
        databaseManager.SeedData();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapControllers();

app.Run();
