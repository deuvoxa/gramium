using Gramium.Framework.Extensions;
using Gramium.Framework.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Debug);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGramium(builder.Configuration["Telegram:Token"]!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var bot = app.Services.GetRequiredService<IGramiumBot>();
_ = Task.Run(async () => await bot.StartAsync());

app.Run();