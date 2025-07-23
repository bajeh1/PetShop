using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using PetShop.Application.Interfaces;
using PetShop.Application.Mappings;
using PetShop.Core.Contexts;
using PetShop.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using PetShop.Application.Behaviors;
using PetShop.Application.Commands;
using PetShop.Application.Commands.Orders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("PetStore"));


//Register the Mapping Profile
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IPetRepository, PetRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();

//Register Mediatr
    var applicationAssembly = typeof(CreateOrderCommand).Assembly;
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssembly));
    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddValidatorsFromAssembly(applicationAssembly);
    // builder.Services.AddValidatorsFromAssembly(typeof(UpdateOrderItemsCommand).Assembly);
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    app.UseExceptionHandler(); 

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapControllers();



    app.Run();
    