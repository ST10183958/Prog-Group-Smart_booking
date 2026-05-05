using Microsoft.AspNetCore.Mvc;
using BookingSystem.Services;
using System.Threading.Tasks;

namespace BookingSystem.Controllers.Api
{
    [Route("api/notify")]
    [ApiController]
    public class NotificationApiController : ControllerBase
    {
        private readonly EmailService _emailService;

        public NotificationApiController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("appointment-accepted")]
        public async Task<IActionResult> NotifyAppointmentAccepted(string email, string doctorName)
        {
            await _emailService.SendEmailAsync(
                email,
                "Appointment Accepted",
                $"Your appointment has been accepted by Dr. {doctorName}. Please check your patient portal."
            );

            return Ok("Email sent");
        }
    }
}