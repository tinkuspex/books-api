using FluentValidation;
using FluentValidation.AspNetCore;
using PruebaUPCH.Application.Interfaces;
using PruebaUPCH.Application.Services;
using PruebaUPCH.Application.Validation;
using PruebaUPCH.Infrastructure.Database;
using PruebaUPCH.Infrastructure.Repositories;


namespace PruebaUPCH.Api.Extension
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Dapper DB Context
            services.AddSingleton<DapperContext>();

            // Repositories
            services.AddScoped<IBookRepository, BookRepository>();

            // Application Services
            services.AddScoped<IBookService, BookService>();
            // Validation
            services.AddValidatorsFromAssemblyContaining<BookCreateValidator>();
            services.AddValidatorsFromAssemblyContaining<BookUpdateValidator>();
            services.AddFluentValidationAutoValidation();

            return services;
        }
    }
}
