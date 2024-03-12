namespace HalloDoc.DataAccess.ViewModel
{
    public class PatientDocumentViewModel
    {
        public DateTime createdate;
        public string Filename { get; set; }
        public int RequestId { get; set; }

        public int ReqClientId { get; set; }

        public string Name { get; set; }

        public bool? IsDeleted { get; set; } 

    }
}
