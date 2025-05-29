using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace UniversitySchedule.UI.Pages;

public class IndexModel : PageModel
{
    // Властивість для зв'язування з формою
    [BindProperty] // Обов'язково для автоматичного зв'язування даних з форми
    public InputModel Input { get; set; } = new InputModel(); // Ініціалізуємо, щоб уникнути NullReferenceException

    // Клас для визначення вхідних даних форми
    public class InputModel
    {
        [Required(ErrorMessage = "Будь ласка, введіть логін.")]
        [Display(Name = "Логін")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть пароль.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

    public void OnGet()
    {
        // Цей метод викликається, коли сторінка завантажується вперше (HTTP GET)
        // Тут ви можете виконати ініціалізацію, якщо потрібно, наприклад, перевірити, чи вже автентифікований користувач.
        // Наприклад, якщо користувач вже увійшов, перенаправити його:
        // if (User.Identity.IsAuthenticated)
        // {
        //     Response.Redirect("/Schedule"); // або на рольову сторінку
        // }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Цей метод викликається, коли форма відправляється (HTTP POST)

        // Перевіряємо, чи всі поля форми відповідають вимогам валідації (наприклад, [Required])
        if (!ModelState.IsValid)
        {
            // Якщо є помилки валідації, повертаємося на поточну сторінку з помилками
            return Page();
        }

        // --- Тут буде логіка звернення до вашого API для автентифікації ---
        // Наприклад:
        // var authService = new YourAuthenticationService(); // Замініть на ваш сервіс
        // var isAuthenticated = await authService.AuthenticateUser(Input.Login, Input.Password);

        // Для початку просто імітуємо успішну автентифікацію
        bool isAuthenticated = (Input.Login == "test" && Input.Password == "password"); // Замініть на реальну логіку

        if (isAuthenticated)
        {
            // Успішна автентифікація.
            // Тепер вам потрібно буде реалізувати створення Identity, Cookies, тощо.
            // Це вимагає інтеграції з ASP.NET Core Identity або ручної реалізації аутентифікації на основі Cookies/JWT.
            // Для простоти зараз просто перенаправимо (це не повна автентифікація):
            // return RedirectToPage("/Schedule"); // Перенаправлення на сторінку розкладу або рольовий дашборд

            // TODO: Реалізувати реальну автентифікацію (наприклад, за допомогою HttpClient до вашого API)
            // Після автентифікації вам потрібно буде отримати інформацію про користувача та його ролі
            // і створити Principal та Claims для зберігання в сесії/куках.
            // Наприклад:
            // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

            // Просто повертаємося на сторінку з повідомленням про успіх для тестування
            // У реальному додатку тут буде redirect
            return RedirectToPage("/SuccessLogin", new { message = "Аутентифікація успішна!" }); // Тимчасове перенаправлення для тесту
        }
        else
        {
            // Невідомий логін або невірний пароль
            ModelState.AddModelError(string.Empty, "Невірний логін або пароль.");
            return Page(); // Повертаємося на сторінку з помилкою
        }
    }
}

