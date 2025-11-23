using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;

namespace Solidaridad.API.Controllers
{
    public class MasterController : ApiController
    {
        #region DI
        private readonly ILoanBatchService _loanBatchService;

        public MasterController(ILoanBatchService loanBatchService)
        {
            _loanBatchService = loanBatchService;
        }
        #endregion

        #region Methods
        [HttpGet("item-units")]
        public async Task<IActionResult> GetItemUnitsAsync()
        {
            return Ok(ApiResult<IEnumerable<SelectItemModel>>.Success(await _loanBatchService.GetItemUnitsAsync()));
        }
        #endregion
    }
}
