using ApiRevision;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog();

Log.Logger= new LoggerConfiguration().
            MinimumLevel.Debug().
            WriteTo.File(@"MyLog\apiLog").
            CreateLogger();

builder.Services.AddControllers
    (opt => opt.ReturnHttpNotAcceptable=true).
    AddNewtonsoftJson().
    AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");

    option.UseSqlServer(connectionString);
}
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
