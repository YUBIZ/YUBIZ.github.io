using Blog;
using Blog.Models.Config;
using Blog.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(builder.Configuration.Get<AppSettings>() ?? throw new InvalidOperationException("AppSettings를 찾을 수 없습니다."));
builder.Services.AddSingleton(builder.Configuration.GetSection("GitHubServiceSettings").Get<GitHubServiceSettings>() ?? throw new InvalidOperationException("GitHubServiceSettings를 찾을 수 없습니다."));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<GitHubService>();

await builder.Build().RunAsync();
