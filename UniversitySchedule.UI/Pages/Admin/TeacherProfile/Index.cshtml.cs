using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging; // ������ ��� ���������
using UniversitySchedule.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq; // ������ ��� .Any()
using System.Threading.Tasks;

namespace UniversitySchedule.UI.Pages.Admin.TeacherProfile
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger; // ������ �����

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger) // ������������ ILogger
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // ������ ������� ���������� ��� �����������
        public IList<TeacherProfileDto> TeacherProfiles { get; set; } = new List<TeacherProfileDto>();

        // ���������� ��� ����� ���������/�����������
        [BindProperty]
        public TeacherProfileDto InputTeacherProfile { get; set; } = new TeacherProfileDto();

        // ������ ��� ����������� ������ ���������
        public IList<InstituteDto> Institutes { get; set; } = new List<InstituteDto>();

        // ���������� ��� ������
        [BindProperty(SupportsGet = true)]
        public string? SearchTeacherId { get; set; } // ����� �� ID
        [BindProperty(SupportsGet = true)]
        public string? SearchTeacherName { get; set; } // ����� �� ϲ�/UserName (�������, ���� API �������)

        // ��������� ������ ������� ��������� (��� ����������� ������)
        public TeacherProfileDto? SingleTeacherProfile { get; set; }

        // ����������� ����������� (����/�������)
        [TempData]
        public string? Message { get; set; } // ��� ���������� ��� ����
        [TempData]
        public string? ErrorMessage { get; set; } // ��� ���������� ��� �������

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadTeacherProfilesAsync(); // ����������� �� ������ ��� ������� ����������� �������
            await LoadInstitutesAsync(); // ����������� ���������
            return Page();
        }

        private async Task LoadTeacherProfilesAsync()
        {
            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage? response = null; // �������� nullable, ��� �������� �������, ���� ������� ������ �� ���������

            try
            {
                string apiUrl = "TeacherProfile"; // ������� URL ��� ��������� ���

                if (!string.IsNullOrWhiteSpace(SearchTeacherId) && Guid.TryParse(SearchTeacherId, out Guid parsedId))
                {
                    apiUrl = $"TeacherProfile/{parsedId}"; // ���� ������ �� ID
                }
                else if (!string.IsNullOrWhiteSpace(SearchTeacherName))
                {
                    // �� �������: ���� ��� API ������� ����� �� ��'��,
                    // �� ������ ����������� ��� ������� � ������ API-���������
                    // ���������: [HttpGet("search-by-name/{name}")]
                    apiUrl = $"TeacherProfile/search-by-name/{Uri.EscapeDataString(SearchTeacherName)}";
                }
                // ����� ��� TeacherProfile �� ����������� ��������, ���� ������ �������� ���, ���� ������ ����.
                // ���� API ������� ��������, ��� ���� ����: apiUrl = $"TeacherProfile/page/{PageNumber}/{PageSize}";

                response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // ������ HttpRequestException, ���� ������-��� �� 2xx

                var content = await response.Content.ReadAsStringAsync();

                // ���� ����� �� ID/Name, �� ������� ���� ��'���, ������ - ������.
                // ����������, �� ���������� ������� � '{' (���� ��'���) ��� '[' (����� ��'����).
                if (content.TrimStart().StartsWith("{") && (!string.IsNullOrWhiteSpace(SearchTeacherId) || !string.IsNullOrWhiteSpace(SearchTeacherName)))
                {
                    SingleTeacherProfile = JsonSerializer.Deserialize<TeacherProfileDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    TeacherProfiles = SingleTeacherProfile != null ? new List<TeacherProfileDto> { SingleTeacherProfile } : new List<TeacherProfileDto>();
                }
                else
                {
                    TeacherProfiles = JsonSerializer.Deserialize<List<TeacherProfileDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                      ?? new List<TeacherProfileDto>();
                    SingleTeacherProfile = null; // ������� ��������� �������, ���� ���������� ���� ������
                }

                if (!TeacherProfiles.Any() && string.IsNullOrEmpty(Message) && string.IsNullOrEmpty(ErrorMessage))
                {
                    Message = "�� �������� ������� ������� ��������� �� �������� ���������.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error during TeacherProfile load: {Message}", ex.Message);
                ErrorMessage = $"������� ������������ ������� ����������: {ex.Message}";
                if (response != null && response.Content != null) // �������� �� null
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
                TeacherProfiles = new List<TeacherProfileDto>();
                SingleTeacherProfile = null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error during TeacherProfile load: {Message}", ex.Message);
                ErrorMessage = $"������� ������� ����� ������� ����������: {ex.Message}";
                TeacherProfiles = new List<TeacherProfileDto>();
                SingleTeacherProfile = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during TeacherProfile load: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
                TeacherProfiles = new List<TeacherProfileDto>();
                SingleTeacherProfile = null;
            }
        }

        private async Task LoadInstitutesAsync() // ������������ �� LoadInstitutesAsync ��� �����������
        {
            var client = _httpClientFactory.CreateClient("UniversityApi");
            try
            {
                var response = await client.GetAsync("api/Institute"); // ��� API-������� ��� ���������
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Institutes = JsonSerializer.Deserialize<List<InstituteDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                             ?? new List<InstituteDto>(); // ������ ?? new List<InstituteDto>()
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error during Institute load: {Message}", ex.Message);
                // �� �� � ��������� �������� ��� ����������� �������, ��� ����� �� �����������.
                // �������, �� �������� ���������� �� ������� � UI, ��� �� ���� ������������� TempData
                // ErrorMessage = $"�� ������� ����������� ���������: {ex.Message}";
                Institutes = new List<InstituteDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error during Institute load: {Message}", ex.Message);
                // ErrorMessage = $"������� ������� ����� ���������: {ex.Message}";
                Institutes = new List<InstituteDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Institute load: {Message}", ex.Message);
                // ErrorMessage = $"������� ����������� ������� ��� ����������� ���������: {ex.Message}";
                Institutes = new List<InstituteDto>();
            }
        }

        public async Task<IActionResult> OnPostSearchAsync() // ������ ����� ��� ������������� (���� ������ �� ID ��� Name)
        {
            // ϳ��� ����-����� ������, ��������������� ��� � ����������� ������� ������
            await LoadTeacherProfilesAsync();
            await LoadInstitutesAsync(); // ��������������� ���������, ���� �������
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "���� �����, �������� ������ ��� ��� ������ �������.";
                await LoadTeacherProfilesAsync();
                await LoadInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage? response = null;

            try
            {
                // ��������� InputTeacherProfile �������.
                var content = new StringContent(
                    JsonSerializer.Serialize(InputTeacherProfile),
                    Encoding.UTF8,
                    "application/json"
                );

                _logger.LogInformation("Sending JSON for creation: {Json}", await content.ReadAsStringAsync());

                // ****** ������� ����������� ���: ����������Ӫ�� POST ASYNC ******
                // ��� ��������� ������ ������� ������ ��������������� POST.
                // URL �������� �� ������ ID, ������� ������ �� �� ����.
                response = await client.PostAsync("TeacherProfile", content);
                response.EnsureSuccessStatusCode();

                // ****** ��²�������� ��� ******
                Message = "������� ��������� ������ ��������!"; // ��������� �����������
                InputTeacherProfile = new TeacherProfileDto(); // ������� �����
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error creating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� ������� ���������: {ex.Message}";
                if (response != null && response.Content != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                    _logger.LogError("API Error Response: {ErrorDetails}", errorDetails);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error creating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"������� ������� ����� ��� �������� �������: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� ������� ��� �������� �������: {ex.Message}";
            }

            return RedirectToPage(); // ��������������� �� OnGetAsync ��� ��������� ������
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            // ��� �����������, �� ������ ���� ID �� �� �������� ����.
            // ����������, �� InputTeacherProfile ��������������� ��� ����: ��������� �� �����������
            // ���� �� ���� ������ �������� ����� ��� �����������, ��� ���� ����������� ������� BindProperty.
            if (string.IsNullOrEmpty(InputTeacherProfile.Id) || string.IsNullOrWhiteSpace(InputTeacherProfile.FirstName) || string.IsNullOrWhiteSpace(InputTeacherProfile.LastName))
            {
                ModelState.AddModelError(string.Empty, "ID, ��'� �� ������� ��������� �� ������ ���� �������� ��� �����������.");
                await LoadTeacherProfilesAsync();
                await LoadInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage? response = null;

            try
            {
                // API ���� ��������� ����� DTO ��� ��������� (���������, TeacherProfileUpdateRequest)
                var updateRequest = new
                {
                    id = InputTeacherProfile.Id, // ID, ���� ������� �������
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

                response = await client.PutAsync($"TeacherProfile/{InputTeacherProfile.Id}", content); // PUT �� ����������� ID
                response.EnsureSuccessStatusCode();

                Message = "������� ��������� ������ ��������!";
                // ������� ���� ����� ���� �������� ���������
                InputTeacherProfile = new TeacherProfileDto();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error updating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� ������� ���������: {ex.Message}";
                if (response != null && response.Content != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error updating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"������� ������� ����� ��� �������� �������: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� ������� ��� �������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid parsedId))
            {
                ModelState.AddModelError(string.Empty, "�������� ID ��� ���������.");
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

                Message = "������� ��������� ������ ��������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error deleting TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� ������� ���������: {ex.Message}";
                if (response != null && response.Content != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting TeacherProfile: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� ������� ��� �������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }
    }

    
   
}