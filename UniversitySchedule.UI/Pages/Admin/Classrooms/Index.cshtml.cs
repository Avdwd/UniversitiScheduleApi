using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using UniversitySchedule.UI.Models;
using System; // ������ ��� Guid

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

        // ������ �������� ��� �����������
        public List<ClassroomDto> Classrooms { get; set; } = new List<ClassroomDto>();

        // ���������� ��� ����� ��������� (�������� string ��� ��������, ������� � int ����� ���������)
        [BindProperty]
        public string NewClassroomNumberString { get; set; } = string.Empty;
        [BindProperty]
        public string NewClassroomBuildingString { get; set; } = string.Empty;


        // ���������� ��� ����� ����������� (�������� string ��� ��������, ������� � int ����� ���������)
        [BindProperty]
        public Guid EditedClassroomId { get; set; }
        [BindProperty]
        public string EditedClassroomNumberString { get; set; } = string.Empty;
        [BindProperty]
        public string EditedClassroomBuildingString { get; set; } = string.Empty;


        // ���������� ��� ���������
        [BindProperty]
        public Guid DeletedClassroomId { get; set; }


        // ���������� ��� ������ �� �������� (�������� string ��� ��������, ������� ����� �������������)
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


        // ����������� �����������
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
                string apiUrl = "Classroom"; // ������� ������� ����������

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
                // ������������ ���� ���� �� �������, ��� � ������ ��'�����.
                if (content.TrimStart().StartsWith("{"))
                {
                    var singleClassroom = JsonSerializer.Deserialize<ClassroomDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Classrooms = singleClassroom != null ? new List<ClassroomDto> { singleClassroom } : new List<ClassroomDto>();
                }
                else // ����������, �� �� ������
                {
                    Classrooms = JsonSerializer.Deserialize<List<ClassroomDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                   ?? new List<ClassroomDto>();
                }

                if (!Classrooms.Any() && string.IsNullOrEmpty(Message) && string.IsNullOrEmpty(ErrorMessage))
                {
                    Message = "�� �������� �������� �� �������� ���������.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "������� HTTP ��� ����������� ��������: {Message}", ex.Message);
                ErrorMessage = $"������� ������������ ��������: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
                Classrooms = new List<ClassroomDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "������� ������������ JSON ��� ����������� ��������: {Message}", ex.Message);
                ErrorMessage = $"������� ������� ����� ��������: {ex.Message}";
                Classrooms = new List<ClassroomDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������� ����������� ������� ��� ����������� ��������: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
                Classrooms = new List<ClassroomDto>();
            }
        }

        public async Task<IActionResult> OnPostCreateClassroomAsync()
        {
            // �������� ������� �����
            if (string.IsNullOrWhiteSpace(NewClassroomNumberString) || string.IsNullOrWhiteSpace(NewClassroomBuildingString))
            {
                ModelState.AddModelError(string.Empty, "����� �� ������ ������� �� ������ ���� ��������.");
                await LoadClassroomsAsync();
                return Page();
            }

            if (!int.TryParse(NewClassroomNumberString, out int numberToCreate))
            {
                ModelState.AddModelError(string.Empty, "����� ������� �� ���� ����� ������.");
                await LoadClassroomsAsync();
                return Page();
            }

            if (!int.TryParse(NewClassroomBuildingString, out int buildingToCreate))
            {
                ModelState.AddModelError(string.Empty, "����� ������� �� ���� ����� ������.");
                await LoadClassroomsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                // ��������� ��������� ��'����, �� ������� ����������� API (ClassroomRequest)
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

                Message = "�������� ������ ������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "������� ��� �������� �������: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� �������: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "������� ������������ JSON ��� �������� �������: {Message}", ex.Message);
                ErrorMessage = $"������� ������� �����: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������� ����������� ������� ��� �������� �������: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
            }

            return RedirectToPage(); // ���������������, ��� ������� ������
        }

        public async Task<IActionResult> OnPostEditClassroomAsync()
        {
            // �������� ������� �����
            if (EditedClassroomId == Guid.Empty || string.IsNullOrWhiteSpace(EditedClassroomNumberString) || string.IsNullOrWhiteSpace(EditedClassroomBuildingString))
            {
                ModelState.AddModelError(string.Empty, "ID, ����� �� ������ ������� �� ������ ���� ��������.");
                await LoadClassroomsAsync();
                return Page();
            }

            if (!int.TryParse(EditedClassroomNumberString, out int numberToEdit))
            {
                ModelState.AddModelError(string.Empty, "����� ������� �� ���� ����� ������.");
                await LoadClassroomsAsync();
                return Page();
            }

            if (!int.TryParse(EditedClassroomBuildingString, out int buildingToEdit))
            {
                ModelState.AddModelError(string.Empty, "����� ������� �� ���� ����� ������.");
                await LoadClassroomsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                // ��������� ��������� ��'����, �� ������� ����������� API (ClassroomRequest)
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

                Message = "�������� ������ ��������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "������� ��� �������� �������: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� �������: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "������� ������������ JSON ��� �������� �������: {Message}", ex.Message);
                ErrorMessage = $"������� ������� �����: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������� ����������� ������� ��� �������� �������: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteClassroomAsync()
        {
            if (DeletedClassroomId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID ������� ��� ��������� �� ���� ���� �������.");
                await LoadClassroomsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                response = await client.DeleteAsync($"Classroom/{DeletedClassroomId}");
                response.EnsureSuccessStatusCode();

                Message = "�������� ������ ��������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "������� ��� �������� �������: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� �������: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������� ����������� ������� ��� �������� �������: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}