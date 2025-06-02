using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using UniversitySchedule.UI.Models;
using System;
using System.Globalization; // Додано для TryParseExact
using System.Linq; // Додано для Split

namespace UniversitySchedule.UI.Pages.Admin.ClassTimes
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

        public List<ClassTimeDto> ClassTimes { get; set; } = new List<ClassTimeDto>();

        // Властивості для форми створення
        [BindProperty]
        public string NewClassTimeframe { get; set; } = string.Empty;

        // Властивості для форми редагування
        [BindProperty]
        public Guid EditedClassTimeId { get; set; }
        [BindProperty]
        public string EditedClassTimeframe { get; set; } = string.Empty;

        // Властивість для видалення
        [BindProperty]
        public Guid DeletedClassTimeId { get; set; }

        // Властивість для пошуку
        [BindProperty(SupportsGet = true)]
        public string? SearchById { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchByTimeframe { get; set; }

        [TempData]
        public string? Message { get; set; }
        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadClassTimesAsync();
            return Page();
        }

        private async Task LoadClassTimesAsync()
        {
            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                string apiUrl = "ClassTime";

                if (!string.IsNullOrWhiteSpace(SearchById) && Guid.TryParse(SearchById, out Guid parsedGuid))
                {
                    apiUrl = $"ClassTime/{parsedGuid}";
                }
                else if (!string.IsNullOrWhiteSpace(SearchByTimeframe))
                {
                    // Для пошуку за часовим проміжком, API очікує рядок
                    apiUrl = $"ClassTime/timeframe/{SearchByTimeframe}";
                }
                else
                {
                    // Наразі у вашому контролері ClassTimeController немає пагінації за замовчуванням
                    // Якщо потрібно, додайте GetClassTimes(pageNumber, pageSize) до контролера
                    // та відповідні параметри тут. Для простоти, викликаємо GetAll.
                }

                response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                if (content.TrimStart().StartsWith("{"))
                {
                    var singleClassTime = JsonSerializer.Deserialize<ClassTimeDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ClassTimes = singleClassTime != null ? new List<ClassTimeDto> { singleClassTime } : new List<ClassTimeDto>();
                }
                else
                {
                    ClassTimes = JsonSerializer.Deserialize<List<ClassTimeDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                   ?? new List<ClassTimeDto>();
                }

                if (!ClassTimes.Any() && string.IsNullOrEmpty(Message) && string.IsNullOrEmpty(ErrorMessage))
                {
                    Message = "Не знайдено жодного часу занять за заданими критеріями.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error during ClassTime load: {Message}", ex.Message);
                ErrorMessage = $"Помилка завантаження часу занять: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
                ClassTimes = new List<ClassTimeDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error during ClassTime load: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних часу занять: {ex.Message}";
                ClassTimes = new List<ClassTimeDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during ClassTime load: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
                ClassTimes = new List<ClassTimeDto>();
            }
        }

        // Допоміжний метод для валідації діапазону часу
        private bool ValidateTimeRange(string timeRangeString, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(timeRangeString))
            {
                ModelState.AddModelError(fieldName, "Часовий проміжок не може бути порожнім.");
                return false;
            }

            var parts = timeRangeString.Split('-').Select(p => p.Trim()).ToArray();
            if (parts.Length != 2)
            {
                ModelState.AddModelError(fieldName, "Формат часу має бути 'ГГ:ХХ - ГГ:ХХ'.");
                return false;
            }

            if (!TimeSpan.TryParseExact(parts[0], "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan startTime) ||
                !TimeSpan.TryParseExact(parts[1], "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan endTime))
            {
                ModelState.AddModelError(fieldName, "Некоректний формат часу. Використовуйте ГГ:ХХ (наприклад, 08:00).");
                return false;
            }

            if (startTime >= endTime)
            {
                ModelState.AddModelError(fieldName, "Час початку має бути раніше часу закінчення.");
                return false;
            }

            TimeSpan minAllowedTime = new TimeSpan(6, 0, 0); // 06:00
            TimeSpan maxAllowedTime = new TimeSpan(20, 0, 0); // 20:00

            if (startTime < minAllowedTime || endTime > maxAllowedTime)
            {
                ModelState.AddModelError(fieldName, "Обидва часи проміжку мають бути в діапазоні від 06:00 до 20:00.");
                return false;
            }

            return true;
        }

        public async Task<IActionResult> OnPostCreateClassTimeAsync()
        {
            if (!ValidateTimeRange(NewClassTimeframe, nameof(NewClassTimeframe)))
            {
                await LoadClassTimesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                var requestBody = new
                {
                    TimeFrame = NewClassTimeframe // Надсилаємо як string
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PostAsync("ClassTime", content);
                response.EnsureSuccessStatusCode();

                Message = "Час заняття успішно додано!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"Помилка при створенні часу заняття: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error creating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditClassTimeAsync()
        {
            if (EditedClassTimeId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID часу заняття не може бути порожнім.");
                await LoadClassTimesAsync();
                return Page();
            }

            if (!ValidateTimeRange(EditedClassTimeframe, nameof(EditedClassTimeframe)))
            {
                await LoadClassTimesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                var requestBody = new
                {
                    TimeFrame = EditedClassTimeframe // Надсилаємо як string
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PutAsync($"ClassTime/{EditedClassTimeId}", content);
                response.EnsureSuccessStatusCode();

                Message = "Час заняття успішно оновлено!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error updating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"Помилка при оновленні часу заняття: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error updating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteClassTimeAsync()
        {
            if (DeletedClassTimeId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID часу заняття для видалення не може бути порожнім.");
                await LoadClassTimesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                response = await client.DeleteAsync($"ClassTime/{DeletedClassTimeId}");
                response.EnsureSuccessStatusCode();

                Message = "Час заняття успішно видалено!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error deleting ClassTime: {Message}", ex.Message);
                ErrorMessage = $"Помилка при видаленні часу заняття: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting ClassTime: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}