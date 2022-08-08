using StackApi.Data;
using Microsoft.EntityFrameworkCore;
using StackApi.Core.IConfiguration;
using StackApi.Common;
using StackExchange.Redis;
using StackApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StackApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackApi.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*Add CORS Attribute*/
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, Builder =>
                 Builder.WithOrigins("http://localhost:4200", "http://127.0.0.1:4200/", "http://192.168.188.171:8080")
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowCredentials());
});
/*Add policy*/
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("Admin", policy =>
    {
        policy.AddAuthenticationSchemes("Bearer");
        policy.RequireClaim("Admin", "true");
    });
});
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});
// Token Validation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
           ValidAudience = builder.Configuration["JwtConfig:Issuer"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"])),
           ClockSkew = TimeSpan.Zero
       };
       options.Events = new JwtBearerEvents
       {
           OnAuthenticationFailed = context =>
           {
               if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
               {
                   context.Response.Headers.Add("Token-Expired", "true");
               }
               return Task.CompletedTask;
           },
           OnMessageReceived = context =>
           {
               var act = context.Request.Query["access_token"];
               if (string.IsNullOrEmpty(act) == false)
               {
                   context.Token = act;
               }
               return Task.CompletedTask;
           }
       };
   });
builder.Services.AddDbContext<PartDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddControllers(option =>
{
    option.Filters.Add(new customActionFilter());
}).AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
});
// .AddJsonOptions(options =>
// {
//     options.JsonSerializerOptions.PropertyNamingPolicy = null; // turn off camel case
// });
/*Auto mapper*/
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*Api Versioning*/
builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddHttpClient<IPayment, Payment>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["paymentURL"]);
});
// Redis Cache
builder.Services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(builder.Configuration["Rediscon"]));
builder.Services.AddSingleton<ICacheService, RedisCacheService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseExceptionMiddleware(); // custom middleware
app.Run();
