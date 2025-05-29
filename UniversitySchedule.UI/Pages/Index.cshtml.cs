using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace UniversitySchedule.UI.Pages;

public class IndexModel : PageModel
{
    // ���������� ��� ��'�������� � ������
    [BindProperty] // ����'������ ��� ������������� ��'�������� ����� � �����
    public InputModel Input { get; set; } = new InputModel(); // ����������, ��� �������� NullReferenceException

    // ���� ��� ���������� ������� ����� �����
    public class InputModel
    {
        [Required(ErrorMessage = "���� �����, ������ ����.")]
        [Display(Name = "����")]
        public string Login { get; set; }

        [Required(ErrorMessage = "���� �����, ������ ������.")]
        [DataType(DataType.Password)]
        [Display(Name = "������")]
        public string Password { get; set; }
    }

    public void OnGet()
    {
        // ��� ����� �����������, ���� ������� ������������� ������ (HTTP GET)
        // ��� �� ������ �������� �����������, ���� �������, ���������, ���������, �� ��� ���������������� ����������.
        // ���������, ���� ���������� ��� ������, ������������� ����:
        // if (User.Identity.IsAuthenticated)
        // {
        //     Response.Redirect("/Schedule"); // ��� �� ������� �������
        // }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // ��� ����� �����������, ���� ����� ������������� (HTTP POST)

        // ����������, �� �� ���� ����� ���������� ������� �������� (���������, [Required])
        if (!ModelState.IsValid)
        {
            // ���� � ������� ��������, ����������� �� ������� ������� � ���������
            return Page();
        }

        // --- ��� ���� ����� ��������� �� ������ API ��� �������������� ---
        // ���������:
        // var authService = new YourAuthenticationService(); // ������ �� ��� �����
        // var isAuthenticated = await authService.AuthenticateUser(Input.Login, Input.Password);

        // ��� ������� ������ ������ ������ ��������������
        bool isAuthenticated = (Input.Login == "test" && Input.Password == "password"); // ������ �� ������� �����

        if (isAuthenticated)
        {
            // ������ ��������������.
            // ����� ��� ������� ���� ���������� ��������� Identity, Cookies, ����.
            // �� ������ ���������� � ASP.NET Core Identity ��� ����� ��������� �������������� �� ����� Cookies/JWT.
            // ��� �������� ����� ������ ������������� (�� �� ����� ��������������):
            // return RedirectToPage("/Schedule"); // ��������������� �� ������� �������� ��� �������� �������

            // TODO: ���������� ������� �������������� (���������, �� ��������� HttpClient �� ������ API)
            // ϳ��� �������������� ��� ������� ���� �������� ���������� ��� ����������� �� ���� ���
            // � �������� Principal �� Claims ��� ��������� � ���/�����.
            // ���������:
            // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

            // ������ ����������� �� ������� � ������������ ��� ���� ��� ����������
            // � ��������� ������� ��� ���� redirect
            return RedirectToPage("/SuccessLogin", new { message = "�������������� ������!" }); // ��������� ��������������� ��� �����
        }
        else
        {
            // �������� ���� ��� ������� ������
            ModelState.AddModelError(string.Empty, "������� ���� ��� ������.");
            return Page(); // ����������� �� ������� � ��������
        }
    }
}

