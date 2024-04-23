namespace HalloDoc.DataAccess.utils
{
    public class enumsFile
    {
        public enum adminRoles
        {
            Regions = 1,
            Scheduling = 2,
            History = 3,
            Accounts = 4,
            MyProfile = 5,
            AdminDashboard = 6,
            Dashboard = 7,
            Role = 11,
            Provider = 12,
            RequestData = 13,
            SendOrder = 14,
            Vendersinfo = 15,
            Profession = 16,
            EmailLogs = 18,
            HaloAdministrators = 19,
            HaloUsers = 20,
            CancelledHistory = 21,
            ProviderLocation = 23,
            HaloEmployee = 24,
            HaloWorkPlace = 25,
            PatientRecords = 28,
            BlockedHistory = 29,
            Invoicing = 30,
            SMSLogs = 32
        }

        public enum physicianRoles
        {
            Dashboard = 7,
            History = 8,
            MySchedule = 9,
            MyProfile = 10,
            SendOrders = 17,
            Invoicing = 31
        }

        public enum requestStatus
        {
            Unassigned =1,
            Accepted = 2,
            Cancelled = 3,
            Concluded = 4,
            Closed = 5,
            Assigned = 6,
            Declined = 7,
            Consult = 8,
            Clear = 9,
            CancelledByProvider = 10,
            CancelledByPatient = 11,
            CCAprovedByAdmin = 12,
            Unpaid = 13,
            Block = 14,
            MdOnHouseCall = 15
        }
        public enum RequestType
        {
            patient = 1,
            family = 2,
            Concierge = 3,
            Business = 4
        }

        public enum DashboardStatus
        {
            newStatus = 1,
            pending = 2,
            active = 8,
            conclude = 4,
            close = 5,
            unpaid = 13
        }

        public enum PhysicianCalltype
        {
            HouseCall = 1,
            Consult = 2
        }

    }
}
