using PsychometricWeb;
using PsychometricWeb.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPsychometricRepo, PsychometricRepo>();
builder.Services.UserAuthenConfig();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Psycho}/{action=Start}/{id?}");

app.Run();
