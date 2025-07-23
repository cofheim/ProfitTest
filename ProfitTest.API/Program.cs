using Microsoft.EntityFrameworkCore;
using ProfitTest.Application.Handlers.Products;
using ProfitTest.Application.Interfaces;
using ProfitTest.Contracts.Messages;
using ProfitTest.Infrastructure.Messaging.Settings;
using ProfitTest.Persistence;
using ProfitTest.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;



services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IProductRepository, ProductRepository>();

services.AddProducer<ProductCreatedMessage>(KafkaSettings);
services.AddProducer<ProductUpdatedMessage>(KafkaSettings);
services.AddProducer<ProductDeletedMessage>(KafkaSettings);

services.AddConsumer<ProductCreatedMessage, ProductMessageHandler>(KafkaSettings);
services.AddConsumer<ProductUpdatedMessage, ProductMessageHandler>(KafkaSettings);
services.AddConsumer<ProductDeletedMessage, ProductMessageHandler>(KafkaSettings);

services.AddDbContext<ProfitTestDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ProfitTestDbContext)));
});

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
