using EmailMarketingWebApi.Data;
using EmailMarketingWebApi.Models;
using EmailMarketingWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Mail;

namespace EmailMarketingWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CronController
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public CronController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet("SendEmail", Name = "SendEmail")]
        public IActionResult SendEmail()
        {
            DateTime sentAt = DateTime.Now;

            //EmailQueue emailQueue = EmailQueue.Where((x) => x.Status == "pending").FirstOrDefault();
            // Select the first email in the queue that has a status of "pending"
            //EmailQueue emailQueue = _context.EmailQueue.Where((x) => x.Status == "pending").FirstOrDefault();
            EmailQueue emailQueue = _context.EmailQueue.FirstOrDefault(e => e.Status == "pending");

            if (emailQueue != null)
            {
                string recipientEmail = emailQueue.RecipientEmail;
                string recipientName = emailQueue.FirstName + " " + emailQueue.LastName;

                try
                {
                    // Create and send an email
                    _emailService.SendEmail(to: recipientEmail, body: emailQueue.Message, subject: emailQueue.Subject);
                    Console.WriteLine("Email sent to: " + recipientEmail);

                    // Update status in the email_queue table
                    emailQueue.Status = "sent";
                    emailQueue.SentDate = sentAt;
                    _context.SaveChanges();

                    return new ObjectResult("Email sent to: " + recipientEmail);
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., logging)
                    Console.WriteLine(ex.Message);

                    Console.WriteLine("Email not sent to: " + recipientEmail);

                    // Update status in the email_queue table
                    emailQueue.Status = "failed";
                    _context.SaveChanges();

                    return new ObjectResult("Email not sent to: " + recipientEmail);
                }

            }
            else
            {
                Console.WriteLine("No emails to send.");
                return new ObjectResult("No emails to send.");
            }
            //return new ObjectResult(emailQueue);
        }


        // Create a function to update campaign status by checking if all emails have been sent
        [HttpGet("UpdateCampaignStatus", Name = "UpdateCampaignStatus")]
        public IActionResult UpdateCampaignStatus()
        {
            // Get all campaigns
            var campaigns = _context.Campaigns.ToList();

            foreach (var campaign in campaigns)
            {
                // Get all emails in the email queue for this campaign
                var emails = _context.EmailQueue.Where(e => e.CampaignId == campaign.CampaignId && e.Status == "pending").ToList();

                // If there are no pending emails, update the campaign status to completed
                if (emails.Count == 0)
                {
                    campaign.Status = "completed";
                    _context.SaveChanges();
                }
            }

            return new ObjectResult("Campaign status updated.");
        }







    }
}
