using Microsoft.Extensions.DependencyInjection;
using TCSTest.Interface;
using TCSTest.Middleware;
using TCSTest.Models;
using TCSTest.Repositories;
using TCSTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register repositories - each uses a separate json file
builder.Services.AddSingleton<JsonFileLockProvider>();
builder.Services.AddSingleton<IRepository<ContentItem>, ContentRepository>(sp =>
{
    var env = sp.GetRequiredService<IHostEnvironment>();
    var dataFolder = Path.Combine(env.ContentRootPath, "Json Store");
    return new ContentRepository(Path.Combine(dataFolder, "content_catalog.json"), sp.GetRequiredService<JsonFileLockProvider>());
});
builder.Services.AddScoped<IRepository<Channel>, ChannelRepository>(sp =>
{
    var env = sp.GetRequiredService<IHostEnvironment>();
    var dataFolder = Path.Combine(env.ContentRootPath, "Json Store");
    return new ChannelRepository(Path.Combine(dataFolder, "channels.json"), sp.GetRequiredService<JsonFileLockProvider>());
});
builder.Services.AddScoped<IRepository<ScheduleEntry>, ScheduleRepository>(sp =>
{
    var env = sp.GetRequiredService<IHostEnvironment>();
    var dataFolder = Path.Combine(env.ContentRootPath, "Json Store");
    return new ScheduleRepository(Path.Combine(dataFolder, "channel_schedule.json"), sp.GetRequiredService<JsonFileLockProvider>());
});

// services
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IChannelService, ChannelService>();
builder.Services.AddScoped<IScheduleService>(sp => new ScheduleService(sp.GetRequiredService<IRepository<ScheduleEntry>>(), sp.GetRequiredService<IRepository<Channel>>(), sp.GetRequiredService<IRepository<ContentItem>>()));

var app = builder.Build();

// use centralized error handling
app.UseApiErrorHandling();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
