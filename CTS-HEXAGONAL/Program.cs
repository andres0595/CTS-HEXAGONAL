using Autofac.Extensions.DependencyInjection;
using Autofac;
using Vitalea.Infraestructura.Connection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var interfaceConfig = new InterfaceConfig();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new Vitalea.Infraestructura.Injections());
        containerBuilder.RegisterModule(new Vitalea.Dominio.Injections());

    });


// Add services to the container.

builder.Services.AddControllers();
interfaceConfig.InitializeConfig();
// Otros servicios
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ReglasCors",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; //ACTIVAR PARA HTTPS
        options.SaveToken = true;
        var secretKey = interfaceConfig.secretKeyJWT;
        var IssuerJWT = interfaceConfig.issuerJWT;
        var AudienceJWT = interfaceConfig.audienceJWT;
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuerSigningKey = false,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidIssuer = IssuerJWT,
            ValidAudience = AudienceJWT,
            ClockSkew = TimeSpan.Zero,
        };
    });
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
