using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;

namespace IncidentsRegistration.Services
{
    public class DecisionService : IDecisionService
    {
        private readonly IncidentsDbContext _context;
        public DecisionService(IncidentsDbContext context) => _context = context;

        public void SaveDecision(Decision decision, CriminalCase? cc, TerritorialTransfer? tt)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Decisions.Add(decision);
                _context.SaveChanges();

                if (decision.DecisionType == "возбуждено уголовное дело" && cc != null)
                {
                    cc.IdDecision = decision.IdDecision;
                    _context.CriminalCases.Add(cc);
                }
                else if (decision.DecisionType == "передано по территориальному признаку" && tt != null)
                {
                    tt.IdDecision = decision.IdDecision;
                    _context.TerritorialTransfers.Add(tt);
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
        }
    }
}
