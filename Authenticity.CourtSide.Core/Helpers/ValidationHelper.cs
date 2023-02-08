using System;
using System.Collections.Generic;
using System.Linq;

namespace Authenticity.CourtSide.Core.Helpers
{
    public static class ValidationHelper
    {
        private const string GENERIC_RESPONSE_ERROR = "An unexpected error has ocurred, please contact the administrator";

        public static void EnsureHasValue(string value, string propertyName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{propertyName} cannot be null or empty");
            }
        }

        public static int EnsureIntValue(int value, string propertyName = null, bool validatePositiveInteger = true)
        {
            if (validatePositiveInteger && value <= 0)
            {
                throw new ArgumentException($"Invalid {propertyName} value");
            }

            return value;
        }

        public static void EnsureListHasItems<T>(IEnumerable<T> list, string propertyName)
        {
            if (list == null)
            {
                throw new ArgumentException($"The {propertyName} list is null");
            }
            else if (!list.Any())
            {
                throw new ArgumentException($"The {propertyName} list is empty");
            }
        }

        public static void EnsureDtoIsNotNull(object dtoModel)
        {
            if (dtoModel == null)
            {
                throw new ArgumentException(GENERIC_RESPONSE_ERROR);
            }
        }
    }
}
