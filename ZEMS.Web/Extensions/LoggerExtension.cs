using Microsoft.Extensions.Logging;
using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using ZEMS.Web.AppException;
using Correlate;
using ZEMS.Application;
using ZEMS.Application.Exception;

namespace ZEMS.Web.Extensions
{
    public static class LoggerExtension
    {
        public static string CustomErrorLogger(this ILogger logger, Exception ex, ICorrelationContextAccessor _correlationContext, string methodName, object parameter = null)
        {
            logger.LogError(ex, Resource.MessagePatternErrorLog, methodName, parameter);
            string traceId = " / " + "Trace Id: " + _correlationContext.CorrelationContext.CorrelationId;
            if (ex is DbUpdateException)
            {
                SqlException sqlException = (SqlException)ex.InnerException;
                if (sqlException != null && (sqlException.Number == 2627 || sqlException.Number == 2601))
                {
                    Regex regex = new Regex(@"[^.]* The duplicate key value is [^.]*\.");
                    Match match = regex.Match(sqlException.Message);
                    if (match.Success)
                    {
                        return string.Format("The record already exists.  {0}", match.Value) + traceId; ;
                    }
                    else
                    {
                        return "A database error occured" + traceId;
                    }
                }
				else if (sqlException != null && (sqlException.Number == 515))
                {
                    return " There are missing required fields.  Please check the file to ensure all required fields are filled up.." + traceId;
                }
                else if (sqlException != null && (sqlException.Number == 547))
                {
                    return " The conflict occurred in database.." + traceId;
                }
            }
            else if (ex is ValidationException)
            {
                return ex.Message ?? "Error occured" + traceId;
            }
            else if (ex is ModelStateException)
            {
                return ex.Message.ToString() + traceId;
            }
            else if (ex is UnAuthorizedException)
            {
                return Resource.PromptUnAuthorized + traceId;
            }
            return Resource.PromptMessageDefaultError + traceId;
        }
    }
}
