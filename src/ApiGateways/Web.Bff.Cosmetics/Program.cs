using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;
using Web.Bff.Cosmetics.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var AllowCors = "_allowCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowCors,
                  policy =>
                  {
                      policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .WithMethods("GET", "POST", "PUT", "DELETE");
                  });
});

var routes = "Routes";

builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = routes;
});

builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddSwaggerGen();

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

builder.Services.AddResponseCompression(option =>
{
    //option.EnableForHttps = true;
    option.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "image/svg+xml" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowCors);

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<PermissionMiddleware>();

//app.UseMiddleware<RequestLoggingMiddleware>();

app.UseOcelot().Wait();

app.Run();