namespace MediaStore.Services.Email
{
    public class EmailSettings
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }

        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }
}