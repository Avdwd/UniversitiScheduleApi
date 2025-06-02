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
        [BindProperty] // ������ ������� BindProperty ��� ������������� ����'�������� � �����
        public TypeSubjectDto NewTypeSubject { get; set; } = new TypeSubjectDto();
        // ������������� TypeSubjectDto ��� ������
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
                // ����������� ����� � List<TypeSubjectDto>
                TypeSubjects = JsonSerializer.Deserialize<List<TypeSubjectDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                Message = $"������� ������������ ���� ��������: {response.StatusCode}";
            }
        }

        public async Task<IActionResult> OnPostCreateAsync() // �������, ��� ��������������� NewTypeSubject
        {
            if (string.IsNullOrEmpty(NewTypeSubject.Type))
            {
                Message = "���� �����, �������� �� ����'����� ����.";
                await LoadTypeSubjects();
                return Page();
            }

            // ����� ������������� NewTypeSubject.Type ��� ���������
            var requestPayload = new { type = NewTypeSubject.Type };

            var httpClient = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestPayload),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(_apiBaseUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                Message = "��� �������� ������ ��������!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Message = $"������� ��������� ���� ��������: {response.StatusCode} - {errorContent}";
            }

            return RedirectToPage();
        }


        // ������� ��� ��������� �� TypeSubjectDto ��� ���������
        public async Task<IActionResult> OnPostUpdateAsync([FromForm] TypeSubjectDto typeSubjectDto)
        {
            // ���������, ��� ���������, API ����� TypeSubjectRequest.
            var requestPayload = new { type = typeSubjectDto.Type };

            if (string.IsNullOrEmpty(typeSubjectDto.Type))
            {
                Message = "����� ���� �� ���� ���� ���������.";
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
                Message = "��� �������� ������ ��������!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Message = $"������� ��������� ���� ��������: {response.StatusCode} - {errorContent}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid IdToDelete)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.DeleteAsync($"{_apiBaseUrl}/{IdToDelete}");

            if (response.IsSuccessStatusCode)
            {
                Message = "��� �������� ������ ��������!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Message = $"������� ��������� ���� ��������: {response.StatusCode} - {errorContent}";
            }

            return RedirectToPage();
        }
    }
}

