using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities
{
    [Table("Addresses")]
    public class Address: BaseEntity, IAuditedEntity
    {
        public Guid CountryId { get; set; }
        
        public Guid AdminLevel1Id { get; set; }
        
        public Guid AdminLevel2Id { get; set; }
        
        public Guid AdminLevel3Id { get; set; }
        
        public Guid AdminLevel4Id { get; set; }
        
        public Guid CreatedBy { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public Guid? UpdatedBy { get; set; }
        
        public DateTime? UpdatedOn { get; set ; }
        public virtual Country Country { get; set; }
        public virtual AdminLevel1 AdminLevel1 { get; set; }
        public virtual AdminLevel2 AdminLevel2 { get; set; }
        public virtual AdminLevel3 AdminLevel3 { get; set; }
        public virtual AdminLevel4 AdminLevel4 { get; set; }
    }
}
