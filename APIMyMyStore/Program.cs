using APIMyMyStore;
using APIMyMyStore.Helpers;
using APIMyMyStore.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var jsonString = File.ReadAllText("firebase-adminsdk.json");
var obfirebase = CommonMethods.ConvertToDictionaryString(jsonString);
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("firebase-adminsdk.json"),
    ProjectId = obfirebase["project_id"]
});
var builder = WebApplication.CreateBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
                            .SetBasePath(builder.Environment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables();

builder.Configuration.AddConfiguration(configurationBuilder.Build());

// configure DI for application services
builder.Services.AddScoped<ITokenService, TokenService>();
// Add services to the container.

// var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// if (builder.Environment.EnvironmentName == "Development")
// {
//     defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// }
// else
// {
//     var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

//     if (connectionUrl != null)
//     {
//         // Use connection string provided at runtime by Heroku.
//         connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
//         var userPassSide = connectionUrl.Split("@")[0];
//         var hostSide = connectionUrl.Split("@")[1];

//         var user = userPassSide.Split(":")[0];
//         var password = userPassSide.Split(":")[1];
//         var host = hostSide.Split("/")[0];
//         var database = hostSide.Split("/")[1].Split("?")[0];

//         defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";

//     }
// }
// Variables.ConnectionSQL = defaultConnectionString;
builder.Services.AddControllers().AddNewtonsoftJson();
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

app.UseAuthorization();

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
       .AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());
    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.Run();