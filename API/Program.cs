using System.Text;
using Common.Enums;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicCatalog.Api.Services;
using MusicCatalog.Api.Validators;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;
using MusicCatalog.Common.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("MusicCatalog.Api")));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateGenreRequestValidator>();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<ArtistService>();
builder.Services.AddScoped<AwardService>();
builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<MoodService>();
builder.Services.AddScoped<PlaylistService>();
builder.Services.AddScoped<RecordLabelService>();
builder.Services.AddScoped<SongService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "MusicCatalog",
        ValidAudience = "Users",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("!Password123!Password123!Password123"))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();

    if (!db.Users.Any(u => u.Username == "Admin"))
    {
        var admin = new User
        {
            Username = "Admin",
            Role = Role.Admin
        };

        var hash = UserService.HashPassword("Admin");
        admin.PasswordHash = hash;

        db.Users.Add(admin);
        db.SaveChanges();
    }
}

app.Run();