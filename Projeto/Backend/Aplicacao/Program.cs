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

// Adicione esta linha para habilitar o CORS
app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<ServicoBuscarCervejas>();
    builder.Services.AddScoped<RepositorioBuscarCervejas>();
    builder.Services.AddScoped<ServicoBuscarCevejaPeloMaiorPreco>();
    builder.Services.AddScoped<RepositorioBuscarCervejaPeloMaiorPreco>();
    builder.Services.AddScoped<ServicoBuscarHistoricoPrecos>();
    builder.Services.AddScoped<RepositorioBuscarHistoricoPrecos>();

    // Configure a política de CORS aqui
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                //builder.WithOrigins("https://beerprice.azurewebsites.net/") // Substitua pela origem do seu aplicativo React Native
                builder.WithOrigins("http://192.168.0.109:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
    });

}