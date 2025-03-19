using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Motora.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AccountService _authService;

        // Единый конструктор
        public ProfileController(AccountService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Email и пароль обязательны.");
                return View();
            }

            try
            {
                var user = await _authService.LoginAsync(email, password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный email или пароль.");
                    return View();
                }

                HttpContext.Session.SetInt32("ID", user.id);
                HttpContext.Session.SetString("Email", user.email);
                HttpContext.Session.SetString("YourName", user.your_name);

                // Редирект на главную страницу
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                ModelState.AddModelError("", "Произошла ошибка. Попробуйте позже.");
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> register(string email, string password, string your_name)
        {
            var user = await _authService.RegisterAsync(email, password, your_name);

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь с таким email уже существует.");
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
