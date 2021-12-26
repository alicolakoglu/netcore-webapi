using System;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ExampleProject.Core
{
    public static class Utilities
    {
        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static string StringAddSpaces(string str, int characterCount)
        {
            if (str.Length <= characterCount)
                return str;

            return Regex.Replace(str, ".{" + characterCount + "}", "$0 ").Trim();
        }
    }
}
