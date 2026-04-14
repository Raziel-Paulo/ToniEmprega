// Program.cs - MODIFICADO
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Filters;
using ToniEmprega.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// DbContext - MODIFICADO: Caminho relativo para pasta BaseDados
var baseDadosPath = Path.Combine(Directory.GetCurrentDirectory(), "BaseDados");
Directory.CreateDirectory(baseDadosPath); // Cria pasta se não existir

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")?
    .Replace("|DataDirectory|", baseDadosPath);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<RequireValidationFilter>();
});

var app = builder.Build();

// Pipeline
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

// ✅ CRIAR BASE DE DADES E ADMIN DEFAULT (APENAS SE NÃO EXISTIR)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // ✅ MODIFICADO: Apenas cria se não existir (não apaga mais)
    context.Database.EnsureCreated();

    Console.WriteLine("✅ Base de dados verificada!");

    // Verifica se admin já existe
    var adminExiste = context.Utilizadores.Any(u => u.Email == "admin@toniemprega.pt");

    if (!adminExiste)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("123");

        var adminUser = new Utilizador
        {
            Nome = "Administrador",
            Email = "admin@toniemprega.pt",
            Palavra_Passe = passwordHash,
            Data_Nascimento = new DateTime(1990, 1, 1),
            Data_Registro = DateTime.Now,
            Id_Tipo_Utilizador = 5, // Administrador
            Id_Estado_Validacao_Utilizador = 2 // Aprovado
        };

        context.Utilizadores.Add(adminUser);
        context.SaveChanges();

        var adminRecord = new Admin
        {
            Id_Utilizador = adminUser.Id
        };

        context.Admins.Add(adminRecord);
        context.SaveChanges();

        Console.WriteLine("✅✅✅ ADMIN CRIADO COM SUCESSO! ✅✅✅");
        Console.WriteLine("   Email: admin@toniemprega.pt");
        Console.WriteLine("   Password: 123");
    }
    else
    {
        Console.WriteLine("ℹ️ Admin já existe.");
    }
}

app.Run();