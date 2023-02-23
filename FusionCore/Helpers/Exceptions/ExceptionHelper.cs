using System;
using System.Collections.Generic;

namespace FusionCore.Helpers.Exceptions
{
    public static class ExceptionHelper
    {
        public static IEnumerable<string> GetAllMessages(this Exception ex)
        {
            var list = new List<string> { ex.Message };
            var next = ex.InnerException;

            while (next != null)
            {
                list.Add(next.Message);
                next = next.InnerException;
            }

            return list;
        }

        public static IEnumerable<Exception> GetAllException(this Exception ex)
        {
            var list = new List<Exception> {ex};
            var nextException = ex.InnerException;

            while (nextException != null)
            {
                list.Add(nextException);
                nextException = nextException.InnerException;
            }

            return list;
        }
    }
}