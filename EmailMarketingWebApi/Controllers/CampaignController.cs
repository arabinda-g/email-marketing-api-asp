using EmailMarketingWebApi.Data;
using EmailMarketingWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EmailMarketingWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CampaignController
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CampaignController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        // Add route for creating a new campaign
        [HttpPost(Name = "CreateCampaign")]
        public IActionResult CreateCampaign([FromBody] CampaignFormData campaignFormData)
        {
            var baseUrl = _configuration["AppConfig:URL"];
            
            // Create a new campaign
            Campaign campaign = new Campaign
            {
                Name = campaignFormData.Name,
                EmailSubject = campaignFormData.EmailSubject,
                Status = campaignFormData.Status
            };

            // Add the campaign to the database
            _context.Campaigns.Add(campaign);
            _context.SaveChanges();

            // Loop through the recipient list and add each recipient to the database
            foreach (RecipientData recipientData in campaignFormData.Recipients)
            {
                // Generate TrackingCode
                string trackingCode = Guid.NewGuid().ToString();

                Recipient recipient = new Recipient
                {
                    CampaignId = campaign.CampaignId,
                    FirstName = recipientData.FirstName,
                    LastName = recipientData.LastName,
                    Email = recipientData.Email
                };

                //_context.Recipients.Add(recipient);
                //_context.SaveChanges();


                // Change the values in the campaign template
                //string emailMessage = ReplaceCampaignTemplateValues(
                //    template: campaignFormData.EmailMessage,
                //    recipientFirstName: recipient.FirstName,
                //    recipientLastName: recipient.LastName
                //    );
                string replacedTemplate = campaignFormData.EmailMessage.Replace("{{recipient_first_name}}", recipient.FirstName);
                replacedTemplate = replacedTemplate.Replace("{{recipient_last_name}}", recipient.LastName);
                replacedTemplate = replacedTemplate.Replace("{{unsubscribe_link}}", baseUrl + "/unsubscribe?email=" + recipient.Email + "&token=" + trackingCode);
                replacedTemplate = replacedTemplate.Replace("{{email_tracker_tag}}", "<img src=\"{{email_tracker_url}}\" alt=\"\" width=\"1\" height=\"1\" style=\"display: block; width: 1px; height: 1px; border: none; margin: 0; padding: 0;\">");
                replacedTemplate = replacedTemplate.Replace("{{email_tracker_url}}", baseUrl + "/tracker?token=" + trackingCode);


                // Add the email to the email queue
                EmailQueue emailQueue = new EmailQueue
                {
                    CampaignId = campaign.CampaignId,
                    SenderEmail = _configuration["SmtpSettings:SenderEmail"],
                    SenderName = _configuration["SmtpSettings:SenderName"],


                    //RecipientId = recipient.RecipientId,
                    RecipientEmail = recipient.Email,
                    FirstName = recipient.FirstName,
                    LastName = recipient.LastName,
                    Subject = campaign.EmailSubject,
                    Message = replacedTemplate,
                    Status = "pending",
                    TrackingCode = trackingCode
                };

                _context.EmailQueue.Add(emailQueue);
                _context.SaveChanges();

            }

            return new ObjectResult(campaign);
        }


        // Create a private function to change values in the campaign template
        private string ReplaceCampaignTemplateValues(string template, string recipientFirstName, string recipientLastName)
        {
            string replacedTemplate = template.Replace("{{recipient_first_name}}", recipientFirstName);
            replacedTemplate = replacedTemplate.Replace("{{recipient_last_name}}", recipientLastName);
            replacedTemplate = replacedTemplate.Replace("{{unsubscribe_link}}", "https://www.example.com/unsubscribe");
            replacedTemplate = replacedTemplate.Replace("{{email_tracker_tag}}", "<img src=\"{{email_tracker_url}}\" alt=\"\" width=\"1\" height=\"1\" style=\"display: block; width: 1px; height: 1px; border: none; margin: 0; padding: 0;\">");
            replacedTemplate = replacedTemplate.Replace("{{email_tracker_url}}", "https://www.example.com/unsubscribe");
            return replacedTemplate;
        }


    }


    public class CampaignFormData
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public RecipientData[] Recipients { get; set; }
    }
    public class RecipientData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
