namespace HalloDoc.DataAccess.ViewModel
{
    public class PatientDashboardViewModel
    {
        public DateTime Createddate { get; set; }

        public String? Status { get; set; }

        public int fileCount { get; set; }
        public int RequestId { get; set; }

        public int phyId { get; set; }  
    }
}
