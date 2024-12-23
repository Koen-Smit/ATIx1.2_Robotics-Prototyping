using Robot_App.Components;
using Robot_App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register MQTT Service as a Singleton
builder.Services.AddSingleton<MqttService>();

var app = builder.Build();

// Configure the MQTT service
// Start the MQTT service asynchronously in the background
var mqttService = app.Services.GetRequiredService<MqttService>();
_ = Task.Run(async () =>
{
    await mqttService.ConnectAsync(
        // host: "your_mqtt_host",
        // port: 8883,
        // clientId: "webapp",
        // username: "your_username",
        // password: "your_password"
    );
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
