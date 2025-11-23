using Solidaridad.Application.Exceptions;
using Solidaridad.Application.Models;
using Solidaridad.Core.Exceptions;
using Newtonsoft.Json;
using Solidaridad.Core.Entities;
using Npgsql;
using Dapper.Contrib.Extensions;
using Dapper;


namespace Solidaridad.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public ExceptionHandlingMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ExceptionHandlingMiddleware> logger)
     {
        _next = next;
        _configuration = configuration;
        _logger = logger;
        _connectionString = _configuration.GetSection("ConnectionStrings").Get<DatabaseConfiguration>().DefaultConnection;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception manually
            await LogException(ex);

            await HandleException(context, ex);
        }
    }

    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    private async Task LogException(Exception ex)
    {
        //  Create a new log entry
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Level = "Error",
            Message = ex.Message,
            Exception = ex.ToString(),
            StackTrace = ex.StackTrace,
        };

        // Insert the log entry into the database
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

    private Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex.Message);

        var code = StatusCodes.Status500InternalServerError;
        var errors = new List<string> { ex.Message };

        code = ex switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ResourceNotFoundException => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            UnprocessableRequestException => StatusCodes.Status422UnprocessableEntity,
            _ => code
        };

        var result = JsonConvert.SerializeObject(ApiResult<string>.Failure(errors));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;

        return context.Response.WriteAsync(result);
    }
}

public class DatabaseConfiguration
{
    public string DefaultConnection { get; set; }
}
