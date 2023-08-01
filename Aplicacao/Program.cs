using Dados.Repositorios;
using Dados.Servicos;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder);
builder.Services.AddControllers();
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

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<ServicoBuscarCervejas>();
    builder.Services.AddScoped<RepositorioBuscarCervejas>();
    builder.Services.AddScoped<ServicoBuscarCevejaPeloMaiorPreco>();
    builder.Services.AddScoped<RepositorioBuscarCervejaPeloMaiorPreco>();

}