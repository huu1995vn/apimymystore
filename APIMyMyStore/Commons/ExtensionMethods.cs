using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
namespace APIMyMyStore
{
    public static class ExtensionMethods
    {
        // Assembly
        // GetLinkerTime
        public static string GetBuildDateFromAssembly(this Assembly assembly, TimeZoneInfo target = null)
        {
            DateTime? localTime = null;
            string res = "";
            try
            {
                var filePath = assembly.Location;

                FileInfo fi = new FileInfo(filePath);
                localTime = fi.LastWriteTime;
            }
            catch (Exception)
            {
                res = "GetBuildDateFromAssembly lỗi";
            }

            if (localTime.HasValue)
            {
                res = localTime.Value.ToString("dd-MM-yyyy HH:mm:ss");
            }

            return res;
        }
        public static List<Dictionary<string, object>> DataTableToList(this DataTable dt)
        {

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, (Convert.IsDBNull(dr[col]) ? null : dr[col]));
                }
                rows.Add(row);
            }
            return rows;
        }
        public static string RemoveLastChar(this String value, string charValue = "")
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (string.IsNullOrEmpty(charValue))
                {
                    return value.Substring(0, value.Length - 1);
                }
                else
                {
                    return (value.EndsWith(charValue) ? value.Substring(0, value.Length - 1) : value);
                }
            }

            return value;
        }
        public static string AddLastChar(this String value, string charValue)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return (value.EndsWith(charValue) ? value : (value + charValue));
            }

            return value;
        }
        public static bool IsPhoneNumber(this string number)
        {
            return Regex.IsMatch(number, @"[0-9]+") && number.Length == 10 && number.StartsWith("0") && number[1] != 0;
        }
        public static bool IsEmail(this string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsUrl(this string url)
        {
            try
            {
                Uri uriResult;
                return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }




    }
}