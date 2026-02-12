// Program.cs - VERSÃO LIMPA E DEFINITIVA
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// DbContext - SEM MIGRAÇÕES, apenas EnsureCreated
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// IMPORTANTE: Verificar se wwwroot existe
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ============================================
// RECRIAR BASE DE DADOS DO ZERO (SEM MIGRAÇÕES)
// ============================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        // ELIMINAR TUDO E RECRIAR
        context.Database.EnsureDeleted();
        Console.WriteLine("✓ Base de dados antiga eliminada");

        context.Database.EnsureCreated();
        Console.WriteLine("✓ Base de dados criada com sucesso!");

        // Verificar tabelas criadas
        var tabelas = context.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        Console.WriteLine("\nTabelas criadas:");
        foreach (var tabela in tabelas)
        {
            Console.WriteLine($"  - {tabela}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ ERRO: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
}

app.Run();