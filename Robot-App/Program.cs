using Robot_App.Components;
using SimpleMqtt;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");
}
builder.Services.AddSingleton<ITask>(sp => new TaskService(connectionString));


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SimpleMqttClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var mqttConfig = configuration.GetSection("MqttConnection").Get<MqttConfig>();

    return new SimpleMqttClient(new()
    {
        Host = mqttConfig!.Host,
        Port = mqttConfig.Port,
        ClientId = mqttConfig.ClientId,
        TimeoutInMs = mqttConfig.TimeoutInMs,
        UserName = mqttConfig.UserName,
        Password = mqttConfig.Password
    });
});

builder.Services.AddHostedService<MqttProcessingService>();


builder.Services.AddSingleton<IStop, StopService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var mqttConfig = configuration.GetSection("MqttConnection").Get<MqttConfig>();

    var mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ("Robot-App", configuration);
    return new StopService(mqttClient);
});

builder.Services.AddSingleton<IBattery, BatteryService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var mqttConfig = configuration.GetSection("MqttConnection").Get<MqttConfig>();

    var mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ("Robot-App", configuration);
    return new BatteryService(connectionString, mqttClient);
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