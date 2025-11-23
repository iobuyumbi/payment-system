using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.Services.Impl;

public class ErrorHandlerService: IErrorHandlerService
{
    #region DI
    private readonly string _connectionString;
    private readonly IConfiguration _configuration;


    public ErrorHandlerService(IConfiguration configuration)
    {
        _connectionString = _configuration.GetSection("ConnectionStrings").Get<DatabaseConfiguration>().DefaultConnection;
    }
    #endregion

    #region Methods
    public async Task AddToLog(LogEntry logEntry)
    {
        try
        {
            using (var connection = GetConnection())
            {
                string sql = "INSERT INTO public.\"LogEntry\"( \"Timestamp\", \"Level\", \"Message\", \"Exception\", \"StackTrace\") " +
                    "VALUES ( @Timestamp, @Level, @Message, @Exception, @StackTrace )";

                var parameters = new DynamicParameters();
                parameters.Add("@Timestamp", logEntry.Timestamp);
                parameters.Add("@Level", logEntry.Level);
                parameters.Add("@Message", logEntry.Message);
                parameters.Add("@Exception", logEntry.Exception);
                parameters.Add("@StackTrace", logEntry.StackTrace);

                await connection.ExecuteAsync(sql, parameters);
            }
        }
        catch (Exception exp)
        {

        }
    } 
    #endregion

    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    protected class DatabaseConfiguration
    {
        public string DefaultConnection { get; set; }
    }

}
