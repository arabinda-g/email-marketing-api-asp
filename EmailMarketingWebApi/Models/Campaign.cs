namespace EmailMarketingWebApi.Models
{
    public class Campaign
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public string EmailSubject { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        //public ICollection<Recipient>? Recipients { get; set; }
        //public ICollection<EmailQueue> EmailQueue { get; set; }

    }
}