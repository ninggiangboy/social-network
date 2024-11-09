using API.Authentication;
using API.Configurations;
using Domain.Entities;
using Infrastructure.Cache;
using Infrastructure.Data;
using Infrastructure.MessageBroker;
using Infrastructure.Securities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddMassTransit(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddDependencyInjection();
builder.Services.AddExceptionHandling();
builder.Services.AddJwtAuthentication(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDevelopment();
}

app.UseCors(
    options => options
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.UseExceptionHandling();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();