using AuthServer.CoreLayer.Configuration;
using AuthServer.CoreLayer.Entities;
using AuthServer.CoreLayer.Repositories;
using AuthServer.CoreLayer.Services;
using AuthServer.CoreLayer.UnitOfWork;
using AuthServer.DataAccessLayer;
using AuthServer.DataAccessLayer.Repositories;
using AuthServer.DataAccessLayer.UnitOfWork;
using AuthServer.ServiceLayer.Services;
using AuthServer.Shared.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
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

        /*
           Dependency Injection (DI) Service Registration
         * Service katman�nda b�t�n class'lar� olu�tururken imzalar� ile birlikte olu�turdu�umuz i�in bu interface'den olu�acak bir nesneye
           DI yani Dependency Injection nesnesi denir. Bir interface'den bu interface implemente eden class ait bir nesne �retip
           API katman�nda bu nesneyi kullanmak i�in program.cs 'de register edilmesi gereklidir. Bu i�lem DI konteynar� ekleme i�lemi de 
           denir.Ayn� �ekilde bu service'lerde alt class'lar kullan�ld��� i�in �rne�in repository, unitofwork gibi class'lar�nda register 
           edilmesi gereklidir.B�ylece Ap� katman�nda bu interface'ler ile depandancy injection ile nesne �retip kullanabiliriz.
         */
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
       //Generic servisleri register ederken a�a��daki gibi edilir.
        builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


        //generic service'de birden fazla generic yap� var Tentity ve Dto  <> aras�na bir de virg�l koyarak ikinci generic yap�y� ekleriz.




        /*
         * Bu i�lem ile appsetting.jon'a giderek ConnectionStrings'in i�indeki SqlServer'� al�r ve bu SqlServer'� kullanarak
           veritaban� i�lemlerini ger�ekle�tirir.Migration basma i�lemini dataAccess katman�nda yapaca��m�z i�in 
           MigrationAssembly ile dataAccess katman�n� belirtiyoruz.
         */
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlOptions =>
            {
               sqlOptions.MigrationsAssembly("AuthServer.DataAccessLayer");
            });

        });
        /*
         Olu�turaca��m�z IdentityContext oldu�undan dolay� Identity'yi de program.cs'e AppDbContext class'� �zerinde program.cs'e
         eklememiz gereklidir.
         */
        builder.Services.AddIdentity<UserApp, IdentityRole>(options =>
        { 
        options.User.RequireUniqueEmail = true;
        options.Password.RequireNonAlphanumeric = false;

        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



        /*
           Gelen Token'�n Api Taraf�ndan Tan�nmas� i�in Gerekli Yap�land�rma

         * Bir sistemde birden fazla �yelik sistemi bulunabilir. �rne�in bayiler i�in farkl� bir �yelik sistemi bulunurken 
           m��teriler i�in de farkl� bir �yelik sistemi olabilir.Yani site i�inde iki farkl� login alan� bulunur.Bu �yelik
           sistemleri scheme olarak adland�r�l�r.Birden fazla �yelik sistemi olsayd� AddAthenc
           scheme ad� girmemiz gerekirdi. JwtBearerDefaults.AuthenticationScheme ifadesi sabit bir string ifadesidir. Bearer 
           stringi tutar.
         
          *DefaultChallengeScheme ile AddJwtBearer'�n �emas� kendi �emam�z�n ayn� oldu�unu bildirdik. AddJwtBearer methodu ile de 
           �yelik sistemimizde authetnication i�leminin token bazl� ger�ekle�ece�ini framework'e bildirdik. Sonra token'�m�z�n
           validation i�lemlerini olu�turmak istedi�imiz token'a g�re ayarlar�z.
         */

        builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
        var tokenOption = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();

        builder.Services.AddCustomTokenAuth(tokenOption!);

        builder.Services.AddSwaggerGen(c =>
        {
            // Swagger Authorization ba�l���n� ekleyin
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,  // Token header'da olacak
                Description = "Please enter a valid token", // Kullan�c�ya ne yapmas� gerekti�i bilgisi
                Name = "Authorization", // Swagger'da kullan�lacak ba�l�k
                Type = SecuritySchemeType.ApiKey, // API anahtar� t�r�
                BearerFormat = "JWT" // Token format�
            });

            // Swagger'a JWT do�rulama gereksinimini ekleyin (Herhangi bir rol gerektirmeden)
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Burada 'Bearer' ba�l���n� kullanaca��z
                }
            },
            new string[] {} // Roles gereksinimi eklenmeden sadece JWT do�rulama
        }
    });
        });




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

        app.UseAuthentication();
        app.UseAuthorization();
      
        app.MapControllers();

        app.Run();
    }
}