using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using UniversitySchedule.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversitySchedule.UI.Pages.Admin.Subjects
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

        // Список предметів для відображення
        public List<SubjectDto> Subjects { get; set; } = new List<SubjectDto>();

        // Властивості для форми створення
        [BindProperty]
        public string NewSubjectName { get; set; } = string.Empty;

        // Властивості для форми редагування
        [BindProperty]
        public Guid EditedSubjectId { get; set; }
        [BindProperty]
        public string EditedSubjectName { get; set; } = string.Empty;

        // Властивість для видалення
        [BindProperty]
        public Guid DeletedSubjectId { get; set; }

        // Властивості для пошуку та пагінації
        [BindProperty(SupportsGet = true)]
        public string? SearchById { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchByName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        // Повідомлення користувачу
        [TempData]
        public string? Message { get; set; }
        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadSubjectsAsync();
            return Page();
        }

        private async Task LoadSubjectsAsync()
        {
            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                string apiUrl = "Subject";

                if (!string.IsNullOrWhiteSpace(SearchById) && Guid.TryParse(SearchById, out Guid parsedGuid))
                {
                    apiUrl = $"Subject/{parsedGuid}"; // Отримати предмет за ID
                }
                else if (!string.IsNullOrWhiteSpace(SearchByName))
                {
                    apiUrl = $"Subject/name/{SearchByName}"; // Отримати предмет за назвою
                }
                else
                {
                    apiUrl = $"Subject/page/{PageNumber}/{PageSize}"; // Пагінація
                }

                response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Десеріалізація може бути як списком, так і єдиним об'єктом.
                if (content.TrimStart().StartsWith("{") && !string.IsNullOrWhiteSpace(SearchById) || !string.IsNullOrWhiteSpace(SearchByName))
                {
                    var singleSubject = JsonSerializer.Deserialize<SubjectDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Subjects = singleSubject != null ? new List<SubjectDto> { singleSubject } : new List<SubjectDto>();
                }
                else
                {
                    Subjects = JsonSerializer.Deserialize<List<SubjectDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                   ?? new List<SubjectDto>();
                }

                if (!Subjects.Any() && string.IsNullOrEmpty(Message) && string.IsNullOrEmpty(ErrorMessage))
                {
                    Message = "Не знайдено жодного предмета за заданими критеріями.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error during Subject load: {Message}", ex.Message);
                ErrorMessage = $"Помилка завантаження предметів: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
                Subjects = new List<SubjectDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error during Subject load: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних предметів: {ex.Message}";
                Subjects = new List<SubjectDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Subject load: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
                Subjects = new List<SubjectDto>();
            }
        }

        public async Task<IActionResult> OnPostCreateSubjectAsync()
        {
            if (string.IsNullOrWhiteSpace(NewSubjectName))
            {
                ModelState.AddModelError(string.Empty, "Назва предмета не може бути порожньою.");
                await LoadSubjectsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                var requestBody = new
                {
                    Name = NewSubjectName
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PostAsync("Subject", content);
                response.EnsureSuccessStatusCode();

                Message = "Предмет успішно додано!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating Subject: {Message}", ex.Message);
                ErrorMessage = $"Помилка при створенні предмета: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error creating Subject: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating Subject: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditSubjectAsync()
        {
            if (EditedSubjectId == Guid.Empty || string.IsNullOrWhiteSpace(EditedSubjectName))
            {
                ModelState.AddModelError(string.Empty, "ID та назва предмета не можуть бути порожніми.");
                await LoadSubjectsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                var requestBody = new
                {
                    Name = EditedSubjectName
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PutAsync($"Subject/{EditedSubjectId}", content);
                response.EnsureSuccessStatusCode();

                Message = "Предмет успішно оновлено!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error updating Subject: {Message}", ex.Message);
                ErrorMessage = $"Помилка при оновленні предмета: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error updating Subject: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Subject: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteSubjectAsync()
        {
            if (DeletedSubjectId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID предмета для видалення не може бути порожнім.");
                await LoadSubjectsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                response = await client.DeleteAsync($"Subject/{DeletedSubjectId}");
                response.EnsureSuccessStatusCode();

                Message = "Предмет успішно видалено!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error deleting Subject: {Message}", ex.Message);
                ErrorMessage = $"Помилка при видаленні предмета: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting Subject: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}