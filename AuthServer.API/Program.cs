using AuthServer.Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


/*
 * Token olu�turulurken kullan�lacak olan CustomTokenOption class�ndaki property olan claim'lerin de�erlerini 
   appsetting.json'dan almak i�in a�a��daki gibi configure ediyoruz. Art�k bu i�lem CustomTokenOption 

 */
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));


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
