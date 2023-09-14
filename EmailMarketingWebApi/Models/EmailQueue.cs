namespace EmailMarketingWebApi.Models
{
    public class EmailQueue
    {
        // Columns: id, campaign_id, sender_email, recipient_email, first_name, last_name, subject, message, headers, tracking_code, status, created_date, updated_date, sent_date, opened_date, clicked_date, bounced_date, unsubscribed_date, message_id
        public int EmailQueueId { get; set; }
        public int CampaignId { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; internal set; }
        public string RecipientEmail { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string? Headers { get; set; }
        public string TrackingCode { get; set; }
        public string Status { get; set; }="pending";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? SentDate { get; set; }
        public DateTime? OpenedDate { get; set; }
        public DateTime? ClickedDate { get; set; }
        public DateTime? BouncedDate { get; set; }
        public DateTime? UnsubscribedDate { get; set; }
        public string? MessageId { get; set; }
    }
}
    
