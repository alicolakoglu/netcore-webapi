using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
using System.Globalization;

namespace ExampleProject.Api
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
        public static List<int> GetYearList(int startYear, int endYear)
        {
            var list = new List<int>();
            for (int i = startYear; i <= endYear; i++)
                list.Add(i);
            return list;
        }

        public static string ToNormalize(this string text)
        {
            return String.Join("", text.Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
        }
        public static string ConvertStringToMD5(string clearText)
        {
            byte[] ByteData = Encoding.ASCII.GetBytes(clearText);
            MD5 oMd5 = MD5.Create();
            byte[] HashData = oMd5.ComputeHash(ByteData);
            StringBuilder oSb = new StringBuilder();
            for (int x = 0; x < HashData.Length; x++)
                oSb.Append(HashData[x].ToString("x2"));
        
            return oSb.ToString();
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
