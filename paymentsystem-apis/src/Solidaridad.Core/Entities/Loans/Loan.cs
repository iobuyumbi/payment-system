using Solidaridad.Core.Common;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.Core.Entities.Loans
{
    public class Loan: BaseEntity, IAuditedEntity
    {
        public Guid LoanID { get; set; }
        public Farmer Borrower { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermInMonths { get; set; }
        public decimal Balance { get; private set; }
        public List<Payment> Payments { get; private set; }
        public Guid CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? UpdatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
