using Azure;
using Azure.Core;
using EmailMarketingWebApi.Data;
using EmailMarketingWebApi.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailMarketingWebApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class EmailTrackingController
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        //private readonly HttpContext _httpContext;

        public EmailTrackingController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //_httpContext = httpContext;
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


                // Add a tracking record to the database
                EmailTracking emailTracking = new EmailTracking
                {
                    EmailQueueId = emailQueue.EmailQueueId,
                    CampaignId = emailQueue.CampaignId,
                    EmailAddress = emailQueue.RecipientEmail,
                    Action = "opened",


                    // Store the IP address and user agent of the user who opened the email
                    //IpAddress = _httpContext.Connection.RemoteIpAddress?.ToString(),
                    //UserAgent = _httpContext.Request.Headers["User-Agent"]
                };
                _context.EmailTracking.Add(emailTracking);
                _context.SaveChanges();
            }

            // Redirect to the tracking pixel
            string baseUrl = _configuration["AppConfig:URL"];
            string trackingPixelUrl = baseUrl + "/images/tracking_pixel.png";
            return new RedirectResult(trackingPixelUrl);
        }

        //// Add route for tracking email clicks
        //[HttpGet("tracker/{trackingCode}/{linkId}", Name = "ClickEmail")]
        //public IActionResult ClickEmail(string trackingCode, int linkId)
        //{
        //    // Get the email from the database
        //    EmailQueue emailQueue = _context.EmailQueue.FirstOrDefault(e => e.TrackingCode == trackingCode);

        //    if (emailQueue != null)
        //    {
        //        // Update the email status
        //        emailQueue.Status = "clicked";
        //        emailQueue.ClickedDate = DateTime.Now;
        //        _context.SaveChanges();
        //    }

        //    // Get the link from the database
        //    Link link = _context.Links.FirstOrDefault(l => l.LinkId == linkId);
        //    if (link != null)
        //    {

        //        // Add a tracking record to the database
        //        EmailTracking emailTracking = new EmailTracking
        //        {
        //            EmailQueueId = emailQueue.EmailQueueId,
        //            CampaignId = emailQueue.CampaignId,
        //            EmailAddress = emailQueue.RecipientEmail,
        //            Action = "clicked",
        //            IpAddress = _httpContext.Connection.RemoteIpAddress?.ToString(),
        //            UserAgent = _httpContext.Request.Headers["User-Agent"]
        //        };
        //        _context.EmailTracking.Add(emailTracking);
        //        _context.SaveChanges();

        //        // Redirect to the link
        //        return new RedirectResult(link.Url);
        //    }
        //    else
        //    {
        //        return new NotFoundResult();
        //    }
        //}

        //// Add route for tracking email bounces
        //[HttpPost("tracker/bounce", Name = "BounceEmail")]
        //public IActionResult BounceEmail([FromBody] BounceData bounceData)
        //{
        //    // Get the email from the database
        //    EmailQueue emailQueue = _context.EmailQueue.FirstOrDefault(e => e.MessageId == bounceData.MessageId);

        //    if (emailQueue != null)
        //    {
        //        // Update the email status
        //        emailQueue.Status = "bounced";
        //        emailQueue.BouncedDate = DateTime.Now;
        //        _context.SaveChanges();

        //        // Add a tracking record to the database
        //        EmailTracking emailTracking = new EmailTracking
        //        {
        //            EmailQueueId = emailQueue.EmailQueueId,
        //            CampaignId = emailQueue.CampaignId,
        //            EmailAddress = emailQueue.RecipientEmail,
        //            Action = "bounced",
        //            IpAddress = bounceData.IpAddress,
        //            UserAgent = bounceData.UserAgent
        //        };
        //        _context.EmailTracking.Add(emailTracking);
        //        _context.SaveChanges();
        //    }

        //    return new OkResult();
        //}

        // Add route for tracking email unsubscribes
        [HttpGet("unsubscribe/{trackingCode}", Name = "UnsubscribeEmail")]
        public IActionResult UnsubscribeEmail(string trackingCode)
        {
            // Get the email from the database
            EmailQueue emailQueue = _context.EmailQueue.FirstOrDefault(e => e.TrackingCode == trackingCode);

            if (emailQueue != null)
            {
                // Update the email status
                emailQueue.Status = "unsubscribed";
                emailQueue.UnsubscribedDate = DateTime.Now;
                _context.SaveChanges();

                // Add a tracking record to the database
                EmailTracking emailTracking = new EmailTracking
                {
                    EmailQueueId = emailQueue.EmailQueueId,
                    CampaignId = emailQueue.CampaignId,
                    EmailAddress = emailQueue.RecipientEmail,
                    Action = "unsubscribed",
                    //IpAddress = _httpContext.Connection.RemoteIpAddress?.ToString(),
                    //UserAgent = _httpContext.Request.Headers["User-Agent"]
                };
                _context.EmailTracking.Add(emailTracking);
                _context.SaveChanges();
            }

            //// Redirect to the unsubscribe confirmation page
            //string baseUrl = _configuration["AppConfig:URL"];
            //string unsubscribeConfirmationUrl = baseUrl + "/unsubscribe-confirmation";
            //return new RedirectResult(unsubscribeConfirmationUrl);

            // Return: You have been unsubscribed
            return new OkObjectResult("You have been unsubscribed.");
        }
    }
}
