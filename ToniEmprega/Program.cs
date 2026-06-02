// Program.cs - LOCAL MDF

using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Filters;
using ToniEmprega.Models;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Base de dados local (.mdf) dentro da pasta BaseDeDados
AppDomain.CurrentDomain.SetData(
    "DataDirectory",
    builder.Environment.ContentRootPath
);

var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BaseDeDados\ToniEmpregaDB.mdf;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;MultipleActiveResultSets=True;Encrypt=False";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// SESSION
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// FILTER GLOBAL
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<RequireValidationFilter>();
});

var app = builder.Build();

// PIPELINE
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
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// CRIAR ADMIN AUTOMATICAMENTE
using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    try
    {
        // Garante que a base existe no arranque
        context.Database.EnsureCreated();

        Console.WriteLine("✅ Base de dados local verificada!");

        // Verifica se admin existe

        var adminExiste =
            context.Utilizadores
            .Any(u =>
                u.Email == "toniemprega@gmail.com"
            );

        if (!adminExiste)
        {
            Console.WriteLine("⚙️ Criando admin...");

            var passwordHash =
                BCrypt.Net.BCrypt
                .HashPassword("123");

            var adminUser = new Utilizador
            {
                Nome = "Administrador ToniEmprega",

                Email = "toniemprega@gmail.com",

                Palavra_Passe = passwordHash,

                Data_Nascimento =
                    new DateTime(1990, 1, 1),

                Data_Registro =
                    DateTime.Now,

                Id_Tipo_Utilizador = 5,

                Id_Estado_Validacao_Utilizador = 2
            };

            context.Utilizadores.Add(adminUser);

            context.SaveChanges();

            var adminRecord = new Admin
            {
                Id_Utilizador =
                    adminUser.Id
            };

            context.Admins.Add(adminRecord);

            context.SaveChanges();

            Console.WriteLine("✅ ADMIN CRIADO!");
            Console.WriteLine(
                "Email: toniemprega@gmail.com"
            );
            Console.WriteLine(
                "Password: 123"
            );
        }
        else
        {
            Console.WriteLine(
                "ℹ️ Admin já existe."
            );
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            "❌ ERRO AO LIGAR À BASE DE DADOS:"
        );

        Console.WriteLine(ex.Message);
    }
}

app.Run();