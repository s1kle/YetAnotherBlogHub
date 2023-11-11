using BlogHub.Identity.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllers();

app.Run();
