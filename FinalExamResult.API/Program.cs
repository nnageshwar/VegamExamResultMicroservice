using FinalExamResult.DBContext;
using FinalExamResult.IRepository;
using FinalExamResult.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FinalYearResultDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FinalExamConnString")));

builder.Services.AddScoped<IStudentMarksRepository, StudentMarksRepository>();

builder.Services.AddControllers();

//Adding Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
