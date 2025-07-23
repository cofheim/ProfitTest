using Microsoft.EntityFrameworkCore;
using ProfitTest.Application.Handlers.Products;
using ProfitTest.Application.Interfaces;
using ProfitTest.Application.Interfaces.Messaging;
using ProfitTest.Application.Services;
using ProfitTest.Contracts.Messages;
using ProfitTest.Infrastructure.Authentication;
using ProfitTest.Infrastructure.Messaging.Settings;
using ProfitTest.Persistence;
using ProfitTest.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// аутентификация
services.Configure<AuthSettings>(builder.Configuration.GetSection(nameof(AuthSettings)));
services.AddScoped<JwtService>();
services.AddAuth(builder.Configuration);

// API
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// сервисы и репозитории
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<IProductService, ProductService>();

// обработчики сообщений
services.AddScoped<IMessageHandler<ProductCreatedMessage>, ProductMessageHandler>();
services.AddScoped<IMessageHandler<ProductUpdatedMessage>, ProductMessageHandler>();
services.AddScoped<IMessageHandler<ProductDeletedMessage>, ProductMessageHandler>();

// кафка
var kafkaConfig = builder.Configuration.GetSection("Kafka:Product");

// продюсеры
services.AddProducer<ProductCreatedMessage>(kafkaConfig);
services.AddProducer<ProductUpdatedMessage>(kafkaConfig);
services.AddProducer<ProductDeletedMessage>(kafkaConfig);

// консьюмеры
services.AddConsumer<ProductCreatedMessage, ProductMessageHandler>(kafkaConfig);
services.AddConsumer<ProductUpdatedMessage, ProductMessageHandler>(kafkaConfig);
services.AddConsumer<ProductDeletedMessage, ProductMessageHandler>(kafkaConfig);

// БД
services.AddDbContext<ProfitTestDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ProfitTestDbContext)));
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
