using Microsoft.AspNetCore.Mvc;
using UNISchedule.Core.Interfaces.ServiceInterfaces;


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
            var result = await _authenticationService.LoginAsync(loginRequest.Email, loginRequest.Password);
            if (result.Succeeded)
            {
                return Ok(new { Token = result.Token });
            }
            return Unauthorized(result.Errors);
        }
        // POST: /Authentication/Register/Stunent
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var result = await _authenticationService.RegisterStudentAsync(registerRequest.Email, registerRequest.Password, registerRequest.FirstName, registerRequest.LastName, registerRequest.Patronymic, registerRequest.Group);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(Login), new { email = registerRequest.Email }, null);
            }
            return BadRequest(result.Errors);
        }
        // POST: /Authentication/Register/Teacher
        [HttpPost("Register/Teacher")]
        public async Task<IActionResult> RegisterTeacher([FromBody] RegisterTeacherRequest registerTeacherRequest)
        {
            var result = await _authenticationService.RegisterTeacherAsync(registerTeacherRequest.Email, registerTeacherRequest.Password, registerTeacherRequest.FirstName, registerTeacherRequest.LastName, registerTeacherRequest.Patronymic, registerTeacherRequest.Institute);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(Login), new { email = registerTeacherRequest.Email }, null);
            }
            return BadRequest(result.Errors);
        }
        // POST: /Authentication/Logout
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            return Ok(new { Message = "Logged out successfully." });
        }
    }
}
