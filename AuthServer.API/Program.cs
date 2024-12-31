using AuthServer.CoreLayer.Configuration;
using AuthServer.Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


/*
 *Options Pattern i�lemi 
  Token olu�turulurken kullan�lacak olan CustomTokenOption class�ndaki property olan claim'lerin de�erlerini 
  appsetting.json'dan almak i�in a�a��daki gibi configure ediyoruz. Art�k bu i�lem CustomTokenOption de�erleri appsetting.json'dan al�nacakt�r.
  ayn� �ekilde client'�n user bilgisi istemeyen api'lere eri�ebilmesi i�in �retilecek token'�n bilgilerinide client class'�nda
  nesne �retti�imizde appsetting.json'dan e�leyerek eri�ebiliyoruz. al�yoruz.   
 */
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

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
