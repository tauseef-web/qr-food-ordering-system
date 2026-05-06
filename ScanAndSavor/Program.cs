using Microsoft.EntityFrameworkCore;
using ScanAndSavor.Data;
using ScanAndSavor.PlatformData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<PlatformDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PlatformConnection")));

// Temporary default tenant context for scaffolded admin pages.
// The production flow should replace this with a request-scoped tenant DbContext factory
// resolved from `{tenantSubdomain}.easeurorder.com`.
var tenantConnectionTemplate = builder.Configuration.GetConnectionString("TenantConnection")
    ?? throw new InvalidOperationException("TenantConnection is not configured.");
var defaultTenantDatabase = builder.Configuration["DefaultTenantDatabase"]
    ?? "EaseUrOrder_Tenant_Template";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(string.Format(tenantConnectionTemplate, defaultTenantDatabase)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();