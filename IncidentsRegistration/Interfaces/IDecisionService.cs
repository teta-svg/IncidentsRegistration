using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface IDecisionService
    {
        void SaveDecision(Decision decision, CriminalCase? cc, TerritorialTransfer? tt);
    }
}
