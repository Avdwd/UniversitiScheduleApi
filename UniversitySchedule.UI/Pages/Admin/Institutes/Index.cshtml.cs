using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http; // Додано для HttpClient
using Microsoft.Extensions.Configuration;
using UniversitySchedule.UI.Models; // Додано для IConfiguration

namespace UniversitySchedule.UI.Pages.Admin.Institutes
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        // _configuration більше не потрібен в методах, якщо BaseAddress встановлено,
        // але його можна залишити, якщо потрібен для інших цілей або налагодження
        private readonly IConfiguration _configuration;

        public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public List<InstituteDto> Institutes { get; set; } = new List<InstituteDto>();

        // DTO (Data Transfer Object) для представлення Інституту, як він приходить/відправляється в API
        // Це має відповідати InstituteResponse/InstituteRequest з вашого API-проекту
        

        // Властивість для нового інституту (для створення)
        [BindProperty] // SupportsGet = true не потрібен, якщо використовується тільки для Post
        public string NewInstituteName { get; set; }

        // Властивості для редагування інституту
        [BindProperty]
        public Guid EditedInstituteId { get; set; }
        [BindProperty]
        public string EditedInstituteName { get; set; }

        // Властивості для видалення інституту
        [BindProperty]
        public Guid DeletedInstituteId { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            await LoadInstitutes();
            return Page();
        }

        // Хендлер для створення інституту
        public async Task<IActionResult> OnPostCreateInstituteAsync()
        {
            if (string.IsNullOrWhiteSpace(NewInstituteName))
            {
                ModelState.AddModelError(string.Empty, "Назва інституту не може бути порожньою.");
                await LoadInstitutes();
                return Page();
            }

            // Отримуємо іменований екземпляр HttpClient
            var client = _httpClientFactory.CreateClient("UniversityApi");

            var content = new StringContent(
                JsonSerializer.Serialize(new { Name = NewInstituteName }),
                Encoding.UTF8,
                "application/json"
            );

            // Використовуємо ВІДНОСНИЙ URL, оскільки BaseAddress вже встановлено
            var response = await client.PostAsync("api/Institute", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Інститут успішно додано!";
                return RedirectToPage();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Помилка при створенні інституту: {response.StatusCode} - {errorContent}");
            }

            await LoadInstitutes();
            return Page();
        }

        // Хендлер для редагування інституту
        public async Task<IActionResult> OnPostEditInstituteAsync()
        {
            if (string.IsNullOrWhiteSpace(EditedInstituteName))
            {
                ModelState.AddModelError(string.Empty, "Назва інституту не може бути порожньою.");
                await LoadInstitutes();
                return Page();
            }

            // Отримуємо іменований екземпляр HttpClient
            var client = _httpClientFactory.CreateClient("UniversityApi");

            var content = new StringContent(
                JsonSerializer.Serialize(new { Id = EditedInstituteId, Name = EditedInstituteName }),
                Encoding.UTF8,
                "application/json"
            );

            // Використовуємо ВІДНОСНИЙ URL
            var response = await client.PutAsync($"api/Institute/{EditedInstituteId}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Інститут успішно оновлено!";
                return RedirectToPage();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Помилка при оновленні інституту: {response.StatusCode} - {errorContent}");
            }

            await LoadInstitutes();
            return Page();
        }

        // Хендлер для видалення інституту
        public async Task<IActionResult> OnPostDeleteInstituteAsync()
        {
            // Отримуємо іменований екземпляр HttpClient
            var client = _httpClientFactory.CreateClient("UniversityApi");

            // Використовуємо ВІДНОСНИЙ URL
            var response = await client.DeleteAsync($"api/Institute/{DeletedInstituteId}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Інститут успішно видалено!";
                return RedirectToPage();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Помилка при видаленні інституту: {response.StatusCode} - {errorContent}");
            }

            await LoadInstitutes();
            return Page();
        }


        // Допоміжний метод для завантаження інститутів
        private async Task LoadInstitutes()
        {
            try
            {
                // Отримуємо іменований екземпляр HttpClient
                var client = _httpClientFactory.CreateClient("UniversityApi");

                // Використовуємо ВІДНОСНИЙ URL
                var response = await client.GetAsync("api/Institute");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Institutes = JsonSerializer.Deserialize<List<InstituteDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Не вдалося завантажити інститути: {ex.Message}");
                Institutes = new List<InstituteDto>();
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, $"Помилка обробки даних інститутів: {ex.Message}");
                Institutes = new List<InstituteDto>();
            }
            catch (Exception ex) // Загальна обробка невідомих винятків
            {
                ModelState.AddModelError(string.Empty, $"Виникла неочікувана помилка: {ex.Message}");
                Institutes = new List<InstituteDto>();
            }
        }
    }
}