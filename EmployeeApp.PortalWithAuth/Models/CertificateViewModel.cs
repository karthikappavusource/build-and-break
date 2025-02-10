namespace EmployeeApp.PortalWithAuth.Models
{
    public class CertificateViewModel
    {
        public DateTime CertifiedOn { get; set; }
        public string QualifiedFor { get; set; }
        public DateTime Expiration { get; set; }
        public IFormFile file { get; set; }
        public string FileBase64 { get; set; }
        public int FileId { get; set; } // This will be used to identify the file for download
        public string status {  get; set; }
    }
}
