using Authenticity.CourtSide.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Authenticity.CourtSide.Core.Helpers
{
    public static class ErrorResponseHelper
    {
        private const string GENERIC_RESPONSE_ERROR = "An unexpected error has ocurred, please contact the administrator";

        public const string GENERIC_ERROR_INSERT_DUPLICATED_KEY = "cannot insert duplicate key";

        public static BaseResponse<IEnumerable<object>>  GetErrorResponse(Exception ex, string methodExecuted, ILogger logger)
        {
            return GetCustomErrorResponse(ex, methodExecuted, logger, GENERIC_RESPONSE_ERROR);
        }

        public static BaseResponse<IEnumerable<object>> GetOriginalErrorResponse(Exception ex, string methodExecuted, ILogger logger)
        {
            return GetCustomErrorResponse(ex, methodExecuted, logger, ex.Message);
        }

        public static BaseResponse<IEnumerable<object>> GetCustomErrorResponse(Exception ex, string methodExecuted, ILogger logger, string customMessage = "")
        {
            if (logger != null)
            {
                logger.LogError(ex.Message, methodExecuted, ex);
            }

            BaseResponse<IEnumerable<object>> response = new BaseResponse<IEnumerable<object>>()
            {
                Success = false,
                Message = customMessage,
                Data = Enumerable.Empty<object>()
            };

            return response;
        }

        public static BaseResponse<IEnumerable<object>> GetErrorResponse(string message)
        {
            BaseResponse<IEnumerable<object>> response = new BaseResponse<IEnumerable<object>>()
            {
                Success = false,
                Message = message,
                Data = Enumerable.Empty<object>()
            };

            return response;
        }
    }
}
