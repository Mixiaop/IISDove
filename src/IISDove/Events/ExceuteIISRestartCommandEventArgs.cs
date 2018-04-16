namespace IISDove.Events
{
    public class ExceuteIISRestartCommandEventArgs
    {
        public DoveSenderDto Sender { get; set; }

        public string SiteName { get; set; }

        public ExceuteIISRestartCommandEventArgs(DoveSenderDto dto, string siteName) {
            Sender = dto;
            SiteName = siteName;
        }
    }
}
