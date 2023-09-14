using Azure;
using EmailMarketingWebApi.Data;
using EmailMarketingWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmailMarketingWebApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class EmailTrackingController
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EmailTrackingController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Add route for tracking email opens
        [HttpGet("tracker/{trackingCode}", Name = "OpenEmail")]
        public IActionResult OpenEmail(string trackingCode)
        {
            // Get the email from the database
            EmailQueue emailQueue = _context.EmailQueue.FirstOrDefault(e => e.TrackingCode == trackingCode);

            if (emailQueue != null)
            {
                // Update the email status
                emailQueue.Status = "opened";
                emailQueue.OpenedDate = DateTime.Now;
                _context.SaveChanges();
            }

            // Redirect to the tracking pixel
            string baseUrl = _configuration["AppConfig:URL"];
            string trackingPixelUrl = baseUrl + "/images/tracking_pixel.png";
            return new RedirectResult(trackingPixelUrl);
        }

    }
}
