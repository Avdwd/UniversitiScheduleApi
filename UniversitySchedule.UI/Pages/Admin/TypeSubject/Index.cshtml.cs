using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using UniversitySchedule.UI.Models;


namespace UniversitySchedule.UI.Pages.Admin.TypeSubject
{



    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl;

        public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"] + "/TypeSubject";
        }
        [BindProperty] // Додаємо атрибут BindProperty для автоматичного прив'язування з форми
        public TypeSubjectDto NewTypeSubject { get; set; } = new TypeSubjectDto();
        // Використовуємо TypeSubjectDto для списку
        public List<TypeSubjectDto> TypeSubjects { get; set; } = new List<TypeSubjectDto>();

        [TempData]
        public string Message { get; set; }

        public async Task OnGetAsync()
        {
            await LoadTypeSubjects();
        }

        private async Task LoadTypeSubjects()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(_apiBaseUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                // Десеріалізуємо прямо в List<TypeSubjectDto>
                TypeSubjects = JsonSerializer.Deserialize<List<TypeSubjectDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                Message = $"Помилка завантаження типів предметів: {response.StatusCode}";
            }
        }

        public async Task<IActionResult> OnPostCreateAsync() // Змінюємо, щоб використовувати NewTypeSubject
        {
            if (string.IsNullOrEmpty(NewTypeSubject.Type))
            {
                Message = "Будь ласка, заповніть усі обов'язкові поля.";
                await LoadTypeSubjects();
                return Page();
            }

            // Тепер використовуємо NewTypeSubject.Type для створення
            var requestPayload = new { type = NewTypeSubject.Type };

            var httpClient = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestPayload),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(_apiBaseUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                Message = "Тип предмету успішно створено!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Message = $"Помилка створення типу предмету: {response.StatusCode} - {errorContent}";
            }

            return RedirectToPage();
        }


        // Змінюємо тип параметру на TypeSubjectDto для оновлення
        public async Task<IActionResult> OnPostUpdateAsync([FromForm] TypeSubjectDto typeSubjectDto)
        {
            // Аналогічно, для оновлення, API очікує TypeSubjectRequest.
            var requestPayload = new { type = typeSubjectDto.Type };

            if (string.IsNullOrEmpty(typeSubjectDto.Type))
            {
                Message = "Назва типу не може бути порожньою.";
                await LoadTypeSubjects();
                return Page();
            }

            var httpClient = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestPayload),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PutAsync($"{_apiBaseUrl}/{typeSubjectDto.Id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                Message = "Тип предмету успішно оновлено!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Message = $"Помилка оновлення типу предмету: {response.StatusCode} - {errorContent}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid IdToDelete)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.DeleteAsync($"{_apiBaseUrl}/{IdToDelete}");

            if (response.IsSuccessStatusCode)
            {
                Message = "Тип предмету успішно видалено!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Message = $"Помилка видалення типу предмету: {response.StatusCode} - {errorContent}";
            }

            return RedirectToPage();
        }
    }
}

