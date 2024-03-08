using Microsoft.EntityFrameworkCore;
using PersonaInformationRegistry.Api.Middlewares;
using PersonaInformationRegistry.Infrastructure;
using PersonaInformationRegistry.Infrastructure.Seeding;
using PersonalInformationRegistry.Application.Commands;
using PersonalInformationRegistry.Application.Mappings;
using PersonalInformationRegistry.Domain.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using PersonalInformationRegistry.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePersonCommand).Assembly));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePersonCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure Development.
if (app.Environment.IsDevelopment())
{
    DataInitializer.SeedData(app);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
