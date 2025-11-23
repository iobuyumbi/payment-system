using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.ServiceProviders
{
    public class ServiceProvider : BaseEntity, IAuditedEntity
    {
        public string OrgName { get; set; }
        public Address Address { get; set; }
        public Guid CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? UpdatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
