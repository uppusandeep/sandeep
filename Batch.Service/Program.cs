using Batch.BusinessLogic.Interfaces;
using Batch.BusinessLogic.Services;
using Batch.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUnemploymentInsuranceRepository, InMemoryUnemploymentInsuranceRepository>();
builder.Services.AddScoped<IUnemploymentInsuranceService, UnemploymentInsuranceService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
