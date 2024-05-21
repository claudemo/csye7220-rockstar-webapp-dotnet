
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Set the URLs the web app will listen on
builder.WebHost.UseUrls(":5113", "https://ec2-3-129-249-87.us-east-2.compute.amazonaws.com:5114");

// Add services to the container.
builder.Services.AddControllers();  // Add controllers to the service collection
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEverything", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});


// Add services to the container.  
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.Configure<HttpClientFactoryOptions>(options =>
{
    options.HttpMessageHandlerBuilderActions.Add(builder =>
    {
        var _httpClientHandler = new HttpClientHandler();
        _httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        builder.PrimaryHandler = _httpClientHandler;
    });
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
