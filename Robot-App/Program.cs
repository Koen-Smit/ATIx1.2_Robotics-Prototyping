using Robot_App.Components;
using Robot_App.Repositories;
using SimpleMqtt;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");
}
builder.Services.AddSingleton<IUserRepository>(sp => new SqlUserRepository(connectionString));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


var simpleMqttClient = new SimpleMqttClient(new()
{
    Host = "", 
    Port = ,
    ClientId = "",
    TimeoutInMs = , 
    UserName = "",
    Password = ""
});


builder.Services.AddSingleton(simpleMqttClient); 
builder.Services.AddHostedService<MqttMessageProcessingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
