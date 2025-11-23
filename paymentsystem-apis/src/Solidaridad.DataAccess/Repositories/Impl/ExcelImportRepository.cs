using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Excel.Import;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class ExcelImportRepository : BaseRepository<ExcelImport>, IExcelImportRepository
{
    public ExcelImportRepository(DatabaseContext context) : base(context) { }
}

public class ExcelImportDetailRepository : BaseRepository<ExcelImportDetail>, IExcelImportDetailRepository
{
    public ExcelImportDetailRepository(DatabaseContext context) : base(context) { }
}
