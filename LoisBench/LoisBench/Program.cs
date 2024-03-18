using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LoisBench;
using LoisBench.Components;
using LoisBench.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/***
 *
 * Required Linux Packages:
 * lscpu  -> get CPU Model name
 * bash, top, grep, sed, awk  -> get CPU usage
 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<HostOptions>(x =>
{
    x.ServicesStartConcurrently = true;
    x.ServicesStopConcurrently = true;
});
builder.Services.AddHostedService<CpuBenchService>();
builder.Services.AddHostedService<SystemMonitorService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

app.Start();

var mainProcessId = Process.GetCurrentProcess().Id;
Console.WriteLine("Own Process ID: " + Process.GetCurrentProcess().Id);

var server = app.Services.GetService<IServer>();
var addressFeature = server?.Features.Get<IServerAddressesFeature>();

var electronStarted = false;
foreach (var address in addressFeature?.Addresses!)
{
    Console.WriteLine("Server is listening on address: " + address);
    if (!electronStarted)
    {
        Task.Run(() =>
        {
            var electron = new Electron();
            electron.Start(address, mainProcessId);
        });

        electronStarted = true;
    }
}

app.WaitForShutdown();