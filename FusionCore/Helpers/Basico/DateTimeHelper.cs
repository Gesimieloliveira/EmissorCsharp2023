using System;

namespace FusionCore.Helpers.Basico
{
    public static class DateTimeHelper
    {
        public static DateTime? Parse(string dateString)
        {
            try
            {
                return DateTime.Parse(dateString);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}