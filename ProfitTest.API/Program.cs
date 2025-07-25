using Microsoft.EntityFrameworkCore;
using ProfitTest.Application.Authentication;
using ProfitTest.Application.Handlers.Products;
using ProfitTest.Application.Interfaces;
using ProfitTest.Application.Interfaces.Auth;
using ProfitTest.Application.Interfaces.Messaging;
using ProfitTest.Application.Services;
using ProfitTest.Contracts.Messages;
using ProfitTest.Infrastructure.Messaging.Settings;
using ProfitTest.Persistence;
using ProfitTest.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Аутентификация
services.Configure<AuthSettings>(builder.Configuration.GetSection(nameof(AuthSettings)));
services.AddScoped<JwtService>();
services.AddScoped<IAuthService, AuthService>();
services.AddAuth(builder.Configuration);

// API
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// Сервисы и репозитории
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<IProductService, ProductService>();

// Кафка
var kafkaConfig = builder.Configuration.GetSection("Kafka:Product");

// Продюсеры
services.AddProducer<ProductCreatedMessage>(kafkaConfig, "ProductCreated");
services.AddProducer<ProductUpdatedMessage>(kafkaConfig, "ProductUpdated");
services.AddProducer<ProductDeletedMessage>(kafkaConfig, "ProductDeleted");

// Консьюмеры
services.AddConsumer<ProductCreatedMessage, ProductMessageHandler>(kafkaConfig, "ProductCreated");
services.AddConsumer<ProductUpdatedMessage, ProductMessageHandler>(kafkaConfig, "ProductUpdated");
services.AddConsumer<ProductDeletedMessage, ProductMessageHandler>(kafkaConfig, "ProductDeleted");

// БД
services.AddDbContext<ProfitTestDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ProfitTestDbContext)));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProfitTestDbContext>();
    dbContext.Database.Migrate();
}

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
