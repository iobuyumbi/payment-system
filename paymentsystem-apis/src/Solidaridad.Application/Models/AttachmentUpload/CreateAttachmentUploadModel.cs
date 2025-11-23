namespace Solidaridad.Application.Models.AttachmentUpload;

public class CreateAttachmentUploadModel
{
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string ImagePath { get; set; }
    public string ThumbPath { get; set; }
    public string ContentType { get; set; }
    public bool IsEntitySaved { get; set; }
}
public class CreateAttachmentUploadResponseModel :BaseResponseModel{ }
