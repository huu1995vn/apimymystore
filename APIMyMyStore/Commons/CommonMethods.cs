using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text.Json;
using System.Security.Cryptography;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Data;
using System.ComponentModel;

namespace APIMyMyStore
{
    public class CommonMethods
    {
        #region "Captcha"

        /// <summary>
        /// return int number with
        /// 1  : Code captcha valid
        /// -2 : Code captcha invalid
        /// -1 : Captcha exprired => request a new capcha
        /// 0 : orther
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pInput"></param>
        /// <returns></returns>
        public static int Captcha_CheckValid(string pId, string pInput)
        {
            try
            {
                string[] items = DecryptStringAes(ConvertBase64FromUrl(pId)).Split('.');
                if (DateTime.Now.Ticks < ConvertToInt64(items[1]))
                {
                    return items[0].Equals(pInput) ? 1 : -2;
                }
                return -1;
            }
            catch { }
            return 0;
        }

        public static string Captcha_GetCode(string pKey)
        {
            try
            {
                string[] items = DecryptStringAes(ConvertBase64FromUrl(pKey)).Split('.');
                if (DateTime.Now.Ticks < ConvertToInt64(items[1]))
                {
                    return items[0];
                }
            }
            catch { }
            return string.Empty;
        }

        public static string Captcha_BuildKey()
        {
            string code = GetRandomString(CommonConstants.CAPTCHA_LENGTH);
            long expired = DateTime.Now.AddMinutes(CommonConstants.CAPTCHA_DURATION).Ticks;
            string key = $"{code}.{expired}";
            return ConvertToBase64Url(EncryptStringAes(key));
        }

        #endregion

        #region "Verify"

        public static bool Verify_CheckExpired(string pCode)
        {
            try
            {
                string[] items = DecryptStringAes(ConvertBase64FromUrl(pCode)).Split('.');
                if (items.Length > 2)
                {
                    return DateTime.Now.Ticks > ConvertToInt64(items[2]);
                }
            }
            catch { }
            return true;
        }

        public static long Verify_GetId(string pCode)
        {
            try
            {
                string[] items = DecryptStringAes(ConvertBase64FromUrl(pCode)).Split('.');
                if (items.Length > 2 && DateTime.Now.Ticks < ConvertToInt64(items[2]))
                {
                    return CommonMethods.ConvertToInt64(items[0]);
                }
            }
            catch { }
            return -1;
        }

        public static string Verify_GetValue(string pCode)
        {
            try
            {
                string[] items = DecryptStringAes(ConvertBase64FromUrl(pCode)).Split('.');
                if (items.Length > 2 && DateTime.Now.Ticks < ConvertToInt64(items[2]))
                {
                    return items[1];
                }
            }
            catch { }
            return string.Empty;
        }

        public static string Verify_CreateCodeNumber(long pId)
        {
            string code = GetRandomNumber(CommonConstants.VERIFY_LENGTH);
            long expired = DateTime.Now.AddHours(CommonConstants.VERIFY_DURATION_HOUR).Ticks;
            string key = $"{pId}.{code}.{expired}";
            return ConvertToBase64Url(EncryptStringAes(key));
        }

        public static string Verify_CreateCodePassword(long pId)
        {
            string code = GetRandomPassword(CommonConstants.VERIFY_LENGTH);
            long expired = DateTime.Now.AddHours(CommonConstants.VERIFY_DURATION_HOUR).Ticks;
            string key = $"{pId}.{code}.{expired}";
            return ConvertToBase64Url(EncryptStringAes(key));
        }

        #endregion

        #region "Verify Email"

        public static bool VerifyEmail_CheckExpired(string pCode)
        {
            try
            {
                string[] items = DecryptStringAes(ConvertBase64FromUrl(pCode)).Split('|');
                if (items.Length > 3)
                {
                    return DateTime.Now.Ticks > ConvertToInt64(items[3]);
                }
            }
            catch { }
            return true;
        }

        public static long VerifyEmail_GetId(string pCode)
        {
            try
            {
                string[] items = DecryptStringAes(ConvertBase64FromUrl(pCode)).Split('|');
                if (items.Length > 3 && DateTime.Now.Ticks < ConvertToInt64(items[3]))
                {
                    return CommonMethods.ConvertToInt64(items[0]);
                }
            }
            catch { }
            return -1;
        }

        public static string VerifyEmail_GetEmail(string pCode)
        {
            try
            {
                string[] items = DecryptStringAes(ConvertBase64FromUrl(pCode)).Split('|');
                if (items.Length > 3 && DateTime.Now.Ticks < ConvertToInt64(items[3]))
                {
                    return items[1];
                }
            }
            catch { }
            return string.Empty;
        }

        public static string VerifyEmail_GetValue(string pCode)
        {
            try
            {
                string[] items = DecryptStringAes(ConvertBase64FromUrl(pCode)).Split('|');
                if (items.Length > 3 && DateTime.Now.Ticks < ConvertToInt64(items[3]))
                {
                    return items[2];
                }
            }
            catch { }
            return string.Empty;
        }

        public static string VerifyEmail_CreateCodeNumber(long pId, string pEmail)
        {
            string code = GetRandomNumber(CommonConstants.VERIFY_EMAIL_LENGTH);
            long expired = DateTime.Now.AddHours(CommonConstants.VERIFY_DURATION_HOUR).Ticks;
            string key = $"{pId}|{pEmail}|{code}|{expired}";
            return ConvertToBase64Url(EncryptStringAes(key));
        }

        #endregion
        #region "Convert Data"

        public static List<Dictionary<string, string>> ConvertDataToList(System.Data.DataTable pData)
        {
            List<Dictionary<string, string>> res = new List<Dictionary<string, string>>();
            foreach (System.Data.DataRow row in pData.Rows)
            {
                res.Add(ConvertDataToDictionary(row, pData.Columns));
            }
            return res;
        }

        public static Dictionary<string, string> ConvertDataToDictionary(System.Data.DataRow pData, System.Data.DataColumnCollection pColumns)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (System.Data.DataColumn col in pColumns)
            {
                res.Add(col.ColumnName, ConvertToString(pData[col.ColumnName]));
            }
            return res;
        }


