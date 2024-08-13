using BackChat.Application.Services;
using BackChat.DataAccess;
using BackChat.DataAccess.Repositories;
using BackChat.Extensions;
using BackChat.Hubs;
using ChatBack.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddStackExchangeRedisCache(options =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    options.Configuration = connection;
});
builder.Services.AddDbContext<BackChatDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("OnionTestDbContext"));
    });

builder.Services.AddScoped<IChatGroupsRepository, ChatGroupsRepository>();
builder.Services.AddScoped<IChatGroupsService, ChatGroupsService>();
builder.Services.AddScoped<IUserChatGroupsRepository, UserChatGroupsRepository>();
builder.Services.AddScoped<IUserChatGroupsService, UserChatGroupsService>();
builder.Services.AddScoped<IImageFileSystem, ImageFileSystem>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();


builder.Services.AddAutoMapper(typeof(MapperMaps));

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7125/chat")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });

});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddApiAuthentication(builder.Configuration);

var app = builder.Build();



app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Service V1");
    });
}

app.UseCors();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");



app.Run();
