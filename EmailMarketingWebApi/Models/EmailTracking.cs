namespace EmailMarketingWebApi.Models
{
    public class EmailTracking
    {
        // Columns: id, email_id, campaign_id, email_address, action, ip_address, user_agent
        public int EmailTrackingId { get; set; }
        public int EmailQueueId { get; set; }
        public int CampaignId { get; set; }
        public string EmailAddress { get; set; }
        public string Action { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
