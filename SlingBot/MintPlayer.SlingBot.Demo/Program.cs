using MintPlayer.SlingBot;
using MintPlayer.SlingBot.Abstractions;
using MintPlayer.SlingBot.Demo;
using MintPlayer.SlingBot.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services
    .Configure<BotOptions>(options =>
    {
        options.AppId = builder.Configuration["GithubApp:AppId"];
        options.ClientId = builder.Configuration["GithubApp:ClientId"];
        options.WebhookUrl = builder.Configuration["GithubApp:WebhookUrl"];
        options.WebhookSecret = builder.Configuration["GithubApp:WebhookSecret"];
        options.PrivateKey = builder.Configuration["GithubApp:PrivateKey"];
        options.PrivateKeyPath = builder.Configuration["GithubApp:PrivateKeyPath"];

        options.DevSmeeChannelUrl = builder.Configuration["WebhookProxy:DevSmeeChannelUrl"];
    });

// Github app => Webhook URL = https://slingbot.mintplayer.com/api/github/webhooks

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddLogging(options => options.AddConsole());
builder.Services.AddSlingBot<GithubProcessor>(builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHealthChecks("/healtz");
app.MapSlingBot();
app.MapControllers();

app.Run();
