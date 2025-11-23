using Solidaridad.Core.Common;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.Core.Entities
{
    public class Account : BaseEntity, IAuditedEntity
    {
        public Farmer Owner { get; set; }
        
        public decimal Balance { get; private set; }

        public List<Transaction> Transactions { get; private set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        // Constructor
        public Account(Farmer owner)
        {
            this.Id = Guid.NewGuid();
            Owner = owner;
            Balance = 0.0m;
            Transactions = new List<Transaction>();
        }
    }
}
