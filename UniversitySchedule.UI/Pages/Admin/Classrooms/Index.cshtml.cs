using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using UniversitySchedule.UI.Models;
using System; // Додано для Guid

namespace UniversitySchedule.UI.Pages.Admin.Classrooms
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

        // Список аудиторій для відображення
        public List<ClassroomDto> Classrooms { get; set; } = new List<ClassroomDto>();

        // Властивості для форми створення (зберігаємо string для введення, парсимо в int перед відправкою)
        [BindProperty]
        public string NewClassroomNumberString { get; set; } = string.Empty;
        [BindProperty]
        public string NewClassroomBuildingString { get; set; } = string.Empty;


        // Властивості для форми редагування (зберігаємо string для введення, парсимо в int перед відправкою)
        [BindProperty]
        public Guid EditedClassroomId { get; set; }
        [BindProperty]
        public string EditedClassroomNumberString { get; set; } = string.Empty;
        [BindProperty]
        public string EditedClassroomBuildingString { get; set; } = string.Empty;


        // Властивість для видалення
        [BindProperty]
        public Guid DeletedClassroomId { get; set; }


        // Властивості для пошуку та пагінації (зберігаємо string для введення, парсимо перед використанням)
        [BindProperty(SupportsGet = true)]
        public string? SearchById { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchByNumberString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchByBuildingString { get; set; }

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
            await LoadClassroomsAsync();
            return Page();
        }

        private async Task LoadClassroomsAsync()
        {
            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                string apiUrl = "Classroom"; // Базовий маршрут контролера

                if (!string.IsNullOrWhiteSpace(SearchById) && Guid.TryParse(SearchById, out Guid parsedGuid))
                {
                    apiUrl = $"Classroom/{parsedGuid}";
                }
                else if (!string.IsNullOrWhiteSpace(SearchByNumberString) && int.TryParse(SearchByNumberString, out int parsedNumber))
                {
                    apiUrl = $"Classroom/number/{parsedNumber}";
                }
                else if (!string.IsNullOrWhiteSpace(SearchByBuildingString) && int.TryParse(SearchByBuildingString, out int parsedBuilding))
                {
                    apiUrl = $"Classroom/building/{parsedBuilding}";
                }
                else
                {
                    apiUrl = $"Classroom/pagination?pageNumber={PageNumber}&pageSize={PageSize}";
                }

                response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                // Десеріалізація може бути як списком, так і єдиним об'єктом.
                if (content.TrimStart().StartsWith("{"))
                {
                    var singleClassroom = JsonSerializer.Deserialize<ClassroomDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Classrooms = singleClassroom != null ? new List<ClassroomDto> { singleClassroom } : new List<ClassroomDto>();
                }
                else // Припускаємо, що це список
                {
                    Classrooms = JsonSerializer.Deserialize<List<ClassroomDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                   ?? new List<ClassroomDto>();
                }

                if (!Classrooms.Any() && string.IsNullOrEmpty(Message) && string.IsNullOrEmpty(ErrorMessage))
                {
                    Message = "Не знайдено аудиторій за заданими критеріями.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Помилка HTTP при завантаженні аудиторій: {Message}", ex.Message);
                ErrorMessage = $"Помилка завантаження аудиторій: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
                Classrooms = new List<ClassroomDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Помилка десеріалізації JSON при завантаженні аудиторій: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних аудиторій: {ex.Message}";
                Classrooms = new List<ClassroomDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Виникла неочікувана помилка при завантаженні аудиторій: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
                Classrooms = new List<ClassroomDto>();
            }
        }

        public async Task<IActionResult> OnPostCreateClassroomAsync()
        {
            // Перевірка вхідних даних
            if (string.IsNullOrWhiteSpace(NewClassroomNumberString) || string.IsNullOrWhiteSpace(NewClassroomBuildingString))
            {
                ModelState.AddModelError(string.Empty, "Номер та корпус аудиторії не можуть бути порожніми.");
                await LoadClassroomsAsync();
                return Page();
            }

            if (!int.TryParse(NewClassroomNumberString, out int numberToCreate))
            {
                ModelState.AddModelError(string.Empty, "Номер аудиторії має бути цілим числом.");
                await LoadClassroomsAsync();
                return Page();
            }

            if (!int.TryParse(NewClassroomBuildingString, out int buildingToCreate))
            {
                ModelState.AddModelError(string.Empty, "Номер корпусу має бути цілим числом.");
                await LoadClassroomsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                // Створення анонімного об'єкта, що відповідає очікуванням API (ClassroomRequest)
                var requestBody = new
                {
                    Number = numberToCreate,
                    Building = buildingToCreate
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PostAsync("Classroom", content);
                response.EnsureSuccessStatusCode();

                Message = "Аудиторію успішно додано!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Помилка при створенні аудиторії: {Message}", ex.Message);
                ErrorMessage = $"Помилка при створенні аудиторії: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Помилка десеріалізації JSON при створенні аудиторії: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Виникла неочікувана помилка при створенні аудиторії: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage(); // Перенаправляємо, щоб оновити список
        }

        public async Task<IActionResult> OnPostEditClassroomAsync()
        {
            // Перевірка вхідних даних
            if (EditedClassroomId == Guid.Empty || string.IsNullOrWhiteSpace(EditedClassroomNumberString) || string.IsNullOrWhiteSpace(EditedClassroomBuildingString))
            {
                ModelState.AddModelError(string.Empty, "ID, номер та корпус аудиторії не можуть бути порожніми.");
                await LoadClassroomsAsync();
                return Page();
            }

            if (!int.TryParse(EditedClassroomNumberString, out int numberToEdit))
            {
                ModelState.AddModelError(string.Empty, "Номер аудиторії має бути цілим числом.");
                await LoadClassroomsAsync();
                return Page();
            }

            if (!int.TryParse(EditedClassroomBuildingString, out int buildingToEdit))
            {
                ModelState.AddModelError(string.Empty, "Номер корпусу має бути цілим числом.");
                await LoadClassroomsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                // Створення анонімного об'єкта, що відповідає очікуванням API (ClassroomRequest)
                var requestBody = new
                {
                    Number = numberToEdit,
                    Building = buildingToEdit
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PutAsync($"Classroom/{EditedClassroomId}", content);
                response.EnsureSuccessStatusCode();

                Message = "Аудиторію успішно оновлено!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Помилка при оновленні аудиторії: {Message}", ex.Message);
                ErrorMessage = $"Помилка при оновленні аудиторії: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Помилка десеріалізації JSON при оновленні аудиторії: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Виникла неочікувана помилка при оновленні аудиторії: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteClassroomAsync()
        {
            if (DeletedClassroomId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID аудиторії для видалення не може бути порожнім.");
                await LoadClassroomsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                response = await client.DeleteAsync($"Classroom/{DeletedClassroomId}");
                response.EnsureSuccessStatusCode();

                Message = "Аудиторію успішно видалено!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Помилка при видаленні аудиторії: {Message}", ex.Message);
                ErrorMessage = $"Помилка при видаленні аудиторії: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Виникла неочікувана помилка при видаленні аудиторії: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}