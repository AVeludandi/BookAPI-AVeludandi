var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on specific ports
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP port
    options.ListenAnyIP(5001, configure => configure.UseHttps()); // HTTPS port
});

// Add services to the container.
builder.Services.AddControllers();

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200"
                )
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
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

// Correct middleware order
app.UseCors("AllowAngular"); // CORS first
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
