using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging; // Додано для логування
using UniversitySchedule.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq; // Додано для .Any()
using System.Threading.Tasks;

namespace UniversitySchedule.UI.Pages.Admin.TeacherProfile
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger; // Додано логер

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger) // Впровадження ILogger
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // Список профілів викладачів для відображення
        public IList<TeacherProfileDto> TeacherProfiles { get; set; } = new List<TeacherProfileDto>();

        // Властивості для форми створення/редагування
        [BindProperty]
        public TeacherProfileDto InputTeacherProfile { get; set; } = new TeacherProfileDto();

        // Додано для випадаючого списку інститутів
        public IList<InstituteDto> Institutes { get; set; } = new List<InstituteDto>();

        // Властивості для пошуку
        [BindProperty(SupportsGet = true)]
        public string? SearchTeacherId { get; set; } // Пошук за ID
        [BindProperty(SupportsGet = true)]
        public string? SearchTeacherName { get; set; } // Пошук за ПІБ/UserName (приклад, якщо API підтримує)

        // Результат пошуку єдиного викладача (для відображення окремо)
        public TeacherProfileDto? SingleTeacherProfile { get; set; }

        // Повідомлення користувачу (успіх/помилка)
        [TempData]
        public string? Message { get; set; } // Для повідомлень про успіх
        [TempData]
        public string? ErrorMessage { get; set; } // Для повідомлень про помилки

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadTeacherProfilesAsync(); // Завантажуємо всі профілі при першому завантаженні сторінки
            await LoadInstitutesAsync(); // Завантажуємо інститути
            return Page();
        }

        private async Task LoadTeacherProfilesAsync()
        {
            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage? response = null; // Зроблено nullable, щоб уникнути помилок, якщо виняток виникає до присвоєння

            try
            {
                string apiUrl = "TeacherProfile"; // Базовий URL для отримання всіх

                if (!string.IsNullOrWhiteSpace(SearchTeacherId) && Guid.TryParse(SearchTeacherId, out Guid parsedId))
                {
                    apiUrl = $"TeacherProfile/{parsedId}"; // Якщо шукаємо за ID
                }
                else if (!string.IsNullOrWhiteSpace(SearchTeacherName))
                {
                    // Це ПРИКЛАД: якщо ваш API підтримує пошук за ім'ям,
                    // ви повинні налаштувати цей маршрут у вашому API-контролері
                    // Наприклад: [HttpGet("search-by-name/{name}")]
                    apiUrl = $"TeacherProfile/search-by-name/{Uri.EscapeDataString(SearchTeacherName)}";
                }
                // Наразі для TeacherProfile не передбачено пагінації, тому просто отримуємо всіх, якщо пошуку немає.
                // Якщо API підтримує пагінацію, тут може бути: apiUrl = $"TeacherProfile/page/{PageNumber}/{PageSize}";

                response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Викине HttpRequestException, якщо статус-код не 2xx

                var content = await response.Content.ReadAsStringAsync();

                // Якщо пошук за ID/Name, то очікуємо один об'єкт, інакше - список.
                // Перевіряємо, чи починається відповідь з '{' (один об'єкт) або '[' (масив об'єктів).
                if (content.TrimStart().StartsWith("{") && (!string.IsNullOrWhiteSpace(SearchTeacherId) || !string.IsNullOrWhiteSpace(SearchTeacherName)))
                {
                    SingleTeacherProfile = JsonSerializer.Deserialize<TeacherProfileDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    TeacherProfiles = SingleTeacherProfile != null ? new List<TeacherProfileDto> { SingleTeacherProfile } : new List<TeacherProfileDto>();
                }
                else
                {
                    TeacherProfiles = JsonSerializer.Deserialize<List<TeacherProfileDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                      ?? new List<TeacherProfileDto>();
                    SingleTeacherProfile = null; // Скидаємо одиночний профіль, якщо відображаємо весь список
                }

                if (!TeacherProfiles.Any() && string.IsNullOrEmpty(Message) && string.IsNullOrEmpty(ErrorMessage))
                {
                    Message = "Не знайдено жодного профілю викладача за заданими критеріями.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error during TeacherProfile load: {Message}", ex.Message);
                ErrorMessage = $"Помилка завантаження профілів викладачів: {ex.Message}";
                if (response != null && response.Content != null) // Перевірка на null
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
                TeacherProfiles = new List<TeacherProfileDto>();
                SingleTeacherProfile = null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error during TeacherProfile load: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних профілів викладачів: {ex.Message}";
                TeacherProfiles = new List<TeacherProfileDto>();
                SingleTeacherProfile = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during TeacherProfile load: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка: {ex.Message}";
                TeacherProfiles = new List<TeacherProfileDto>();
                SingleTeacherProfile = null;
            }
        }

        private async Task LoadInstitutesAsync() // Перейменував на LoadInstitutesAsync для послідовності
        {
            var client = _httpClientFactory.CreateClient("UniversityApi");
            try
            {
                var response = await client.GetAsync("api/Institute"); // Ваш API-маршрут для інститутів
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Institutes = JsonSerializer.Deserialize<List<InstituteDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                             ?? new List<InstituteDto>(); // Додано ?? new List<InstituteDto>()
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error during Institute load: {Message}", ex.Message);
                // Це не є критичною помилкою для відображення профілів, але варто її зафіксувати.
                // Можливо, ви захочете відобразити цю помилку у UI, але це може перевантажити TempData
                // ErrorMessage = $"Не вдалося завантажити інститути: {ex.Message}";
                Institutes = new List<InstituteDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error during Institute load: {Message}", ex.Message);
                // ErrorMessage = $"Помилка обробки даних інститутів: {ex.Message}";
                Institutes = new List<InstituteDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Institute load: {Message}", ex.Message);
                // ErrorMessage = $"Виникла неочікувана помилка при завантаженні інститутів: {ex.Message}";
                Institutes = new List<InstituteDto>();
            }
        }

        public async Task<IActionResult> OnPostSearchAsync() // Змінено назву для універсальності (може шукати за ID або Name)
        {
            // Після будь-якого пошуку, перезавантажуємо дані з урахуванням критеріїв пошуку
            await LoadTeacherProfilesAsync();
            await LoadInstitutesAsync(); // Перезавантажуємо інститути, якщо потрібно
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Будь ласка, перевірте введені дані для нового профілю.";
                await LoadTeacherProfilesAsync();
                await LoadInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage? response = null;

            try
            {
                // Серіалізуємо InputTeacherProfile напряму.
                var content = new StringContent(
                    JsonSerializer.Serialize(InputTeacherProfile),
                    Encoding.UTF8,
                    "application/json"
                );

                _logger.LogInformation("Sending JSON for creation: {Json}", await content.ReadAsStringAsync());

                // ****** КЛЮЧОВЕ ВИПРАВЛЕННЯ ТУТ: ВИКОРИСТОВУЄМО POST ASYNC ******
                // Для створення нового ресурсу завжди використовується POST.
                // URL зазвичай не включає ID, оскільки ресурс ще не існує.
                response = await client.PostAsync("TeacherProfile", content);
                response.EnsureSuccessStatusCode();

                // ****** ПОВІДОМЛЕННЯ ТУТ ******
                Message = "Профіль викладача успішно створено!"; // Правильне повідомлення
                InputTeacherProfile = new TeacherProfileDto(); // Очищаємо форму
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error creating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"Помилка при створенні профілю викладача: {ex.Message}";
                if (response != null && response.Content != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                    _logger.LogError("API Error Response: {ErrorDetails}", errorDetails);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error creating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних при створенні профілю: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка при створенні профілю: {ex.Message}";
            }

            return RedirectToPage(); // Перенаправляємо на OnGetAsync для оновлення списку
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            // Для редагування, ми повинні мати ID та всі необхідні поля.
            // Припустимо, що InputTeacherProfile використовується для обох: створення та редагування
            // Якщо ви маєте окрему модальну форму для редагування, вам може знадобитися окремий BindProperty.
            if (string.IsNullOrEmpty(InputTeacherProfile.Id) || string.IsNullOrWhiteSpace(InputTeacherProfile.FirstName) || string.IsNullOrWhiteSpace(InputTeacherProfile.LastName))
            {
                ModelState.AddModelError(string.Empty, "ID, ім'я та прізвище викладача не можуть бути порожніми для редагування.");
                await LoadTeacherProfilesAsync();
                await LoadInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage? response = null;

            try
            {
                // API може очікувати інший DTO для оновлення (наприклад, TeacherProfileUpdateRequest)
                var updateRequest = new
                {
                    id = InputTeacherProfile.Id, // ID, який потрібно оновити
                    userName = InputTeacherProfile.UserName,
                    firstName = InputTeacherProfile.FirstName,
                    lastName = InputTeacherProfile.LastName,
                    middleName = InputTeacherProfile.MiddleName,
                    instituteId = InputTeacherProfile.Institute.Id
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(updateRequest),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PutAsync($"TeacherProfile/{InputTeacherProfile.Id}", content); // PUT до конкретного ID
                response.EnsureSuccessStatusCode();

                Message = "Профіль викладача успішно оновлено!";
                // Очищаємо поля форми після успішного оновлення
                InputTeacherProfile = new TeacherProfileDto();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error updating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"Помилка при оновленні профілю викладача: {ex.Message}";
                if (response != null && response.Content != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error updating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"Помилка обробки даних при оновленні профілю: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка при оновленні профілю: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid parsedId))
            {
                ModelState.AddModelError(string.Empty, "Недійсний ID для видалення.");
                await LoadTeacherProfilesAsync();
                await LoadInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage? response = null;

            try
            {
                response = await client.DeleteAsync($"TeacherProfile/{parsedId}");
                response.EnsureSuccessStatusCode();

                Message = "Профіль викладача успішно видалено!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error deleting TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"Помилка при видаленні профілю викладача: {ex.Message}";
                if (response != null && response.Content != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (Статус: {(int)response.StatusCode}, Деталі: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"Виникла неочікувана помилка при видаленні профілю: {ex.Message}";
            }

            return RedirectToPage();
        }
    }

    
   
}