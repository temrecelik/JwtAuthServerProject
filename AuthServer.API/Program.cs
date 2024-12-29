using AuthServer.Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


/*
 * Token oluþturulurken kullanýlacak olan CustomTokenOption classýndaki property olan claim'lerin deðerlerini 
   appsetting.json'dan almak için aþaðýdaki gibi configure ediyoruz. Artýk bu iþlem CustomTokenOption 

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
