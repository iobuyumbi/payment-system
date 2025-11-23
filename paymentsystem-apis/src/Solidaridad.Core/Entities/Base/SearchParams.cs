using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities.Base
{
    public class SearchParams
    {
        public SearchParams()
        {
            PageNumber = 1;
            PageSize = 2000;
            Filter = string.Empty;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //public Guid CreatedBy { get; set; }
        public string Filter { get; set; }
        public DateTime FromDate { get; set; } = DateTime.UtcNow.AddMonths(-3);

        public DateTime ToDate { get; set; } = DateTime.UtcNow;
        public Guid? CountryId { get; set; }
    }
    public class FarmerSearchParams : SearchParams
    {
        public FarmerSearchParams()
        {
            ProjectId = null;
        }
        public Guid? CountryId { get; set; }
        public Guid? AdminLevel1Id { get; set; }
        public Guid? AdminLevel2Id { get; set; }
        public Guid? AdminLevel3Id { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? PaymentBatchId { get; set; }

    }
    public class LoanBatchSearchParams : SearchParams
    {
        public LoanBatchSearchParams()
        {

        }
        public Guid? ProjectId { get; set; }
        public List<string> ProjectIds { get; set; }
    }
    public class AssociateSearchParams : SearchParams
    {
        public AssociateSearchParams()
        {

        }
        public Guid BatchId { get; set; }


    }
    public class DisbursementSearchParams : SearchParams
    {
        public DisbursementSearchParams()
        {

        }



    }
    public class ImportSearchParams : SearchParams
    {
        public ImportSearchParams()
        {

        }
        public Guid? PaymentBatchId { get; set; }


    }

    public class PaymentSearchParams : SearchParams
    {
        public PaymentSearchParams()
        {

        }
    }

    public class DeductibleSearchParams : SearchParams
    {
        public DeductibleSearchParams()
        {

        }

        public Guid PaymentBatchId { get; set; }
    }

    public class CooperativeSearchParams : SearchParams
    {
        public CooperativeSearchParams()
        {
            CountryId = new Guid();
        }
        public Guid? CountryId { get; set; }
    }
    public class AttachmentSearchParams : SearchParams
    {
        public AttachmentSearchParams()
        {

        }
        public Guid ApplicationId { get; set; }

    }
    public class FarmingInputSearchParams : SearchParams
    {
        public FarmingInputSearchParams()
        {

        }

    }
    public class ProjectSearchParams : SearchParams
    {
        public ProjectSearchParams()
        {
            CountryIdList = [];

        }
      
        public List<string> CountryIdList { get; set; }

    }
    public class LoanItemSearchParams : SearchParams
    {
        public LoanItemSearchParams()
        {

        }

    }
    public class WalletSearchParams : SearchParams
    {
        public WalletSearchParams()
        {

        }

    }
    public class ApplicationStatusSearchParams : SearchParams
    {
        public ApplicationStatusSearchParams()
        {

        }

    }
    public class AdminLevel1SearchParams : SearchParams
    {
        public AdminLevel1SearchParams()
        {

        }
        public Guid? CountryId { get; set; }
    }

    public class AdminLevel2SearchParams : SearchParams
    {
        public AdminLevel2SearchParams()
        {

        }
        public Guid? CountyId { get; set; }
    }
    public class AdminLevel3SearchParams : SearchParams
    {
        public AdminLevel3SearchParams()
        {

        }
        public Guid? SubCountyId { get; set; }
    }
    public class ItemCategorySearchParams : SearchParams
    {
        public ItemCategorySearchParams()
        {

        }

    }
    public class LoanCategorySearchParams : SearchParams
    {
        public LoanCategorySearchParams()
        {

        }

    }
    public class LoanApplicationSearchParams : SearchParams
    {
        public LoanApplicationSearchParams()
        {
            BatchId = new Guid();
        }
        public Guid BatchId { get; set; }
    }
    public class RoleSearchParams : SearchParams
    {
        public RoleSearchParams()
        {

        }

    }
    public class RolePermissionSearchParams : SearchParams
    {
        public RolePermissionSearchParams()
        {

        }

    }
    public class ModuleSearchParams : SearchParams
    {
        public ModuleSearchParams()
        {

        }

    }


    public class FacilitationSearchParams : SearchParams
    {
        public FacilitationSearchParams()
        {
            BatchId = string.Empty;
        }
        public string BatchId { get; set; }
    }

    public class PortolioSearchParams : SearchParams
    {
        public PortolioSearchParams()
        {
            BatchIds = Array.Empty<Guid>(); // Fixed the type to match Guid[]  
            EndDate = DateTime.UtcNow;
            CurrentUser = Guid.Empty;
            OfficerId = "";
            StatusId = "";
        }
        public Guid[] BatchIds { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CurrentUser { get; set; }
        public string OfficerId { get; set; }
        public string StatusId { get; set; }
    }

    public class DisbursedSearchParams : SearchParams
    {
        public DisbursedSearchParams()
        {
            BatchIds = Array.Empty<Guid>(); // Fixed the type to match Guid[]  
            StartDate = DateTime.UtcNow;
            EndDate = DateTime.UtcNow;
            CurrentUser = Guid.Empty;
            OfficerId = "";
        }
        public Guid[] BatchIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CurrentUser { get; set; }
        public string OfficerId { get; set; }
    }

    public class PaymentReportSearchParams : SearchParams
    {
        public PaymentReportSearchParams()
        {
            PaymentBatchIds = Array.Empty<Guid>(); // Fixed the type to match Guid[]  
            StartDate = DateTime.UtcNow;
            EndDate = DateTime.UtcNow;
            CurrentUser = Guid.Empty;
            OfficerId = "";
            Status = -1;
            
        }
        public Guid[] PaymentBatchIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CurrentUser { get; set; }
        public string OfficerId { get; set; }
        public sbyte Status { get; set; }
    }


    public class AuthorisedUsersSearchParams : SearchParams
    {
        public AuthorisedUsersSearchParams()
        {
            Permission = ""; // Fixed the type to match Guid[]  
           
        }
        public string Permission { get; set; }
        
    }
}