        public static string GetContentType(string pExtension)
        {
            string res;
            switch (pExtension)
            {
                case "png":
                    res = "image/png";
                    break;
                case "jpg":
                case "jpge":
                    res = "image/jpg";
                    break;
                case "bmp":
                    res = "image/bmp";
                    break;
                case "gif":
                    res = "image/gif";
                    break;
                case "ico":
                    res = "image/x-icon";
                    break;
                case "svg":
                    res = "image/svg+xml";
                    break;
                case "webp":
                    res = "image/webp";
                    break;
                case "doc":
                case "docx":
                case "xls":
                case "xlsx":
                case "pdf":
                    res = "application/" + pExtension;
                    break;
                case "css":
                    res = "text/css";
                    break;
                case "js":
                    res = "application/javascript";
                    break;
                default:
                    res = "file/" + pExtension;
                    break;
            };
            return res;
        }

        #endregion

        #region "Commons"

        public static string GetRandomNumber(int pLength)
        {
            string Str = "0123456789";
            string Result = string.Empty;
            Random rand = new Random();
            for (int i = 0; i < pLength; i++)
            {
                Result += Str[rand.Next(Str.Length)];
            }
            return Result;
        }

        public static string GetRandomString(int pLength)
        {
            string Str = "abcdefghijklmnpqrtuvswzxy0123456789";
            string Result = string.Empty;
            Random rand = new Random();
            for (int i = 0; i < pLength; i++)
            {
                Result += Str[rand.Next(Str.Length)];
            }
            return Result;
        }

        public static string GetRandomPassword(int pLength)
        {
            string Str = "aAbBcCdDeEfFgGh012HiIjJkKlLmMnN345pPqQrRtTuUvVsSwWzZxXyY6789";
            string Result = string.Empty;
            Random rand = new Random();
            for (int i = 0; i < pLength; i++)
            {
                Result += Str[rand.Next(Str.Length)];
            }
            return Result;
        }

        public static string FormatUrl(string pUrl)
        {
            if (pUrl.Contains("http://") || pUrl.Contains("https://"))
            {
                return pUrl;
            }
            return $"http://{pUrl}";
        }

        public static string FormatUrlSSL(string pUrl, bool pIsSSL)
        {
            pUrl = RemoveHttpUrl(pUrl);
            return pIsSSL ? $"https://{pUrl}" : $"http://{pUrl}";
        }

        public static string RemoveHttpUrl(string pUrl)
        {
            if (pUrl.StartsWith("http://"))
            {
                return pUrl.Substring(7);
            }

            if (pUrl.StartsWith("https://"))
            {
                return pUrl.Substring(8);
            }
            return pUrl;
        }

