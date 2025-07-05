using Application;
using Application.Common.Interfaces;
using Domain;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDomain()
    .AddApplication()
    .AddInfrastructure();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/migrate", async (CancellationToken cancellationToken = default) =>
    {
        // create new scope for the migration operation
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            // Ensure the database is created and migrations are applied.
            await dbContext.MigrateAsync(cancellationToken);

            // Seed the identity data.
            await dbContext.SeedAsync(cancellationToken);
        }

        return Results.Ok("Database migrated and seeded.");
    });
}
else
{
    // create new scope for the migration operation
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        // Ensure the database is created and migrations are applied.
        await dbContext.MigrateAsync();

        // Seed the identity data.
        await dbContext.SeedAsync();
    }
}

app.Run();