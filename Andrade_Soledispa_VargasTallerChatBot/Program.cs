using Andrade_Soledispa_VargasTallerChatBot;
using Andrade_Soledispa_VargasTallerChatBot.Interfaces;
using Andrade_Soledispa_VargasTallerChatBot.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IChatBotService, ChatBotService>();
builder.Services.AddScoped<IRespuestaRepository, RespuestaRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRespuestaRepository, RespuestaRepository>();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();  


app.UseAuthorization();
app.MapControllers();
app.Run();
