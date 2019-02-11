

namespace ArVision.Service.Pharma.Shared
{
    
    public static class PharmaServiceRoutes
    {
        public const string ROUTE_PREFIX = @"Pharma"; 
        public const string ROUTE_TEST = @"Test";
        public const string ROUTE_GET_VERSION = @"GetVersion";
        public const string ROUTE_GET_JUICE_LIST = @"GetJuiceList";
        public const string ROUTE_GET_LIST = @"GetList/{table}";
        public const string ROUTE_GET_PATIENT_LIST = @"GetPatientList";
        public const string ROUTE_ADD_PATIENT_RX = @"AddPatientRX";
        public const string ROUTE_EDIT_PATIENT_RX = @"EditPatientRX";
        public const string ROUTE_ADD_RX_TO_PATIENT = @"AddRXToPatient";
        public const string ROUTE_GET_PATIENT_WITH_RX = @"GetPatientWithRX/{id}";
        public const string ROUTE_ADD_VISIT_TO_PATIENT = @"AddVisitToPatient";
    }
}
