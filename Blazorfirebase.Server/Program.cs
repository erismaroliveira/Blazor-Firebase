using Blazorfirebase.Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIAL", Path.Combine(Environment.CurrentDirectory, builder.Configuration.GetValue<string>("Firebase:PathJson")));

var tokenAuthority = string.Format("https://securetoken.google.com/{0}", builder.Configuration.GetValue<string>("Firebase:ProjectId"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = tokenAuthority;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = tokenAuthority,
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetValue<string>("Firebase:ProjectId"),
            ValidateLifetime = true
        };
    });

builder.Services.Configure<FirebaseConfig>(builder.Configuration.GetSection("Firebase"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCors(opt => opt.AddDefaultPolicy(builder => {
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    //builder.WithOrigins("*", "https://localhost:7273/");
    //builder.AllowAnyHeader();
    //builder.AllowAnyMethod();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
