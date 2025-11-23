//using LinqToDB;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using NPOI.SS.UserModel;
//using NPOI.XSSF.UserModel;
//using Solidaridad.Core.Entities;
//using Solidaridad.Core.Entities.Excel.Shared;
//using Solidaridad.DataAccess.Persistence;
//using System.Text;
//using ExcelImport = Solidaridad.Core.Entities.ExcelImport;

//namespace Solidaridad.DataAccess.Repositories.Impl
//{
//    public class ExcelImportRepository_not_in_use : IExcelImportRepository
//    {
//        public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
//        #region DI
//        protected readonly DatabaseContext Context;
//        protected readonly DbSet<ExcelImport> DbSet;

//        public ExcelImportRepository_not_in_use(DatabaseContext context)
//        {
//            Context = context;
//            DbSet = context.Set<ExcelImport>();

//        }
//        #endregion

//        public async Task<IEnumerable<ExcelImport>> GetImportedFilesHistory(ExcelApiSearchParams searchParams)
//        {
//            try
//            {
//                var query = Context.Set<ExcelImport>().AsQueryable();

//                if (!string.IsNullOrEmpty(searchParams.ModuleName))
//                {
//                    query = query.Where(e => e.Module == searchParams.ModuleName);
//                }

//                if (searchParams.FileName != null && searchParams.FileName.Count > 0)
//                {
//                    var fileNames = searchParams.FileName.ToArray();
//                    query = query.Where(e => fileNames.Contains(e.Filename));
//                }

//                if (searchParams.Date != null && searchParams.Date.Count > 0)
//                {
//                    var dates = searchParams.Date.Select(d => DateTime.Parse(d)).ToArray();
//                    query = query.Where(e => dates.Contains(e.ImportedDateTime.Date));
//                }

//                return query;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        public async Task<List<Farmer>> ImportFarmers(IFormFile file, int importId)
//        {
//            try
//            {
//                StringBuilder sb = new StringBuilder();

//                var _listFarmers = new List<Farmer>();


//                #region Read Data
//                ReadFarmers(file, _listFarmers);


//                #endregion

//                #region Insert Data
//                await InsertFarmers(_listFarmers, importId);
//                #endregion

//                return _listFarmers;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<int> SaveExcelImport(ExcelImport excelImport)
//        {
//            try
//            {
//                var addedEntity = (await DbSet.AddAsync(excelImport)).Entity;
//                await Context.SaveChangesAsync();

//                return addedEntity != null ? 1 : 0;
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public async Task<bool> UpdateExcelImport(ExcelImport excelImport)
//        {
//            try
//            {
//                var updatedEntity = DbSet.Update(excelImport);
//                await Context.SaveChangesAsync();

//                return updatedEntity != null ? true : false;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private void ReadFarmers(IFormFile file, List<Farmer> _listFarmers)
//        {
//            XSSFWorkbook hssfwb;
//            using (var stream = file.OpenReadStream())
//            {
//                hssfwb = new XSSFWorkbook(stream);
//            }

//            ISheet sheet = hssfwb.GetSheet(Convert.ToString($"WorkSheet"));

//            // check if sheet exists
//            if (sheet != null)
//            {
//                for (int row = 1; row <= sheet.LastRowNum; row++)
//                {
//                    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
//                    {
//                        //var val = sheet.GetRow(row).GetCell(1)?.StringCellValue;
//                        int col = 0;
//                        _listFarmers.Add(new Farmer
//                        {

//                            //FirstName = ParseUtility.ParseAnyValueToString(sheet.GetRow(row).GetCell(col++)?.StringCellValue),
//                            //OtherNames = ParseUtility.ParseAnyValueToString(sheet.GetRow(row).GetCell(col++)?.StringCellValue),
//                            //CreatedBy= ParseUtility.ParseAnyValueToString(sheet.GetRow(row).GetCell(col++)?.StringCellValue),
//                            //ImportId = ParseUtility.ParseIntValue(sheet.GetRow(row).GetCell(col++)?.StringCellValue),
//                            //Address = new Address
//                            //{
//                            //    Street = ParseUtility.ParseAnyValueToString(sheet.GetRow(row).GetCell(col++)?.StringCellValue),
//                            //    City = ParseUtility.ParseAnyValueToString(sheet.GetRow(row).GetCell(col++)?.StringCellValue),
//                            //    State = ParseUtility.ParseAnyValueToString(sheet.GetRow(row).GetCell(col++)?.StringCellValue),
//                            //    ZipCode = ParseUtility.ParseAnyValueToString(sheet.GetRow(row).GetCell(col++)?.NumericCellValue),
//                            //    Country = ParseUtility.ParseAnyValueToString(sheet.GetRow(row).GetCell(col++)?.StringCellValue),

//                            //}
//                        });
//                    }
//                }
//            }
//        }





//        private async Task InsertFarmers(List<Farmer> listFarmers, int importId)
//        {
//            try
//            {

//                foreach (var farmer in listFarmers)
//                {
//                    farmer.ImportId = importId;

//                    //farmer.CreatedBy = _userContext.CurrentUser.Id;

//                    Context.Farmers.Add(farmer);
//                }

//                await Context.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }







//    }
//}
