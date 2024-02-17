using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Models.Models;

namespace University.WebApi.Mapping
{
    public static class EnumNameExtension
    {
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));

            return displayAttribute?.Name ?? value.ToString();
        }
    }
}
