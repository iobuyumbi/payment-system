using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.AttachmentUpload;
using Solidaridad.Application.Models.LoanAttachmentModel;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Repositories;

namespace Solidaridad.API.Controllers;

[Authorize]
public class FileUploadController : ApiController
{
    #region DI

    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IAttachmentUploadService _attachmentUploadService;
    private readonly IAttachmentMappingRepository _attachmentMappingRepository;
    private readonly IBatchAttachmentMappingRepository _batchAttachmentMappingRepository;
    public FileUploadController(

        IWebHostEnvironment webHostEnvironment, IAttachmentUploadService attachmentUploadService,
        IAttachmentMappingRepository attachmentMappingRepository, IBatchAttachmentMappingRepository batchAttachmentMappingRepository)
    {

        _webHostEnvironment = webHostEnvironment;
        _attachmentUploadService = attachmentUploadService;
        _attachmentMappingRepository = attachmentMappingRepository;
        _batchAttachmentMappingRepository = batchAttachmentMappingRepository;
    }
    #endregion

    [AllowAnonymous]
    [HttpPost, DisableRequestSizeLimit]
    [Route("loanapplications")]
    public async Task<ActionResult> Upload(string appId)
    {
        string[] attachmentIds = new string[Request.Form.Files.Count];
        try
        {
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                var newPath = Path.Combine(_webHostEnvironment.ContentRootPath, $"wwwroot\\uploads\\loanapplications");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                    Directory.CreateDirectory($"{newPath}\\thumbs");
                }
                if (file.Length > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string originalNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    string newFileName = $"{originalNameWithoutExtension}_{Guid.NewGuid()}{extension}";

                    string fullPath = Path.Combine(newPath, newFileName);
                    string thumbFolderName = "thumbs";
                    string thumbFileName = $"thumb_{newFileName}";
                    string thumbPath = Path.Combine(newPath, thumbFolderName, thumbFileName);

                    if (extension == ".jpeg" || extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".webp")
                    {
                        using (var image = Image.Load(file.OpenReadStream()))
                        {
                            int height = 500;
                            // Resize the image to the desired dimensions for the main image
                            int width = (int)Math.Round((double)image.Width / image.Height * height);
                            image.Mutate(x => x.Resize(new Size(width, height)));
                            image.Save(fullPath);

                            // Generate the thumbnail image
                            int thumbWidth = 300; // You can change this value according to your requirements
                            int thumbHeight = (int)Math.Round((double)image.Height / image.Width * thumbWidth);
                            image.Mutate(x => x.Resize(new Size(thumbWidth, thumbHeight)));
                            image.Save(thumbPath);
                        }
                    }
                    else if (extension == ".pdf")
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        thumbPath = null;
                    }
                    else
                    {
                        continue;
                    }

                    var param = new CreateAttachmentUploadModel
                    {
                        ContentType = file.ContentType,
                        FileName = newFileName,
                        FileSize = file.Length,
                        ImagePath = $"uploads/loanapplications/{newFileName}",
                        ThumbPath = $"uploads/loanapplications/thumbs/{thumbFileName}",
                    };

                    var response = await _attachmentUploadService.CreateAsync(param);
                    if (response != null)
                    {
                        attachmentIds[i] = Convert.ToString(response.Id);
                        if (!string.IsNullOrEmpty(appId))
                        {
                            var map = new AttachmentMapping
                            {
                                LoanApplicationId = Guid.Parse(appId),
                                AttachmentId = (Guid)response.Id
                            };
                            await _attachmentMappingRepository.AddAsync(map);
                        }
                    }
                }
            }

            return Ok(new ApiResponseModel<object>
            {
                Success = true,
                Message = "File saved successfully.",
                Data = attachmentIds
            });
        }
        catch (System.Exception)
        {
            return BadRequest(new { success = false, message = "File could not be uploaded" });
        }

    }

    [AllowAnonymous]
    [HttpPost, DisableRequestSizeLimit]
    [Route("LoanBatchUpload")]
    public async Task<ActionResult> LoanBatchUpload(string appId)
    {
        string[] attachmentIds = new string[Request.Form.Files.Count];
        try
        {
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                var newPath = Path.Combine(_webHostEnvironment.ContentRootPath, $"wwwroot\\uploads\\loanbatchfiles");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                    Directory.CreateDirectory($"{newPath}\\thumbs");
                }
                if (file.Length > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string originalNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    string newFileName = $"{originalNameWithoutExtension}_{Guid.NewGuid()}{extension}";

                    string fullPath = Path.Combine(newPath, newFileName);
                    string thumbFolderName = "thumbs";
                    string thumbFileName = $"thumb_{newFileName}";
                    string thumbPath = Path.Combine(newPath, thumbFolderName, thumbFileName);

                    if (extension == ".jpeg" || extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".webp")
                    {
                        using (var image = Image.Load(file.OpenReadStream()))
                        {
                            int height = 500;
                            // Resize the image to the desired dimensions for the main image
                            int width = (int)Math.Round((double)image.Width / image.Height * height);
                            image.Mutate(x => x.Resize(new Size(width, height)));
                            image.Save(fullPath);

                            // Generate the thumbnail image
                            int thumbWidth = 300; // You can change this value according to your requirements
                            int thumbHeight = (int)Math.Round((double)image.Height / image.Width * thumbWidth);
                            image.Mutate(x => x.Resize(new Size(thumbWidth, thumbHeight)));
                            image.Save(thumbPath);
                        }
                    }
                    else if (extension == ".pdf")
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        thumbPath = null;
                    }
                    else
                    {
                        continue;
                    }

                    var param = new CreateAttachmentUploadModel
                    {
                        ContentType = file.ContentType,
                        FileName = newFileName,
                        FileSize = file.Length,
                        ImagePath = $"uploads/loanbatchfiles/{newFileName}",
                        ThumbPath = $"uploads/loanbatchfiles/thumbs/{thumbFileName}",
                    };

                    var response = await _attachmentUploadService.CreateAsync(param);
                    if (response != null)
                    {
                        attachmentIds[i] = Convert.ToString(response.Id);
                        if (!string.IsNullOrEmpty(appId))
                        {
                            var map = new BatchAttachmentMapping
                            {
                                LoanBatchId = Guid.Parse(appId),
                                AttachmentId = (Guid)response.Id
                            };
                            await _batchAttachmentMappingRepository.AddAsync(map);
                        }
                    }
                }
            }

            return Ok(new ApiResponseModel<object>
            {
                Success = true,
                Message = "File saved successfully.",
                Data = attachmentIds
            });
        }
        catch (System.Exception)
        {
            return BadRequest(new { success = false, message = "File could not be uploaded" });
        }

    }

    [AllowAnonymous]
    [HttpPost, DisableRequestSizeLimit]
    [Route("locations")]
    public async Task<ActionResult> UploadLogo()
    {
        string[] attachmentIds = new string[Request.Form.Files.Count];
        try
        {
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                var newPath = Path.Combine(_webHostEnvironment.ContentRootPath, $"wwwroot\\uploads\\locations");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                    Directory.CreateDirectory($"{newPath}\\thumbs");
                }
                if (file.Length > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string originalNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    string newFileName = $"{originalNameWithoutExtension}_{Guid.NewGuid()}{extension}";

                    string fullPath = Path.Combine(newPath, newFileName);
                    string thumbFolderName = "thumbs";
                    string thumbFileName = $"thumb_{newFileName}";
                    string thumbPath = Path.Combine(newPath, thumbFolderName, thumbFileName);

                    if (extension == ".jpeg" || extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".webp")
                    {
                        using (var image = Image.Load(file.OpenReadStream()))
                        {
                            int height = 500;
                            // Resize the image to the desired dimensions for the main image
                            int width = (int)Math.Round((double)image.Width / image.Height * height);
                            image.Mutate(x => x.Resize(new Size(width, height)));
                            image.Save(fullPath);

                            // Generate the thumbnail image
                            int thumbWidth = 300; // You can change this value according to your requirements
                            int thumbHeight = (int)Math.Round((double)image.Height / image.Width * thumbWidth);
                            image.Mutate(x => x.Resize(new Size(thumbWidth, thumbHeight)));
                            image.Save(thumbPath);
                        }
                    }
                    else if (extension == ".pdf")
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        thumbPath = null;
                    }
                    else
                    {
                        continue;
                    }

                    var param = new CreateAttachmentUploadModel
                    {
                        ContentType = file.ContentType,
                        FileName = newFileName,
                        FileSize = file.Length,
                        ImagePath = $"uploads/locations/{newFileName}",
                        ThumbPath = $"uploads/locations/thumbs/{thumbFileName}",
                    };

                    var response = await _attachmentUploadService.CreateAsync(param);
                    if (response != null)
                    {
                        attachmentIds[i] = Convert.ToString(response.Id);
                    }
                }
            }
                return Ok(new ApiResponseModel<object>
                {
                    Success = true,
                    Message = "File saved successfully.",
                    Data = attachmentIds
                });
            
            
        }
        catch (System.Exception)
        {
            return BadRequest(new { success = false, message = "File could not be uploaded" });
        }

    }

    [AllowAnonymous]
    [HttpGet("download/{module}")]
    public IActionResult DownloadFile(string module)
    {
        string filename = module switch
        {
            "farmer" => "Farmer_Upload_Template.xlsx",
            "loanApplication" => "kenya_loan_applications_template.xlsx",
            "PaymentDeductibles" => "Payment_Batch_Upload_Templates.xlsx",
            "PaymentFacilitations" => "Facilitation_Payments_Upload_Template.xlsx",
            _ => "" // Default case
        };

        if (string.IsNullOrEmpty(filename))
        {
            return NotFound("File not found");
        }

        var filePath = Path.Combine("wwwroot/downloads/excel_templates", filename);
        if (System.IO.File.Exists(filePath))
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", filename);
        }
        else
        {
            return NotFound("File not found");
        }
    }

}
