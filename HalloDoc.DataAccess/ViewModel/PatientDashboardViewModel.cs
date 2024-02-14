namespace HalloDoc.DataAccess.ViewModel
{
    public class PatientDashboardViewModel
    {

        public DateTime Createddate { get; set; }

        public String? Status { get; set; }

        public string Filename { get; set; } = null!;
        public int RequestId { get; set; }
    }
}
