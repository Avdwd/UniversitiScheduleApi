using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // Для SelectList
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using UniversitySchedule.UI.Models;

namespace UniversitySchedule.UI.Pages.Admin.Groups
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

       

        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
        public SelectList InstitutesSelectList { get; set; }

        [BindProperty]
        public string NewGroupName { get; set; } = string.Empty;
        [BindProperty]
        public Guid NewGroupInstituteId { get; set; }

        [BindProperty]
        public Guid EditedGroupId { get; set; }
        [BindProperty]
        public string EditedGroupName { get; set; } = string.Empty;
        [BindProperty]
        public Guid EditedGroupInstituteId { get; set; }

        [BindProperty]
        public Guid DeletedGroupId { get; set; }

        [TempData]
        public string Message { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadGroupsAndInstitutesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateGroupAsync()
        {
            if (string.IsNullOrWhiteSpace(NewGroupName) || NewGroupInstituteId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Назва групи та інститут не можуть бути порожніми.");
                await LoadGroupsAndInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null; // Оголошуємо response тут

            try
            {
                var requestBody = new
                {
                    Name = NewGroupName,
                    Institute = new { Id = NewGroupInstituteId }
                };
                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PostAsync("Group", content); // Присвоюємо response
                response.EnsureSuccessStatusCode();

                Message = "Групу успішно додано!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Помилка при створенні групи: {Message}", ex.Message);
                ErrorMessage = $"Помилка при створенні групи: {ex.Message}";
                // Тепер response доступний, і ми можемо читати його вміст
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Помилка десеріалізації JSON при створенні групи: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Виникла неочікувана помилка при створенні групи: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditGroupAsync()
        {
            if (EditedGroupId == Guid.Empty || string.IsNullOrWhiteSpace(EditedGroupName) || EditedGroupInstituteId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID групи, назва та інститут не можуть бути порожніми.");
                await LoadGroupsAndInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null; // Оголошуємо response тут

            try
            {
                var requestBody = new
                {
                    Name = EditedGroupName,
                    Institute = new { Id = EditedGroupInstituteId }
                };
                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PutAsync($"Group/{EditedGroupId}", content); // Присвоюємо response
                response.EnsureSuccessStatusCode();

                Message = "Групу успішно оновлено!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Помилка при оновленні групи: {Message}", ex.Message);
                ErrorMessage = $"Помилка при оновленні групи: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Помилка десеріалізації JSON при оновленні групи: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Виникла неочікувана помилка при оновленні групи: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteGroupAsync()
        {
            if (DeletedGroupId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID групи для видалення не може бути порожнім.");
                await LoadGroupsAndInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null; // Оголошуємо response тут

            try
            {
                response = await client.DeleteAsync($"Group/{DeletedGroupId}"); // Присвоюємо response
                response.EnsureSuccessStatusCode();

                Message = "Групу успішно видалено!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Помилка при видаленні групи: {Message}", ex.Message);
                ErrorMessage = $"Помилка при видаленні групи: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Виникла неочікувана помилка при видаленні групи: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }

        private async Task LoadGroupsAndInstitutesAsync()
        {
            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage groupResponse = null;     // Оголошуємо тут
            HttpResponseMessage instituteResponse = null; // Оголошуємо тут

            // Завантаження груп
            try
            {
                groupResponse = await client.GetAsync("Group"); // Присвоюємо сюди
                groupResponse.EnsureSuccessStatusCode();
                var groupContent = await groupResponse.Content.ReadAsStringAsync();
                Groups = JsonSerializer.Deserialize<List<GroupDto>>(groupContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                         ?? new List<GroupDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Не вдалося завантажити групи з API: {Message}", ex.Message);
                ErrorMessage = $"Не вдалося завантажити групи: {ex.Message}";
                if (groupResponse != null) // Перевіряємо, чи є response
                {
                    var errorDetails = await groupResponse.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)groupResponse.StatusCode}, Деталі: {errorDetails})";
                }
                Groups = new List<GroupDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Помилка обробки JSON при завантаженні груп: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних груп: {ex.Message}";
                Groups = new List<GroupDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Виникла неочікувана помилка при завантаженні груп: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
                Groups = new List<GroupDto>();
            }

            // Завантаження інститутів для випадаючого списку
            try
            {
                instituteResponse = await client.GetAsync("api/Institute"); // Присвоюємо сюди
                instituteResponse.EnsureSuccessStatusCode();
                var instituteContent = await instituteResponse.Content.ReadAsStringAsync();
                var institutes = JsonSerializer.Deserialize<List<InstituteDto>>(instituteContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                 ?? new List<InstituteDto>();
                InstitutesSelectList = new SelectList(institutes, "Id", "Name");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Не вдалося завантажити інститути для вибору: {Message}", ex.Message);
                if (instituteResponse != null) // Перевіряємо, чи є response
                {
                    var errorDetails = await instituteResponse.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)instituteResponse.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Виникла неочікувана помилка при завантаженні інститутів для вибору: {Message}", ex.Message);
            }
        }
    }
}