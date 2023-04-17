using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RegisterDemo.Data;
using RegisterDemo.Service.Model;
using RegisterDemo.Service.Service;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// for Entity FrameWork
var Configuration = builder.Configuration;
builder.Services.AddDbContext<DbDemo>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnect")));

//identity
//builder.Services.AddDbContext<DbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnect")));
builder.Services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<DbDemo>()
.AddDefaultTokenProviders();

//Adding Authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

//Add config for required email
builder.Services.Configure<IdentityOptions>(
    opts => opts.SignIn.RequireConfirmedEmail = true
    );

//email config
var emailConfig = Configuration
.GetSection("EmailConfiguration")
.Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

////service
builder.Services.AddScoped<IEmailService, EmailService>();

//controller
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
