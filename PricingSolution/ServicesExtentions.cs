using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pricing.Application.Interfaces;
using Pricing.Infrastructure.Persistence;
using Pricing.Infrastructure.Repositories;

namespace Pricing.Api;

public static class ServicesExtentions
{
    public static void RegisterRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ISupplierRepository, SupplierRepository>();
        builder.Services.AddTransient<IProductRepository, ProductRepository>();
        builder.Services.AddTransient<IPriceListRepository, PriceListRepository>();
        builder.Services.AddTransient<IPriceCalculator, PriceCalculator>();
        builder.Services.AddTransient<IRateProvider, RateProvider>();

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    public static void RegistEndPointsDefinitions(this WebApplication app)
    {
        IEnumerable<IEndPointDefinition>? endPointDefinitions = typeof(Program).Assembly
                                                .GetTypes()
                                                .Where(e => e.IsAssignableTo(typeof(IEndPointDefinition)) && !e.IsAbstract && !e.IsInterface)
                                                .Select(Activator.CreateInstance)
                                                .Cast<IEndPointDefinition>();

        foreach (IEndPointDefinition endPoint in endPointDefinitions)
            endPoint.RegisterEndPoint(app);
    }
}
