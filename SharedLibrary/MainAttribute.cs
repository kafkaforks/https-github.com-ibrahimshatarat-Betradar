using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class MainAttribute : Attribute
    {

    }
    public static class AttributesHelperExtension
    {
        public static string ToDescription(this Enum value)
        {
            var da =  (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }

   
    public static class Helpers
    {
    }
}
