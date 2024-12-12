using Gramium.Examples.BudgetManager.Database;
using Gramium.Examples.BudgetManager.Services;
using Gramium.Framework.Database.Enums;
using Gramium.Framework.Extensions;
using Gramium.Framework.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Debug);
});

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddGramium(configuration["Telegram:Token"]!)
    .AddDatabase<BudgetManagerDbContext>(configuration.GetConnectionString("DefaultConnection")!, DatabaseProvider.Postgresql);
services.AddScoped<UserService>();

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