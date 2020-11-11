using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TalentPool.Infrastructure.Extensions
{
    public static class EnumerationExtensions
    {
        public static string GetDescription(this object value, string defaultReplacement = "")
        {
            if (value == null)
                return defaultReplacement;
            Type enumType = value.GetType();
            string name = Enum.GetName(enumType, value);
            if (!string.IsNullOrEmpty(name))
            {
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    if (Attribute.GetCustomAttribute(fieldInfo,
                    typeof(DisplayAttribute), false) is DisplayAttribute displayAttribute)
                    {
                        return displayAttribute.Name;
                    }

                }
            }
            return defaultReplacement;
        }
    }
}
