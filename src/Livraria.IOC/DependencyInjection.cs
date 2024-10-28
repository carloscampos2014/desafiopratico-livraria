using FluentValidation;
using Livraria.Database.Context;
using Livraria.Database.Repositories;
using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Requests;
using Livraria.Domain.Services;
using Livraria.Domain.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Livraria.IOC;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, string connectionString)
    {
        SQLitePCL.Batteries.Init();
        services.AddDbContext<LivrariaDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IValidator<BookRequest>, BookRequestValidator>();
        services.AddScoped<IAddBookService, AddBookService>();
        services.AddScoped<IDeleteBookService, DeleteBookService>();
        services.AddScoped<IGetAllBookService, GetAllBookService>();
        services.AddScoped<IGetByIdBookService, GetByIdBookService>();
        services.AddScoped<IUpdateBookService, UpdateBookService>();
    }

}
