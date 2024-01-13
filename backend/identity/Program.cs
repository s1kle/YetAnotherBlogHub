using BlogHub.Identity.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguration(builder.Configuration);
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("All");
app.UseIdentityServer();
app.MapControllers();

app.Run();