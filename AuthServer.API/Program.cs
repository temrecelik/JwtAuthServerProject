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
         *Options Pattern iþlemi 
          Token oluþturulurken kullanýlacak olan CustomTokenOption classýndaki property olan claim'lerin deðerlerini 
          appsetting.json'dan almak için aþaðýdaki gibi configure ediyoruz. Artýk bu iþlem CustomTokenOption deðerleri appsetting.json'dan alýnacaktýr.
          ayný þekilde client'ýn user bilgisi istemeyen api'lere eriþebilmesi için üretilecek token'ýn bilgilerinide client class'ýnda
          nesne ürettiðimizde appsetting.json'dan eþleyerek eriþebiliyoruz. alýyoruz.   
         */
        builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
        builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

        /*
           Dependency Injection (DI) Service Registration
         * Service katmanýnda bütün class'larý oluþtururken imzalarý ile birlikte oluþturduðumuz için bu interface'den oluþacak bir nesneye
           DI yani Dependency Injection nesnesi denir. Bir interface'den bu interface implemente eden class ait bir nesne üretip
           API katmanýnda bu nesneyi kullanmak için program.cs 'de register edilmesi gereklidir. Bu iþlem DI konteynarý ekleme iþlemi de 
           denir.Ayný þekilde bu service'lerde alt class'lar kullanýldýðý için örneðin repository, unitofwork gibi class'larýnda register 
           edilmesi gereklidir.Böylece Apý katmanýnda bu interface'ler ile depandancy injection ile nesne üretip kullanabiliriz.
         */
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
       //Generic servisleri register ederken aþaðýdaki gibi edilir.
        builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


        //generic service'de birden fazla generic yapý var Tentity ve Dto  <> arasýna bir de virgül koyarak ikinci generic yapýyý ekleriz.




        /*
         * Bu iþlem ile appsetting.jon'a giderek ConnectionStrings'in içindeki SqlServer'ý alýr ve bu SqlServer'ý kullanarak
           veritabaný iþlemlerini gerçekleþtirir.Migration basma iþlemini dataAccess katmanýnda yapacaðýmýz için 
           MigrationAssembly ile dataAccess katmanýný belirtiyoruz.
         */
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlOptions =>
            {
               sqlOptions.MigrationsAssembly("AuthServer.DataAccessLayer");
            });

        });
        /*
         Oluþturacaðýmýz IdentityContext olduðundan dolayý Identity'yi de program.cs'e AppDbContext class'ý üzerinde program.cs'e
         eklememiz gereklidir.
         */
        builder.Services.AddIdentity<UserApp, IdentityRole>(options =>
        { 
        options.User.RequireUniqueEmail = true;
        options.Password.RequireNonAlphanumeric = false;

        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



        /*
           Gelen Token'ýn Api Tarafýndan Tanýnmasý için Gerekli Yapýlandýrma

         * Bir sistemde birden fazla üyelik sistemi bulunabilir. Örneðin bayiler için farklý bir üyelik sistemi bulunurken 
           müþteriler için de farklý bir üyelik sistemi olabilir.Yani site içinde iki farklý login alaný bulunur.Bu üyelik
           sistemleri scheme olarak adlandýrýlýr.Birden fazla üyelik sistemi olsaydý AddAthenc
           scheme adý girmemiz gerekirdi. JwtBearerDefaults.AuthenticationScheme ifadesi sabit bir string ifadesidir. Bearer 
           stringi tutar.
         
          *DefaultChallengeScheme ile AddJwtBearer'ýn þemasý kendi þemamýzýn ayný olduðunu bildirdik. AddJwtBearer methodu ile de 
           üyelik sistemimizde authetnication iþleminin token bazlý gerçekleþeceðini framework'e bildirdik. Sonra token'ýmýzýn
           validation iþlemlerini oluþturmak istediðimiz token'a göre ayarlarýz.
         */

        builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
        var tokenOption = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();

        builder.Services.AddCustomTokenAuth(tokenOption!);

        builder.Services.AddSwaggerGen(c =>
        {
            // Swagger Authorization baþlýðýný ekleyin
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,  // Token header'da olacak
                Description = "Please enter a valid token", // Kullanýcýya ne yapmasý gerektiði bilgisi
                Name = "Authorization", // Swagger'da kullanýlacak baþlýk
                Type = SecuritySchemeType.ApiKey, // API anahtarý türü
                BearerFormat = "JWT" // Token formatý
            });

            // Swagger'a JWT doðrulama gereksinimini ekleyin (Herhangi bir rol gerektirmeden)
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Burada 'Bearer' baþlýðýný kullanacaðýz
                }
            },
            new string[] {} // Roles gereksinimi eklenmeden sadece JWT doðrulama
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