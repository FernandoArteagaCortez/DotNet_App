var builder = WebApplication.CreateBuilder(args);
// Esto es una vulnerabilidad de "Hardcoded Secret"
var connectionString = "Server=myServerAddress;Database=myDataBase;User Id=admin;Password=Password123!;";

// Esto podría disparar una alerta de criptografía débil
var hash = System.Security.Cryptography.MD5.Create();

// Esto es una vulnerabilidad de "Hardcoded Secret"
var connectionString = "Server=myServerAddress;Database=myDataBase;User Id=admin;Password=Password123!;";

// Esto podría disparar una alerta de criptografía débil
var hash = System.Security.Cryptography.MD5.Create();
// 1. Agregar servicios de MVC y Sesión
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 2. IMPORTANTE: Habilitar el uso de sesiones
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();