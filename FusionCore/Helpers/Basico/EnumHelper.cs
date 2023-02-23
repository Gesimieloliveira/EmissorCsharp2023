using System;
using System.ComponentModel;

namespace FusionCore.Helpers.Basico
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum @enum)
        {
            var type = @enum.GetType();
            var memInfo = type.GetMember(@enum.ToString());

            if (memInfo.Length <= 0)
                return string.Empty;

            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0
                ? ((DescriptionAttribute) attrs[0]).Description
                : @enum.ToString();
        }
    }
}