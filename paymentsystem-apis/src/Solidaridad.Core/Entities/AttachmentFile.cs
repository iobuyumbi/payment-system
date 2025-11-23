using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class AttachmentFile : BaseEntity
{
    public string FileName { get; set; } 
    public long FileSize { get; set; } 
    public string ImagePath { get; set; } 
    public string ThumbPath { get; set; }
    public string ContentType { get; set; } 
    public bool IsEntitySaved { get; set; }

}
