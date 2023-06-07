using MacMarketGroupApi.Models;
using MacMarketGroupApi.Services;
// using AspNetCore.Mvc.ModelBinding.Binders

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DBCollections>(builder.Configuration.GetSection("DBCollections"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add database Services
builder.Services.AddSingleton<AuthensService>();
builder.Services.AddSingleton<CategoriesService>();
builder.Services.AddSingleton<ProductsService>();
builder.Services.AddSingleton<UsersService>();

// Add Init Services
// builder.Services.AddSingleton<DBConnection>();
builder.Services.AddSingleton<AuthsHelper>();
builder.Services.AddSingleton<FilesHelper>();


builder.Services.AddScoped<CustomActionFilter>();

// builder.Services.AddScoped<IAuthsHelper, IAuthsHelper>();

// services.AddScoped<IJwtService, JwtService>();

// Options is Custom Model Binding
builder.Services.AddControllers(options =>
        {
            // options.ModelBinderProviders.Insert(0, new AuthorEntityBinderProvider());
        });

builder.Services.AddControllers();


// .AddJsonOptions(
//         options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("https://localhost:7000");
