using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Excel.Import;
namespace Solidaridad.DataAccess.Repositories;

public interface IExcelImportRepository: IBaseRepository<ExcelImport> { }

public interface IExcelImportDetailRepository : IBaseRepository<ExcelImportDetail> { }

