using System;
using AutoMapper;
using Infraestrutura.Contexto;
using Infraestrutura.Repositorio;
using Microsoft.EntityFrameworkCore;
using ToDoApi.DTO.Mappings;
using ToDoApi.Extensao;
using ToDoApi.Filtros;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ApiToDo",
        Version = "v1",
        Description = "Lista de Tarefas",
        Contact = new OpenApiContact
        {
            Name = "Monica Lima",
            Email = "flima.monica@gmail.com"
        }
     });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});



builder.Services.AddScoped<TarefaFiltros>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
string? SqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
                             options.UseSqlServer(SqlConnection));

var MapearConfig = new MapperConfiguration(mc =>
                             {
                                 mc.AddProfile(new MapeandoPerfil());
                             });
IMapper mapper = MapearConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Listagem de Tarefas");
    });
}
app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
