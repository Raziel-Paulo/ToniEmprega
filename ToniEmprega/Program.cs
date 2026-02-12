// Program.cs
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext com log detalhado para debug
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors());

// Add Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ============================================
// CORREÇÃO: Criar base de dados de forma segura
// ============================================
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Garantir que a base de dados é eliminada e recriada se houver erros de schema
        // context.Database.EnsureDeleted(); // Descomenta se quiseres forçar recriação

        // Criar a base de dados e aplicar migrações
        context.Database.EnsureCreated();

        // Ou usar migrações formais:
        // context.Database.Migrate();

        Console.WriteLine("Base de dados criada com sucesso!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERRO ao criar base de dados: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    // Não impedir a aplicação de correr, mas logar o erro
}

app.Run();