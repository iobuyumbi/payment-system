using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Loans
{
    public class LoanPayment : BaseEntity, IAuditedEntity
    {
        public Guid PaymentID { get; set; }
        public Guid LoanID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? UpdatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
