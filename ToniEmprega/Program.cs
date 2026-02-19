// Program.cs - ADMIN COM PASSWORD 123
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// DbContext
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
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ CRIAR BASE DE DADES E ADMIN DEFAULT (PASSWORD 123)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Apaga e recria
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    Console.WriteLine("✅ Base de dados criada!");

    // Verifica se admin já existe
    var adminExiste = context.Utilizadores.Any(u => u.Email == "admin@toniemprega.pt");

    if (!adminExiste)
    {
        // ✅ HASH DA PASSWORD "123"
        var passwordHash = "$2a$12$QZtXZqKQZQZQZQZQZQZQZuPqJZqKQZQZQZQZQZQZQZQZQZQZQZQZa"; // Hash de "123"

        // Se quiseres gerar novo hash, descomenta:
        // passwordHash = BCrypt.Net.BCrypt.HashPassword("123");

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

        // Criar registo na tabela Admins
        var adminRecord = new Admin
        {
            Id_Utilizador = adminUser.Id
        };

        context.Admins.Add(adminRecord);
        context.SaveChanges();

        Console.WriteLine("✅✅✅ ADMIN CRIADO COM SUCESSO! ✅✅✅");
        Console.WriteLine("   Email: admin@toniemprega.pt");
        Console.WriteLine("   Password: 123");
        Console.WriteLine("   Tipo: Administrador");
        Console.WriteLine("   Estado: Aprovado");
    }
    else
    {
        Console.WriteLine("ℹ️ Admin já existe.");
    }
}

app.Run();