using System.Net;
using Microsoft.Data.SqlClient;

namespace IMDBApi_Assignment4.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            object response;

            switch (exception)
            {
                case SqlException sqlEx when sqlEx.Number == 547: // Foreign key constraint violation
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    response = new { message = GetForeignKeyConstraintMessage(sqlEx) };
                    break;

                case SqlException sqlEx:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new { message = "A database error occurred.", details = sqlEx.Message };
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new { message = "An internal server error occurred." };
                    break;
            }

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }

        private static string GetForeignKeyConstraintMessage(SqlException sqlException)
        {
            var message = sqlException.Message.ToLower();

            if (message.Contains("actor"))
                return "Cannot delete actor because it is referenced by one or more movies.";
            if (message.Contains("producer"))
                return "Cannot delete producer because it is referenced by one or more movies.";
            if (message.Contains("genre"))
                return "Cannot delete genre because it is referenced by one or more movies.";

            return "Cannot delete this record because it is referenced by other data.";
        }
    }
}