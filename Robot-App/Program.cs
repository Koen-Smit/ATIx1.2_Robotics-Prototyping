using Robot_App.Components;
using SimpleMqtt;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");
}
builder.Services.AddSingleton<IBattery>(sp => new BatteryService(connectionString));
builder.Services.AddSingleton<ITask>(sp => new TaskService(connectionString));


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SimpleMqttClient>(sp => new SimpleMqttClient(new()
{

}));

builder.Services.AddHostedService<MqttProcessingService>();


builder.Services.AddSingleton<IStop, StopService>(sp =>
{
    var mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ("Robot-App");
    return new StopService(mqttClient);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();