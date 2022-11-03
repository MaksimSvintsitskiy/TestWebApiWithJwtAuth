using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TestWebApiWithJwtAuth.Authenticate;
using TestWebApiWithJwtAuth.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.

ConfigureApp(app);

app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<IUsersService, UsersService>();

    #region configure JWTAuthentication
    JwtAuthOptions jwtAuthOptions = new JwtAuthOptions();
    builder.Services.AddSingleton(_ => jwtAuthOptions);

    
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAuthOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtAuthOptions.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = jwtAuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true
            };
        });
    #endregion

    builder.Services.AddAuthorization();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

static void ConfigureApp(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

