namespace LaporPinjol.Models
{
    public class Report
    {
        public int ReportId { get; set; }

        //User ID from AspNetUser table
        public string? UserId { get; set; }

        public string? LoanCompanyName { get; set; }

        public string? LoanCompanyBankAccount { get; set; }

        public string? LoanCompanyPhoneNumber { get; set; }

        public string? EvidenceLinkOnCloudStorage { get; set; }

        public ReportStatus VerificationStatus { get; set; }

        public DateTime SubmissionDate { get; set; }
    }

    public enum ReportStatus
    {
        Submitted,
        Verified,
        Investigated
    }
}
