using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http; // ������ ��� HttpClient
using Microsoft.Extensions.Configuration;
using UniversitySchedule.UI.Models; // ������ ��� IConfiguration

namespace UniversitySchedule.UI.Pages.Admin.Institutes
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        // _configuration ����� �� ������� � �������, ���� BaseAddress �����������,
        // ��� ���� ����� ��������, ���� ������� ��� ����� ����� ��� ������������
        private readonly IConfiguration _configuration;

        public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public List<InstituteDto> Institutes { get; set; } = new List<InstituteDto>();

        // DTO (Data Transfer Object) ��� ������������� ���������, �� �� ���������/������������� � API
        // �� �� ��������� InstituteResponse/InstituteRequest � ������ API-�������
        

        // ���������� ��� ������ ��������� (��� ���������)
        [BindProperty] // SupportsGet = true �� �������, ���� ��������������� ����� ��� Post
        public string NewInstituteName { get; set; }

        // ���������� ��� ����������� ���������
        [BindProperty]
        public Guid EditedInstituteId { get; set; }
        [BindProperty]
        public string EditedInstituteName { get; set; }

        // ���������� ��� ��������� ���������
        [BindProperty]
        public Guid DeletedInstituteId { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            await LoadInstitutes();
            return Page();
        }

        // ������� ��� ��������� ���������
        public async Task<IActionResult> OnPostCreateInstituteAsync()
        {
            if (string.IsNullOrWhiteSpace(NewInstituteName))
            {
                ModelState.AddModelError(string.Empty, "����� ��������� �� ���� ���� ���������.");
                await LoadInstitutes();
                return Page();
            }

            // �������� ���������� ��������� HttpClient
            var client = _httpClientFactory.CreateClient("UniversityApi");

            var content = new StringContent(
                JsonSerializer.Serialize(new { Name = NewInstituteName }),
                Encoding.UTF8,
                "application/json"
            );

            // ������������� ²������� URL, ������� BaseAddress ��� �����������
            var response = await client.PostAsync("api/Institute", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "�������� ������ ������!";
                return RedirectToPage();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"������� ��� �������� ���������: {response.StatusCode} - {errorContent}");
            }

            await LoadInstitutes();
            return Page();
        }

        // ������� ��� ����������� ���������
        public async Task<IActionResult> OnPostEditInstituteAsync()
        {
            if (string.IsNullOrWhiteSpace(EditedInstituteName))
            {
                ModelState.AddModelError(string.Empty, "����� ��������� �� ���� ���� ���������.");
                await LoadInstitutes();
                return Page();
            }

            // �������� ���������� ��������� HttpClient
            var client = _httpClientFactory.CreateClient("UniversityApi");

            var content = new StringContent(
                JsonSerializer.Serialize(new { Id = EditedInstituteId, Name = EditedInstituteName }),
                Encoding.UTF8,
                "application/json"
            );

            // ������������� ²������� URL
            var response = await client.PutAsync($"api/Institute/{EditedInstituteId}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "�������� ������ ��������!";
                return RedirectToPage();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"������� ��� �������� ���������: {response.StatusCode} - {errorContent}");
            }

            await LoadInstitutes();
            return Page();
        }

        // ������� ��� ��������� ���������
        public async Task<IActionResult> OnPostDeleteInstituteAsync()
        {
            // �������� ���������� ��������� HttpClient
            var client = _httpClientFactory.CreateClient("UniversityApi");

            // ������������� ²������� URL
            var response = await client.DeleteAsync($"api/Institute/{DeletedInstituteId}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "�������� ������ ��������!";
                return RedirectToPage();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"������� ��� �������� ���������: {response.StatusCode} - {errorContent}");
            }

            await LoadInstitutes();
            return Page();
        }


        // ��������� ����� ��� ������������ ���������
        private async Task LoadInstitutes()
        {
            try
            {
                // �������� ���������� ��������� HttpClient
                var client = _httpClientFactory.CreateClient("UniversityApi");

                // ������������� ²������� URL
                var response = await client.GetAsync("api/Institute");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Institutes = JsonSerializer.Deserialize<List<InstituteDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"�� ������� ����������� ���������: {ex.Message}");
                Institutes = new List<InstituteDto>();
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, $"������� ������� ����� ���������: {ex.Message}");
                Institutes = new List<InstituteDto>();
            }
            catch (Exception ex) // �������� ������� �������� �������
            {
                ModelState.AddModelError(string.Empty, $"������� ����������� �������: {ex.Message}");
                Institutes = new List<InstituteDto>();
            }
        }
    }
}