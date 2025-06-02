using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using UniversitySchedule.UI.Models;
using System;
using System.Globalization; // ������ ��� TryParseExact
using System.Linq; // ������ ��� Split

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

        // ���������� ��� ����� ���������
        [BindProperty]
        public string NewClassTimeframe { get; set; } = string.Empty;

        // ���������� ��� ����� �����������
        [BindProperty]
        public Guid EditedClassTimeId { get; set; }
        [BindProperty]
        public string EditedClassTimeframe { get; set; } = string.Empty;

        // ���������� ��� ���������
        [BindProperty]
        public Guid DeletedClassTimeId { get; set; }

        // ���������� ��� ������
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
                    // ��� ������ �� ������� ��������, API ����� �����
                    apiUrl = $"ClassTime/timeframe/{SearchByTimeframe}";
                }
                else
                {
                    // ����� � ������ ��������� ClassTimeController ���� �������� �� �������������
                    // ���� �������, ������� GetClassTimes(pageNumber, pageSize) �� ����������
                    // �� ������� ��������� ���. ��� ��������, ��������� GetAll.
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
                    Message = "�� �������� ������� ���� ������ �� �������� ���������.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error during ClassTime load: {Message}", ex.Message);
                ErrorMessage = $"������� ������������ ���� ������: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
                ClassTimes = new List<ClassTimeDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error during ClassTime load: {Message}", ex.Message);
                ErrorMessage = $"������� ������� ����� ���� ������: {ex.Message}";
                ClassTimes = new List<ClassTimeDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during ClassTime load: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
                ClassTimes = new List<ClassTimeDto>();
            }
        }

        // ��������� ����� ��� �������� �������� ����
        private bool ValidateTimeRange(string timeRangeString, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(timeRangeString))
            {
                ModelState.AddModelError(fieldName, "������� ������� �� ���� ���� �������.");
                return false;
            }

            var parts = timeRangeString.Split('-').Select(p => p.Trim()).ToArray();
            if (parts.Length != 2)
            {
                ModelState.AddModelError(fieldName, "������ ���� �� ���� '��:�� - ��:��'.");
                return false;
            }

            if (!TimeSpan.TryParseExact(parts[0], "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan startTime) ||
                !TimeSpan.TryParseExact(parts[1], "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan endTime))
            {
                ModelState.AddModelError(fieldName, "����������� ������ ����. �������������� ��:�� (���������, 08:00).");
                return false;
            }

            if (startTime >= endTime)
            {
                ModelState.AddModelError(fieldName, "��� ������� �� ���� ����� ���� ���������.");
                return false;
            }

            TimeSpan minAllowedTime = new TimeSpan(6, 0, 0); // 06:00
            TimeSpan maxAllowedTime = new TimeSpan(20, 0, 0); // 20:00

            if (startTime < minAllowedTime || endTime > maxAllowedTime)
            {
                ModelState.AddModelError(fieldName, "������ ���� ������� ����� ���� � ������� �� 06:00 �� 20:00.");
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
                    TimeFrame = NewClassTimeframe // ��������� �� string
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PostAsync("ClassTime", content);
                response.EnsureSuccessStatusCode();

                Message = "��� ������� ������ ������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� ���� �������: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error creating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"������� ������� �����: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditClassTimeAsync()
        {
            if (EditedClassTimeId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID ���� ������� �� ���� ���� �������.");
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
                    TimeFrame = EditedClassTimeframe // ��������� �� string
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                response = await client.PutAsync($"ClassTime/{EditedClassTimeId}", content);
                response.EnsureSuccessStatusCode();

                Message = "��� ������� ������ ��������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error updating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� ���� �������: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error updating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"������� ������� �����: {ex.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating ClassTime: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteClassTimeAsync()
        {
            if (DeletedClassTimeId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID ���� ������� ��� ��������� �� ���� ���� �������.");
                await LoadClassTimesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("UniversityApi");
            HttpResponseMessage response = null;

            try
            {
                response = await client.DeleteAsync($"ClassTime/{DeletedClassTimeId}");
                response.EnsureSuccessStatusCode();

                Message = "��� ������� ������ ��������!";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error deleting ClassTime: {Message}", ex.Message);
                ErrorMessage = $"������� ��� �������� ���� �������: {ex.Message}";
                if (response != null)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage += $" (������: {(int)response.StatusCode}, �����: {errorDetails})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting ClassTime: {Message}", ex.Message);
                ErrorMessage = $"������� ����������� �������: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}