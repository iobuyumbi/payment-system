using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Repositories;

public interface ILoanStatementRepository : IBaseRepository<LoanStatement> {

    Task DeleteLoanStatement(Guid id);
}

