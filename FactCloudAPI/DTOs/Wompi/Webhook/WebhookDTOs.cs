namespace FactCloudAPI.DTOs.Wompi.Webhook
{
    public class WompiWebhookDto
    {
        public string Event { get; set; }
        public WebhookData Data { get; set; }
        public long Timestamp { get; set; }
        public WebhookSignature Signature { get; set; }
    }

    public class WebhookData
    {
        public WebhookTransaction Transaction { get; set; }
    }

    public class WebhookTransaction
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public int AmountInCents { get; set; }
        public string Currency { get; set; }
        public string CustomerEmail { get; set; }
    }

    public class WebhookSignature
    {
        public string CheckSum { get; set; }
    }

}
