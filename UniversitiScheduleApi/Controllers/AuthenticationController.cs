using Microsoft.AspNetCore.Identity; // Додайте це для SignInResult, IdentityResult
using Microsoft.AspNetCore.Identity.Data; // Може бути використано для LoginRequest/RegisterRequest, якщо це з Identity
using Microsoft.AspNetCore.Mvc;
using UNISchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // POST: /Authentication/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            
            var result = await _authenticationService.LoginAsync(loginRequest.Email, loginRequest.Password, false); // false для RememberMe

            if (result.Succeeded)
            {
                // Якщо cookie встановлено, просто повертаємо OK.
                // Клієнт (Razor Page) повинен буде перенаправити.
                return Ok(new { Message = "Login successful." });
            }
            if (result.RequiresTwoFactor)
            {
                return Unauthorized(new { Message = "Requires two-factor authentication." });
            }
            if (result.IsLockedOut)
            {
                return Unauthorized(new { Message = "Account locked out." });
            }
            // Якщо не успішно і не інші статуси, то це невірні облікові дані.
            return Unauthorized(new { Message = "Invalid email or password." });
        }

        // POST: /Authentication/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterStudentRequest registerRequest)
        {
           var group = Group.Create(registerRequest.Group.Id, registerRequest.InstituteId);

            var result = await _authenticationService.RegisterStudentAsync(registerRequest.Email, registerRequest.Password, registerRequest.FirstName, registerRequest.LastName, registerRequest.Patronymic,  );
            if (result.Succeeded)
            {
                // Реєстрація успішна. Можна автоматично ввійти користувача, якщо потрібно,
                // або просто повідомити про успіх.
                // CreatedAtAction використовується для API, щоб вказати,
                // куди можна перейти, щоб отримати новий ресурс (тут, можливо, вхід).
                return CreatedAtAction(nameof(Login), new { email = registerRequest.Email }, new { Message = "Registration successful." });
            }
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        // POST: /Authentication/Register/Teacher
        [HttpPost("Register/Teacher")]
        public async Task<IActionResult> RegisterTeacher([FromBody] RegisterTeacherRequest registerTeacherRequest)
        {
            var result = await _authenticationService.RegisterTeacherAsync(registerTeacherRequest.Email, registerTeacherRequest.Password, registerTeacherRequest.FirstName, registerTeacherRequest.LastName, registerTeacherRequest.Patronymic, registerTeacherRequest.Institute);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(Login), new { email = registerTeacherRequest.Email }, new { Message = "Registration successful." });
            }
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        // POST: /Authentication/Logout
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync(); // Видаляє cookie
            return Ok(new { Message = "Logged out successfully." });
        }
    }
}