        public static string GetDomainByUrl(string pUrl)
        {
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(pUrl, @"^(?:\w+://)?([^/?#]*)", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }

        public static List<string> GetLinkFromContent(string pContent)
        {
            List<string> lstLinks = new List<string>();
            System.Text.RegularExpressions.Match mLink;
            System.Text.RegularExpressions.MatchCollection lstMatchs = System.Text.RegularExpressions.Regex.Matches(pContent, "<a([^>]*)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            foreach (System.Text.RegularExpressions.Match item in lstMatchs)
            {
                mLink = System.Text.RegularExpressions.Regex.Match(item.Value, "href='([^']*)'", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (mLink.Success)
                {
                    if (!string.IsNullOrEmpty(mLink.Groups[1].Value))
                    {
                        lstLinks.Add(mLink.Groups[1].Value);
                    }
                }
            }
            return lstLinks;
        }

        public static string FormatQueryLink(string pLink)
        {
            return pLink.Replace("%3F", "?").Replace("%3D", "=").Replace("%26", "&");
        }

        public static string FilterQuerySearch(string pText)
        {
            return RemoveExtraWhiteSpaces(pText);
        }

        public static string FilterNumber(string pText)
        {
            return System.Text.RegularExpressions.Regex.Replace(pText, "[^0-9]+", string.Empty);
        }

        public static string FilterName(string pText)
        {
            pText = FilterHtmlTag(pText);
            pText = RemoveExtraWhiteSpaces(pText);
            pText = pText.Replace("\"", "");
            return pText;
        }

        public static string FilterUserName(string pText)
        {
            pText = ConvertUnicodeToASCII(pText.ToLower().Trim());
            return System.Text.RegularExpressions.Regex.Replace(pText, "[^a-z0-9]", string.Empty);
        }

        public static string FilterCode(string pText)
        {
            pText = ConvertUnicodeToASCII(pText.ToLower().Trim());
            return System.Text.RegularExpressions.Regex.Replace(pText, "[^a-z0-9_-]", string.Empty);
        }

        public static string FilterHtmlTag(string pText)
        {
            return pText.Replace("<", "&lt;").Replace(">", "&gt;").Replace("{", "").Replace("}", "").Trim();
        }

        public static string FilterTitleTag(string pText)
        {
            return pText.Replace("\"", "").Trim();
        }

        public static string FormatNumber(decimal pNumber)
        {
            string res = (pNumber != 0) ? pNumber.ToString("###,###") : "0";
            //res = res.Replace(",", ".");
            return res;
        }

        public static string FormatNumber(decimal pNumber, string pSplitChar)
        {
            return (pNumber != 0) ? pNumber.ToString($"###{pSplitChar}###") : "0";
        }

        public static string FormatNumber(int pNumber, int pLengthString)
        {
            return pNumber.ToString().PadLeft(pLengthString, '0');
        }

        public static string FormatNumberDecimal(decimal pNumber, int pDecimalLength)
        {
            return (pNumber != 0) ? pNumber.ToString($"###,###.{new string('#', pDecimalLength)}") : "0";
        }

        public static string FormatNumberSEO(decimal pNumber)
        {
            return (pNumber != 0) ? pNumber.ToString("###,###.#") : "0";
        }

        public static string FormatMoney(decimal pNumber)
        {
            decimal temp = pNumber / 1000000000;
            if (temp >= 1)
            {
                return FormatNumberDecimal(Math.Round(temp, 3), 3) + " tỷ";
            }

            temp = pNumber / 1000000;
            if (temp >= 1)
            {
                return FormatNumberDecimal(Math.Round(temp, 1), 1) + " triệu";
            }

            return FormatNumber(pNumber);
        }

        public static string FormatMoneyToMillion(decimal pNumber)
        {
            decimal temp = pNumber / 1000000;
            if (temp >= 1)
            {
                string res = FormatNumber(Math.Round(temp, 0)).Replace(',', '.');
                return res + " triệu";
            }
            return FormatNumber(pNumber);
        }

        public static string FormatRewriteUrl(string pText)
        {
            string result = ConvertUnicodeToASCII(pText.ToLower().Trim());

            result = System.Text.RegularExpressions.Regex.Replace(result, "[^a-zA-Z0-9]", "-");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"[-]+", "-");
            if (result.StartsWith("-"))
            {
                result = result.Substring(1);
            }
            if (result.EndsWith("-"))
            {
                result = result[0..^1];
            }

            return result;
        }

        public static string RemoveExtraWhiteSpaces(string pText)
        {
            pText = pText.Trim();
            pText = System.Text.RegularExpressions.Regex.Replace(pText, @"[ ]+", " ");
            return pText;
        }

        public static string RemoveHtmlTag(string pText)
        {
            return System.Text.RegularExpressions.Regex.Replace(pText, @"<([^>]*)>", string.Empty);
        }

        public static string FormatFirstCharToUpper(string pText)
        {
            return pText.First().ToString().ToUpper() + pText.Substring(1);
        }

        public static string[] SplitPhone(string pText)
        {
            return pText.Split('-');
        }

        public static string GetPhoneText(string pText)
        {
            string PhoneText = string.Empty;
            string[] lstPhone = SplitPhone(pText);
            string temp;
            foreach (string item in lstPhone)
            {
                temp = FilterNumber(item);
                if (!string.IsNullOrEmpty(temp))
                {
                    PhoneText += string.IsNullOrEmpty(PhoneText) ? temp : ";" + temp;
                }
            }
            return PhoneText;
        }

        public static string GetPhoneTel(string pText)
        {
            string[] lstPhone = SplitPhone(pText);
            return FilterNumber(lstPhone[0]);
        }

        public static int CalDiscount(decimal pPrice, decimal pDiscountPrice)
        {
            if (pPrice == 0 || pDiscountPrice == 0)
            {
                return 0;
            }
            return (int)((pPrice - pDiscountPrice) / pPrice * 100);
        }
        #endregion

        #region "Convert Object"

        public static int ConvertToInt32(object pNumber)
        {
            try
            {
                return Convert.ToInt32(pNumber);
            }
            catch
            {
                return 0;
            }
        }

        public static long ConvertToInt64(object pNumber)
        {
            try
            {
                return Convert.ToInt64(pNumber);
            }
            catch
            {
                return 0;
            }
        }

        public static decimal ConvertToDecimal(object pNumber)
        {
            try
            {
                return Convert.ToDecimal(pNumber);
            }
            catch
            {
                return 0;
            }
        }

        public static double ConvertToDouble(object pNumber)
        {
            try
            {
                return Convert.ToDouble(pNumber);
            }
            catch
            {
                return 0;
            }
        }

        public static string ConvertToString(object pString)
        {
            try
            {
                return Convert.ToString(pString);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool ConvertToBoolean(object pBoolean)
        {
            try
            {
                return Convert.ToBoolean(pBoolean);
            }
            catch
            {
                return false;
            }
        }

        public static DateTime ConvertToDateTime(object pDate)
        {
            try
            {
                return Convert.ToDateTime(pDate);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static object ConvertToDateTime(string pDate, string pFormat = CommonConstants.DD_MM_YYYY)
        {
            try
            {
                return DateTime.ParseExact(pDate, pFormat, null);
            }
            catch
            {
                return DBNull.Value;
            }
        }

        public static DateTime ConvertUnixTimeStampToDateTime(double pUnixTimeStamp)
        {
            try
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(pUnixTimeStamp).ToLocalTime();
                return dateTime;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static decimal ConvertPercentRating(decimal pRatingValue)
        {
            // Tính trên thang điểm 5
            return pRatingValue / 5 * 100;
        }

        public static string ConvertUnicodeToASCII(string text)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.ToLower().Normalize(System.Text.NormalizationForm.FormD);
            string res = regex.Replace(strFormD, String.Empty).Replace('đ', 'd');
            return res;
        }

        public static string ConvertKeywordSearchs(params object[] pTexts)
        {
            List<string> listSearch = new List<string>();
            foreach (var text in pTexts)
            {
                var txtSearch = ConvertToString(text);
                if (!string.IsNullOrEmpty(txtSearch))
                {
                    txtSearch = ConvertUnicodeToASCII(txtSearch);
                    listSearch.Add(txtSearch);
                }
            }

            return string.Join(";", listSearch.ToArray());
        }

        public static string ConvertToBase64Url(string pData)
        {
            return pData.TrimEnd('=').Replace('+', '_').Replace('/', '-');
        }

        public static string ConvertBase64FromUrl(string pData)
        {
            string res = pData.Replace('_', '+').Replace('-', '/');
            switch (pData.Length % 4)
            {
                case 2: res += "=="; break;
                case 3: res += "="; break;
            }
            return res;
        }

        /// <summary>
        /// Lọc ra những Id lớn hơn 0
        /// </summary>
        /// <param name="pIdList">Kiểu dữ liệu: ,1,2,0,3,</param>
        /// <returns>[1,2,3]</returns>
        public static List<long> ParseStringToList(string pIdList)
        {
            List<long> lstData = new List<long>();
            if (!string.IsNullOrEmpty(pIdList))
            {
                string[] arrData = pIdList.Split(',');
                long value;
                foreach (string item in arrData)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        value = ConvertToInt64(item);
                        if (value > 0)
                        {
                            lstData.Add(value);
                        }
                    }
                }
            }
            return lstData;
        }

        /// <summary>
        /// Lọc lại những Id lớn hơn 0 trong pIdList
        /// </summary>
        /// <param name="pIdList">Kiểu dữ liệu: ,1,0,3,</param>
        /// <returns>1,3</returns>
        public static string ResetStringList(string pIdList)
        {
            List<long> lstIds = ParseStringToList(pIdList);
            return JoinListToString(lstIds);
        }

        /// <summary>
        /// Lọc lại những Id lớn hơn 0 trong pIdList
        /// </summary>
        /// <param name="pIdList">Kiểu dữ liệu: ["1", "2"]</param>
        /// <returns>1,2</returns>
        public static string ParseStringByList(List<string> pIdList)
        {
            List<long> lstIds = new List<long>();
            long value;
            if (pIdList != null && pIdList.Count > 0)
            {
                foreach (string item in pIdList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        value = ConvertToInt64(item);
                        if (value > 0)
                        {
                            lstIds.Add(value);
                        }
                    }
                }
            }
            return JoinListToString(lstIds);
        }

        public static string JoinListToString(long[] pIdList)
        {
            return string.Join(",", pIdList);
        }

        public static string JoinListToString(List<long> pIdList)
        {
            if (pIdList != null && pIdList.Count > 0)
            {
                return string.Join(",", pIdList);
            }
            return string.Empty;
        }

        public static string JoinListToString(List<string> pIdList)
        {
            if (pIdList != null && pIdList.Count > 0)
            {
                return string.Join(",", pIdList);
            }
            return string.Empty;
        }

        #endregion

        #region "DateTime"

        public static DateTime ParseToDateTime(string pDate, string pFormat = CommonConstants.DD_MM_YYYY)
        {
            try
            {
                return DateTime.ParseExact(pDate, pFormat, null);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string FormatDateTime(DateTime pDateTime)
        {
            return FormatDateTime(pDateTime, CommonConstants.DD_MM_YYYY);
        }

        public static string FormatDateTime(DateTime pDateTime, string pFormat)
        {
            try
            {
                return pDateTime.ToString(pFormat);
            }
            catch
            {
                return "";
            }
        }

        public static DateTime StartOfWeek(DateTime pDate)
        {
            int diff = (7 + (pDate.DayOfWeek - DayOfWeek.Monday)) % 7;
            return pDate.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(DateTime pDate)
        {
            DateTime startOfWeek = StartOfWeek(pDate);
            return startOfWeek.AddDays(6);
        }

        public static DateTime StartOfMonth(DateTime pDate)
        {
            return new DateTime(pDate.Year, pDate.Month, 1);
        }

        public static DateTime EndOfMonth(DateTime pDate)
        {
            return new DateTime(pDate.Year, pDate.Month, 1).AddMonths(1).AddDays(-1);
        }

        #endregion

        #region "Encrypt & Decrypt"

        public static string CreateUniqueID(int pLengthString, long pIntUniqueId)
        {
            Random rand = new Random();
            string keys = "abcdefghijklmnopqrstuvwxyz0123456789";
            string[] inserts = new string[] { "08cf", "3ensu", "67djt", "29bor", "1vhkq", "aw4xy", "g5m", "lipz" };
            long temp = pIntUniqueId;
            string result = string.Empty;
            do
            {
                int du = (int)(temp % keys.Length);
                temp = temp / keys.Length;
                result = string.Concat(keys[du], result);
            } while (temp > 0);

            int size = pLengthString - result.Length - 1;
            string ins = string.Empty;
            if (size >= 0 && size < inserts.Length)
            {
                ins = inserts[size][rand.Next(inserts[size].Length)].ToString();
                for (int i = 0; i < size; i++)
                {
                    ins = string.Concat(ins, keys[rand.Next(keys.Length)]);
                }
                ins = string.Concat(ins, result);
            }
            return ins;
        }

        public static string GetEncryptMD5(string pValue)
        {
            System.Security.Cryptography.MD5 algorithm = System.Security.Cryptography.MD5.Create();
            byte[] data = algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pValue));
            string result = "";
            for (int i = 0; i < data.Length; i++)
            {
                result += data[i].ToString("x2").ToUpperInvariant();
            }
            return result;
        }

        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static string EncodeUnicodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.UnicodeEncoding.Unicode.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        static public string DecodeUnicodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.UnicodeEncoding.Unicode.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static string EncryptStringAes(string plainText)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = CommonConstants.SECURITY_KEY;
                aesAlg.IV = CommonConstants.SECURITY_IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptStringAes(string pEncryptString)
        {
            byte[] cipherText = Convert.FromBase64String(pEncryptString);

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = CommonConstants.SECURITY_KEY;
                aesAlg.IV = CommonConstants.SECURITY_IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        public static void CompressGzipContentFile(string pContent, string pPathFile)
        {
            byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(pContent);
            using (FileStream fs = new FileStream(pPathFile, FileMode.Create))
            {
                using (GZipStream stream = new GZipStream(fs, CompressionMode.Compress))
                {
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    stream.Close();
                }
                fs.Close();
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #endregion

        #region "Property Json"
        public static JsonElement? GetPropertyJsonElement(JsonElement pJson, string pPropertyName)
        {
            try
            {

                return pJson.GetProperty(pPropertyName);
            }
            catch
            {
                return null;
            }
        }
        public static int GetPropertyInt32(JsonElement pJson, string pPropertyName)
        {
            try
            {
                JsonElement json = pJson.GetProperty(pPropertyName);
                if (json.ValueKind == JsonValueKind.String)
                {
                    return ConvertToInt32(json.GetString());
                }
                return json.GetInt32();
            }
            catch
            {
                return 0;
            }
        }

        public static long GetPropertyInt64(JsonElement pJson, string pPropertyName)
        {
            try
            {
                JsonElement json = pJson.GetProperty(pPropertyName);
                if (json.ValueKind == JsonValueKind.String)
                {
                    return ConvertToInt64(json.GetString());
                }
                return json.GetInt64();
            }
            catch
            {
                return 0;
            }
        }

        public static decimal GetPropertyDecimal(JsonElement pJson, string pPropertyName)
        {
            try
            {
                JsonElement json = pJson.GetProperty(pPropertyName);
                if (json.ValueKind == JsonValueKind.String)
                {
                    return ConvertToDecimal(json.GetString());
                }
                return json.GetDecimal();
            }
            catch
            {
                return 0;
            }
        }

        public static double GetPropertyDouble(JsonElement pJson, string pPropertyName)
        {
            try
            {
                JsonElement json = pJson.GetProperty(pPropertyName);
                if (json.ValueKind == JsonValueKind.String)
                {
                    return ConvertToDouble(json.GetString());
                }
                return json.GetDouble();
            }
            catch
            {
                return 0;
            }
        }

        public static string GetPropertyString(JsonElement pJson, string pPropertyName)
        {
            try
            {
                return ConvertToString(pJson.GetProperty(pPropertyName).GetString());
            }
            catch
            {
                return string.Empty;
            }
        }

        public static DateTime GetPropertyDateTime(JsonElement pJson, string pPropertyName, string pFormat = CommonConstants.DD_MM_YYYY)
        {
            string dateTime = GetPropertyString(pJson, pPropertyName);
            return ParseToDateTime(dateTime, pFormat);
        }

        public static Boolean GetPropertyBoolean(JsonElement pJson, string pPropertyName)
        {
            try
            {
                return pJson.GetProperty(pPropertyName).GetBoolean();
            }
            catch
            {
                return false;
            }
        }

        public static long[] GetPropertyArrayInt64(JsonElement pJson, string pPropertyName)
        {
            try
            {
                return pJson.GetProperty(pPropertyName).EnumerateArray().Select(x => x.GetInt64()).ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static List<long> GetPropertyListInt64(JsonElement pJson, string pPropertyName)
        {
            try
            {
                return pJson.GetProperty(pPropertyName).EnumerateArray().Select(x => x.GetInt64()).ToList();
            }
            catch
            {
                return null;
            }
        }

        public static List<string> GetPropertyListString(JsonElement pJson, string pPropertyName)
        {
            try
            {
                return ConvertToListString(pJson.GetProperty(pPropertyName));
            }
            catch
            {
                return null;
            }
        }

        public static List<JsonElement> GetPropertyListJsonElement(JsonElement pJson, string pPropertyName)
        {
            try
            {
                return ConvertToListJsonElement(pJson.GetProperty(pPropertyName));
            }
            catch
            {
                return null;
            }
        }

        public static List<Dictionary<string, string>> GetPropertyListDictionary(JsonElement pJson, string pPropertyName)
        {
            try
            {
                return CommonMethods.ConvertToListDictionary(CommonMethods.ConvertToJsonString(GetPropertyListJsonElement(pJson, pPropertyName)));
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region "Json"

        public static JsonElement ConvertToJsonElement(string pJson)
        {
            return JsonSerializer.Deserialize<JsonElement>(pJson, new JsonSerializerOptions() { AllowTrailingCommas = true });
        }

        public static string ConvertToJsonString(object pObject)
        {
            JsonSerializerOptions jsOptions = new JsonSerializerOptions();
            jsOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize(pObject, jsOptions);
        }

        /// <summary>
        /// [1,2,3,4]
        /// </summary>
        /// <param name="pIdList">Danh sách Ids: 1,2,3,4</param>
        /// <returns></returns>
        public static string ConvertToJsonStringByIdList(string pIdList)
        {
            List<long> lstIds = ParseStringToList(pIdList);
            return ConvertToJsonString(lstIds);
        }

        public static void ConvertToTable(JsonElement.ArrayEnumerator pDataSet, int pIndex, ref List<string> pColumns, ref List<List<object>> pRows)
        {
            pColumns = null;
            pRows = null;
            if (pDataSet.Count() > pIndex)
            {
                JsonElement.ArrayEnumerator jsTable = pDataSet.ElementAt(pIndex).EnumerateArray();
                if (jsTable.Count() > 1)
                {
                    pColumns = CommonMethods.ConvertToListString(jsTable.ElementAt(0));
                    pRows = CommonMethods.ConvertToList(jsTable.ElementAt(1));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">{"CategoryId": 1, "ProductTypeId": 1, "Status": 1}</param>
        /// <returns></returns>
        public static Dictionary<string, long> ConvertToDictionaryInt64(JsonElement pJson)
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, long>>(pJson.ToString());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Json Widgets {ProductId: No}
        /// </summary>
        /// <param name="pJson">{"1": 20, "2": 21, "4": 22}</param>
        /// <returns></returns>
        public static Dictionary<string, int> ConvertToDictionaryInt32(string pJson)
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, int>>(pJson);
            }
            catch
            {
                return null;
            }
        }

        public static Dictionary<string, string> ConvertToDictionaryString(JsonElement pJson)
        {
            try
            {
                return pJson.EnumerateObject().ToDictionary(p => p.Name, p => p.Value.ToString());
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">{"id": 1, "name": "Mazda 3 Premium"}</param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertToDictionaryString(string pJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(pJson))
                {
                    JsonElement json = ConvertToJsonElement(pJson);
                    return ConvertToDictionaryString(json);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">{"id": 1, "name": "Mazda 3 Premium"}</param>
        /// <returns></returns>
        public static Dictionary<string, dynamic> ConvertToDictionaryDynamic(string pJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(pJson))
                {
                    return JsonSerializer.Deserialize<Dictionary<string, dynamic>>(pJson);

                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static List<object> ConvertToListObject(JsonElement pJson)
        {
            try
            {
                return pJson.EnumerateArray().Select(x => (object)x.ToString()).ToList();
            }
            catch
            {
                return null;
            }
        }

        public static List<long> ConvertToListInt64(JsonElement pJson)
        {
            try
            {
                // return pJson.EnumerateArray().Select(x => x.GetInt64()).ToList();
                return pJson.EnumerateArray().Select(x => x.ValueKind == JsonValueKind.Number ? x.GetInt64() : ConvertToInt64(x.GetString())).ToList();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">[1,2,3,4,5]</param>
        /// <returns></returns>
        public static List<long> ConvertToListInt64(string pJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(pJson))
                {
                    JsonElement json = ConvertToJsonElement(pJson);
                    return ConvertToListInt64(json);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static List<string> ConvertToListString(JsonElement pJson)
        {
            try
            {
                return pJson.EnumerateArray().Select(x => x.ToString()).ToList();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">[1, "Mazda 3 Premium", 3]</param>
        /// <returns></returns>
        public static List<string> ConvertToListString(string pJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(pJson))
                {
                    JsonElement json = ConvertToJsonElement(pJson);
                    return ConvertToListString(json);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static List<JsonElement> ConvertToListJsonElement(JsonElement pJson)
        {
            try
            {
                return pJson.EnumerateArray().ToList();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">[[1, "Mazda 3 Deluxe"][1, "Mazda 3 Premium"]] OR [{"id": 1, "name": "Mazda 3 Deluxe"},{"id": 2, "name": "Mazda 3 Premium"}]</param>
        /// <returns></returns>
        public static List<JsonElement> ConvertToListJsonElement(string pJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(pJson))
                {
                    JsonElement json = ConvertToJsonElement(pJson);
                    return ConvertToListJsonElement(json);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">[{"id": 1, "name": "Mazda 3 Deluxe"},{"id": 2, "name": "Mazda 3 Premium"}]</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> ConvertToListDictionary(string pJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(pJson))
                {
                    JsonElement json = ConvertToJsonElement(pJson);
                    return ConvertToListDictionary(json);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">[{"id": 1, "name": "Mazda 3 Deluxe"},{"id": 2, "name": "Mazda 3 Premium"}]</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> ConvertToListDictionary(JsonElement pJson)
        {
            try
            {
                List<Dictionary<string, string>> lstResults = new List<Dictionary<string, string>>();
                List<JsonElement> lstJson = ConvertToListJsonElement(pJson);
                if (lstJson != null)
                {
                    foreach (JsonElement item in lstJson)
                    {
                        lstResults.Add(ConvertToDictionaryString(item));
                    }
                }
                return lstResults;
            }
            catch
            {
                return null;
            }
        }

        public static List<Dictionary<string, object>> ConvertToListDictionaryObject(string pJson)
        {
            try
            {
                return JsonSerializer.Deserialize<List<Dictionary<string, object>>>(pJson);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">[[1, 'vi', 'Tiếng Việt'], [2, 'en', 'Tiếng Anh']]</param>
        /// <returns></returns>
        public static List<List<object>> ConvertToList(string pJson)
        {
            try
            {
                List<List<object>> lstResults = new List<List<object>>();
                List<JsonElement> lstJson = ConvertToListJsonElement(pJson);
                foreach (JsonElement item in lstJson)
                {
                    lstResults.Add(ConvertToListObject(item));
                }
                return lstResults;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">[[1, 'vi', 'Tiếng Việt'], [2, 'en', 'Tiếng Anh']]</param>
        /// <returns></returns>
        public static List<List<object>> ConvertToList(JsonElement pJson)
        {
            try
            {
                List<List<object>> lstResults = new List<List<object>>();
                List<JsonElement> lstJson = ConvertToListJsonElement(pJson);
                foreach (JsonElement item in lstJson)
                {
                    lstResults.Add(ConvertToListObject(item));
                }
                return lstResults;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pJson">{"vi": {overview: "Tổng quan", exterior: "Ngoại thất"}, "en":{overview: "Overview", exterior: "Exterior"}}</param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> ConvertToDictionary(string pJson)
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> lstResults = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> lstTemp = ConvertToDictionaryString(pJson);
                foreach (string key in lstTemp.Keys)
                {
                    lstResults.Add(key, ConvertToDictionaryString(lstTemp[key]));
                }
                return lstResults;
            }
            catch
            {
                return null;
            }
        }

      

        public static int GetInt32ByKey(List<string> pDatas, List<string> pFields, string pFieldName)
        {
            int index = pFields.FindIndex(x => x == pFieldName);
            if (index > -1 && index < pDatas.Count)
            {
                return ConvertToInt32(pDatas[index]);
            }
            return -1;
        }

        public static long GetInt64ByKey(List<string> pDatas, List<string> pFields, string pFieldName)
        {
            int index = pFields.FindIndex(x => x == pFieldName);
            if (index > -1 && index < pDatas.Count)
            {
                return ConvertToInt64(pDatas[index]);
            }
            return -1;
        }

        public static decimal GetDecimalByKey(List<string> pDatas, List<string> pFields, string pFieldName)
        {
            int index = pFields.FindIndex(x => x == pFieldName);
            if (index > -1 && index < pDatas.Count)
            {
                return ConvertToDecimal(pDatas[index]);
            }
            return 0;
        }

        public static Boolean GetBooleanByKey(List<string> pDatas, List<string> pFields, string pFieldName)
        {
            int index = pFields.FindIndex(x => x == pFieldName);
            if (index > -1 && index < pDatas.Count)
            {
                return ConvertToBoolean(pDatas[index]);
            }
            return false;
        }

        public static string GetStringByKey(List<string> pDatas, List<string> pFields, string pFieldName)
        {
            int index = pFields.FindIndex(x => x == pFieldName);
            if (index > -1 && index < pDatas.Count)
            {
                return pDatas[index];
            }
            return string.Empty;
        }

        public static string GetStringByKey(string pJson, string pKey)
        {
            if (!string.IsNullOrEmpty(pJson))
            {
                JsonElement json = ConvertToJsonElement(pJson);
                return GetPropertyString(json, pKey);
            }
            return string.Empty;
        }

        public static object GetObjectByKey(List<object> pRows, List<string> pColumns, string pColumnName)
        {
            int index = pColumns.IndexOf(pColumnName);
            if (index > -1 && index < pRows.Count)
            {
                return pRows[index];
            }
            return null;
        }

        public static void SetStringByKey(List<string> pDatas, List<string> pFields, string pFieldName, string pValue)
        {
            int index = pFields.FindIndex(x => x == pFieldName);
            if (index > -1 && index < pDatas.Count)
            {
                pDatas[index] = pValue;
            }
        }

        #endregion

        #region "Compress"

        public static byte[] CompressContent(string pData)
        {
            byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(pData);
            if (data.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                    {
                        gzip.Write(data, 0, data.Length);
                        gzip.Flush();
                        gzip.Close();
                    }
                    return stream.ToArray();
                }
            }
            return data;
        }

        #endregion

        #region "Email"

        public static bool IsEmail(string pEmail)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(pEmail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Loc lai Email
        /// </summary>
        /// <param name="pEmailList">Moi Email cach nhau bang dau ;</param>
        /// <returns></returns>
        public static string FilterEmails(string pEmailList)
        {
            if (string.IsNullOrEmpty(pEmailList))
            {
                return string.Empty;
            }
            List<string> lstEmail = new List<string>();
            string[] arrEmail = pEmailList.Split(";");
            foreach (string email in arrEmail)
            {
                if (IsEmail(email))
                {
                    lstEmail.Add(email);
                }
            }
            return string.Join(";", lstEmail);
        }

        #endregion

        #region "Replace Email Marketing"

        private static string GetDataByFormat(object pData, string pFormat)
        {
            switch (pFormat)
            {
                case "#":
                    return CommonMethods.FormatNumber(CommonMethods.ConvertToDecimal(pData));
                case "##": // Number SEO
                    return CommonMethods.FormatNumberSEO(CommonMethods.ConvertToDecimal(pData));
                case "@":
                    return CommonMethods.FormatDateTime(CommonMethods.ConvertToDateTime(pData), CommonConstants.DD_MM_YYYY);
                case "@@":
                    return CommonMethods.FormatDateTime(CommonMethods.ConvertToDateTime(pData), CommonConstants.DD_MM_YYYY_HH_MM);
                case "@@@": // Datetime SEO
                    return CommonMethods.FormatDateTime(CommonMethods.ConvertToDateTime(pData), CommonConstants.DD_MM_YYYY_SEO);
                case "@@@@": // DAY
                    return CommonMethods.FormatDateTime(CommonMethods.ConvertToDateTime(pData), "dd");
                case "@@@@@": // MONTH
                    return CommonMethods.FormatDateTime(CommonMethods.ConvertToDateTime(pData), "MM");
                case "@@@@@@": // YEAR
                    return CommonMethods.FormatDateTime(CommonMethods.ConvertToDateTime(pData), "yyyy");
                default:
                    break;
            }
            return CommonMethods.ConvertToString(pData);
        }

        private static string ReplaceStringContent(string pContent, Dictionary<string, string> pData)
        {
            return System.Text.RegularExpressions.Regex.Replace(pContent, @"\$(?<key>([0-9a-z]+))(?<format>[#@]*)", m =>
            {
                string key = m.Groups["key"].Value;
                string format = m.Groups["format"].Value;
                if (pData.ContainsKey(key))
                {
                    return GetDataByFormat(pData[key], format);
                }
                return string.Empty;
            }, System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        public static string ReplaceEmailContent(string pContent, Dictionary<string, string> pData)
        {
            pContent = System.Text.RegularExpressions.Regex.Replace(pContent, @"<gh:for[\s]*resource[\s]*=[\s]*[""']?(?<value>.*?)[""'][\s]*>(?<content>.*?)</gh:for>", m =>
            {
                string resource = m.Groups["value"].Value.Replace("$", string.Empty);
                if (pData.ContainsKey(resource))
                {
                    StringBuilder sb = new StringBuilder();
                    List<Dictionary<string, string>> lst = CommonMethods.ConvertToListDictionary(pData[resource]);
                    for (int i = 0; i < lst.Count; i++)
                    {
                        string content = m.Groups["content"].Value;
                        sb.AppendLine(ReplaceStringContent(content, lst[i]));
                    }
                    return sb.ToString();
                }
                return string.Empty;
            }, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            pContent = ReplaceStringContent(pContent, pData);
            return pContent;
        }

        public static void FilterReceiveEmail(ref string pReceiveName, ref string pReceiveEmail, string pStoreName, string pStoreEmail)
        {
            if (string.IsNullOrEmpty(pReceiveName))
            {
                pReceiveName = string.IsNullOrEmpty(pStoreName) ? "Qúy khách" : pStoreName;
            }

            if (string.IsNullOrEmpty(pReceiveEmail))
            {
                pReceiveEmail = pStoreEmail;
            }
            else if (!pReceiveEmail.Contains(pStoreEmail))
            {
                pReceiveEmail += ";" + pStoreEmail;
            }
            pReceiveEmail = FilterEmails(pReceiveEmail);
        }

        #endregion

        #region "Format HTML Content"

        public static string FormatHtmlContent(string pContent)
        {
            pContent = System.Text.RegularExpressions.Regex.Replace(pContent, "<([^>]*)>", delegate (System.Text.RegularExpressions.Match m)
            {
                string res = string.Empty;
                string str = m.Groups[1].Value.ToLower();
                System.Text.RegularExpressions.Match mTemp;

                // Tag <img>
                if (str.StartsWith("img"))
                {
                    mTemp = System.Text.RegularExpressions.Regex.Match(m.Groups[1].Value, " src=\"([^\"]*)\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                    if (mTemp.Success)
                    {
                        return "<p style=\"text-align:center;\"><img src=\"" + mTemp.Groups[1].Value + "\"/></p>";
                    }
                    return "<p style=\"text-align:center;\"><" + m.Groups[1].Value + "/></p>";
                }
                // Tag <iframe>
                if (str.StartsWith("iframe"))
                {
                    return "<div class=\"embed-responsive;\"><" + m.Groups[1].Value + "></iframe></div>";
                }
                // Tag <strong>, <b>
                if (str.Equals("strong") || str.Equals("b"))
                {
                    return "<strong>";
                }
                if (str.Equals("/strong") || str.Equals("/b"))
                {
                    return "</strong>";
                }
                // Tag <br/>
                if (str.Equals("br") || str.StartsWith("br/") || str.StartsWith("br /"))
                {
                    return "<br>";
                }
                // Tag <table>
                if (str.StartsWith("table"))
                {
                    return "<table class=\"table table-bordered\">";
                }
                if (str.Equals("/table"))
                {
                    return "</table>";
                }
                if (str.StartsWith("tr"))
                {
                    return "<tr>";
                }
                if (str.Equals("/tr"))
                {
                    return "</tr>";
                }
                if (str.StartsWith("td") || (str.StartsWith("th") && !str.StartsWith("thead")))
                {
                    string resTD = string.Empty;
                    mTemp = System.Text.RegularExpressions.Regex.Match(m.Groups[1].Value, " colspan=\"([^\"]*)\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                    if (mTemp.Success)
                    {
                        resTD += " colspan=\"" + mTemp.Groups[1].Value + "\"";
                    }
                    mTemp = System.Text.RegularExpressions.Regex.Match(m.Groups[1].Value, " rowspan=\"([^\"]*)\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                    if (mTemp.Success)
                    {
                        resTD += " rowspan=\"" + mTemp.Groups[1].Value + "\"";
                    }
                    return $"<{(str.StartsWith("td") ? "td" : "th")}{resTD}>";
                }
                if (str.Equals("/td"))
                {
                    return "</td>";
                }
                if (str.Equals("/th"))
                {
                    return "</th>";
                }
                // Tag <ul>
                if (str.StartsWith("ul"))
                {
                    return "<ul>";
                }
                if (str.Equals("/ul"))
                {
                    return "</ul>";
                }
                // Tag <ol>
                if (str.StartsWith("ol"))
                {
                    return "<ol>";
                }
                if (str.Equals("/ol"))
                {
                    return "</ol>";
                }
                // Tag <li>
                if (str.StartsWith("li"))
                {
                    return "<li>";
                }
                if (str.Equals("/li"))
                {
                    return "</li>";
                }
                if (str.Equals("/div") || str.Equals("/p") || str.Equals("/h1") || str.Equals("/h2") || str.Equals("/h3") || str.Equals("/h4") || str.Equals("/h5") || str.Equals("/h6"))
                {
                    return "<br>";
                }
                return res;
            }, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);

            pContent = pContent.Replace("\t", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty);
            pContent = System.Text.RegularExpressions.Regex.Replace(pContent, "( ){3,20}", string.Empty);
            pContent = System.Text.RegularExpressions.Regex.Replace(pContent, "( ){2}", " ");
            pContent = System.Text.RegularExpressions.Regex.Replace(pContent, "(<br>){2,100}", "<br><br>");

            string[] arrayLines = pContent.Split(new string[] { "<br>" }, StringSplitOptions.None);
            string text;
            StringBuilder sbContent = new StringBuilder();
            foreach (string line in arrayLines)
            {
                text = line.Trim();
                if (!string.IsNullOrEmpty(text) && text != "&nbsp;")
                {
                    if (!text.StartsWith("<div") && !text.StartsWith("<p") && !text.StartsWith("<ul") && !text.StartsWith("<ol") && !text.StartsWith("<li") &&
                        !text.StartsWith("<table") && !text.StartsWith("<tr") && !text.StartsWith("<td") && !text.StartsWith("<th"))
                    {
                        sbContent.Append("<p>" + text + "</p>");
                    }
                    else
                    {
                        sbContent.Append(text);
                    }
                }
            }
            return sbContent.ToString().Replace("<p>&nbsp;</p>", string.Empty);
        }

        public static string FormatHtmlContentInTable(string pContent)
        {
            pContent = System.Text.RegularExpressions.Regex.Replace(pContent, "<table([^>]*)>(.*?)</table>", delegate (System.Text.RegularExpressions.Match m)
            {
                string res = FormatHtmlContent(m.Groups[2].Value);
                return "<table class=\"table table-bordered\">" + res + "</table>";
            }, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            pContent = System.Text.RegularExpressions.Regex.Replace(pContent, "<ul([^>]*)>(.*?)</ul>", delegate (System.Text.RegularExpressions.Match m)
            {
                string res = FormatHtmlContent(m.Groups[2].Value);
                return "<ul>" + res + "</ul>";
            }, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            pContent = System.Text.RegularExpressions.Regex.Replace(pContent, "<ol([^>]*)>(.*?)</ol>", delegate (System.Text.RegularExpressions.Match m)
            {
                string res = FormatHtmlContent(m.Groups[2].Value);
                return "<ol>" + res + "</ol>";
            }, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            return pContent;
        }

        public static string FormatNoFollowContent(string pContent)
        {
            return System.Text.RegularExpressions.Regex.Replace(pContent, "<a([^>]*)>", delegate (System.Text.RegularExpressions.Match m)
            {
                if (m.Groups[1].Value.ToLower().Contains("rel='nofollow'") || m.Groups[1].Value.ToLower().Contains("rel=\"nofollow\""))
                    return m.Value;
                return "<a rel=\"nofollow\"" + m.Groups[1].Value + ">";
            }, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
        }

        #endregion

        #region "Images"

        public static int GetQualityImage(int pWidth, int pHeight)
        {
            int size = Math.Max(pWidth, pHeight);
            if (pWidth < 400 || pHeight < 400)
            {
                size = Math.Min(pWidth, pHeight);
            }

            //if (size > 1000)
            //{
            //    return 75;
            //}

            if (size > 600)
            {
                return 90;
            }

            return 100;
        }

        #endregion

        public static void WriteLog(string pMessage, string pPathFolder = "")
        {
            try
            {
                string pathFile = string.IsNullOrEmpty(pPathFolder) ? "C:/Logs/DailyXe" : pPathFolder;
                if (!System.IO.Directory.Exists(pathFile))
                {
                    System.IO.Directory.CreateDirectory(pathFile);
                }
                pathFile = System.IO.Path.Combine(pathFile, $"{System.DateTime.Now.ToString("dd-MM-yyyy")}.txt");
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(pathFile, true))
                {
                    streamWriter.WriteLine("------------------------------------------------------------------------------------");
                    streamWriter.WriteLine("Time log error: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                    streamWriter.WriteLine("------------------------------------------------------------------------------------");
                    streamWriter.WriteLine(pMessage);
                    streamWriter.WriteLine("------------------------------------------------------------------------------------");
                    streamWriter.Close();
                }
            }
            catch { }
        }

        public static async Task WriteLogAsync(string pMessage, string pPathFolder = "")
        {
            try
            {
                string pathFile = string.IsNullOrEmpty(pPathFolder) ? "C:/Logs/DailyXe" : pPathFolder;
                if (!System.IO.Directory.Exists(pathFile))
                {
                    System.IO.Directory.CreateDirectory(pathFile);
                }
                pathFile = System.IO.Path.Combine(pathFile, $"{System.DateTime.Now.ToString("dd-MM-yyyy")}.txt");
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(pathFile, true))
                {
                    await streamWriter.WriteLineAsync("------------------------------------------------------------------------------------");
                    await streamWriter.WriteLineAsync("Time log error: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                    await streamWriter.WriteLineAsync("------------------------------------------------------------------------------------");
                    await streamWriter.WriteLineAsync(pMessage);
                    await streamWriter.WriteLineAsync("------------------------------------------------------------------------------------");
                    streamWriter.Close();
                }
            }
            catch { }
        }

        public static string ConvertYouTubeDuration(object obDuration)
        {
            try
            {
                string duration = CommonMethods.ConvertToString(obDuration);
                var list = duration.Split(':').ToList();
                var n = list.Count;
                if (n > 0)
                {
                    while (CommonMethods.ConvertToInt64(list[0]) <= 0 && n > 0)
                    {
                        list.RemoveAt(0);
                        n--;
                    }
                }

                var nFirst = list[0];
                list[0] = CommonMethods.ConvertToInt64(nFirst).ToString();
                if (list.Count == 1)
                {
                    return string.Format("0:{0}", list[0]);
                }
                return string.Join(":", list.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static T GetEntity<T>(DataRow row) where T : new()
        {
            var entity = new T();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                //Get the description attribute
                var descriptionAttribute = (DescriptionAttribute)property.GetCustomAttributes(typeof(DescriptionAttribute), true).SingleOrDefault();
                if (descriptionAttribute == null)
                    continue;

                property.SetValue(entity, row[descriptionAttribute.Description]);
            }

            return entity;
        }

        public static List<T> ConvertToEntity<T>(DataSet pDataSet) where T : new()
        {
            List<T> listData = new List<T>();
            foreach (DataRow row in pDataSet.Tables[0].Rows)
            {
                listData.Add(GetEntity<T>(row));
            }
            return listData;
        }
       
    }
}