using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // ��� SelectList
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
                ModelState.AddModelError(string.Empty, "����� ����� �� �������� �� ������ ���� ��������.");
                await LoadGroupsAndInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null; // ��������� response ���

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

                response = await client.PostAsync("Group", content); // ���������� response
                response.EnsureSuccessStatusCode();

                Message = "����� ������ ������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "������� ��� �������� �����: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� �����: {ex.Message}";
                // ����� response ���������, � �� ������ ������ ���� ����
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "������� ������������ JSON ��� �������� �����: {Message}", ex.Message);
                ErrorMessage = $"������� ������� �����: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������� ����������� ������� ��� �������� �����: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditGroupAsync()
        {
            if (EditedGroupId == Guid.Empty || string.IsNullOrWhiteSpace(EditedGroupName) || EditedGroupInstituteId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID �����, ����� �� �������� �� ������ ���� ��������.");
                await LoadGroupsAndInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null; // ��������� response ���

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

                response = await client.PutAsync($"Group/{EditedGroupId}", content); // ���������� response
                response.EnsureSuccessStatusCode();

                Message = "����� ������ ��������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "������� ��� �������� �����: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� �����: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "������� ������������ JSON ��� �������� �����: {Message}", ex.Message);
                ErrorMessage = $"������� ������� �����: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������� ����������� ������� ��� �������� �����: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteGroupAsync()
        {
            if (DeletedGroupId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID ����� ��� ��������� �� ���� ���� �������.");
                await LoadGroupsAndInstitutesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null; // ��������� response ���

            try
            {
                response = await client.DeleteAsync($"Group/{DeletedGroupId}"); // ���������� response
                response.EnsureSuccessStatusCode();

                Message = "����� ������ ��������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "������� ��� �������� �����: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� �����: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������� ����������� ������� ��� �������� �����: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }

        private async Task LoadGroupsAndInstitutesAsync()
        {
            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage groupResponse = null;     // ��������� ���
            HttpResponseMessage instituteResponse = null; // ��������� ���

            // ������������ ����
            try
            {
                groupResponse = await client.GetAsync("Group"); // ���������� ����
                groupResponse.EnsureSuccessStatusCode();
                var groupContent = await groupResponse.Content.ReadAsStringAsync();
                Groups = JsonSerializer.Deserialize<List<GroupDto>>(groupContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                         ?? new List<GroupDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "�� ������� ����������� ����� � API: {Message}", ex.Message);
                ErrorMessage = $"�� ������� ����������� �����: {ex.Message}";
                if (groupResponse != null) // ����������, �� � response
                {
                    var errorDetails = await groupResponse.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)groupResponse.StatusCode}, �����: {errorDetails})";
                }
                Groups = new List<GroupDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "������� ������� JSON ��� ����������� ����: {Message}", ex.Message);
                ErrorMessage = $"������� ������� ����� ����: {ex.Message}";
                Groups = new List<GroupDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������� ����������� ������� ��� ����������� ����: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
                Groups = new List<GroupDto>();
            }

            // ������������ ��������� ��� ����������� ������
            try
            {
                instituteResponse = await client.GetAsync("api/Institute"); // ���������� ����
                instituteResponse.EnsureSuccessStatusCode();
                var instituteContent = await instituteResponse.Content.ReadAsStringAsync();
                var institutes = JsonSerializer.Deserialize<List<InstituteDto>>(instituteContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                 ?? new List<InstituteDto>();
                InstitutesSelectList = new SelectList(institutes, "Id", "Name");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "�� ������� ����������� ��������� ��� ������: {Message}", ex.Message);
                if (instituteResponse != null) // ����������, �� � response
                {
                    var errorDetails = await instituteResponse.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)instituteResponse.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������� ����������� ������� ��� ����������� ��������� ��� ������: {Message}", ex.Message);
            }
        }
    }
}