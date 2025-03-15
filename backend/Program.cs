using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TranslasApp.Backend.Data;
using TranslasApp.Backend.Interfaces;
using TranslasApp.Backend.Repoistory;

var builder = WebApplication.CreateBuilder(args);

// Add any necessary service configurations here
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", 
        builder => builder
            .WithOrigins("http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddDbContext<ApplicationDBContext>(options => { 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
});

builder.Services.AddScoped<ICarrierRepository, CarrierRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IReceiverRepository, ReceiverRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
    app.MapOpenApi();
    app.MapScalarApiReference(options => {
        options
            .WithTitle("GOL API")
            .WithTheme(ScalarTheme.Moon)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
    app.MapGet("/", () => Results.Redirect("/scalar"));
}

app.UseHttpsRedirection();

app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
