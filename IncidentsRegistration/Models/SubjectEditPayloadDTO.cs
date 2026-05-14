namespace IncidentsRegistration.Models
{
    public class SubjectEditPayloadDTO
    {
        public int IncidentId { get; set; }
        public Subject? Subject { get; set; }
        public SubjectRole CurrentRole { get; set; }
    }
}
