using Microsoft.AspNetCore.Routing;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace VoxelMapsTestTask.Utils
{
    public static class ExtensionMethodsUtil
    {
        public static bool IsSignRequest(this RouteData data)
        {
            return data.Values["action"].ToString().Contains("Sign");
        }
        public static string GetResource(this RouteData data)
        {
            return "[" + data.Values["action"].ToString().ToUpper() +"]:" + data.Values["controller"];
        }
        public static string FormatCNPJ(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }
        public static string GetDescription(this Enum enumElement)
        {
            return enumElement.GetType()
                              .GetMember(enumElement.ToString())
                              .First()
                              .GetCustomAttribute<DisplayAttribute>()
                              .GetDescription();
        }
        public static string GetShorName(this Enum enumElement)
        {
            return enumElement.GetType()
                              .GetMember(enumElement.ToString())
                              .First()
                              .GetCustomAttribute<DisplayAttribute>()
                              .GetShortName();
        }
        public static string GetInitials(this string name)
        {
            string[] nameSplit = name.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string initials = string.Empty;

            initials += nameSplit[0].Substring(0, 1).ToUpper();

            if (nameSplit.Length > 1)
                initials += nameSplit[^1].Substring(0, 1).ToUpper();

            return initials;
        }
        public static string FormatMoney(this decimal vl)
        {
            if (vl == 0)
                return "--/--";

            string value = vl.ToString();
            value = value.Replace(",", ".");

            int dotIndex = value.IndexOf(".");

            if (dotIndex > 0)
            {
                value = value.Replace(".", ",");

                if (value.Substring(0, dotIndex).Length < 4)
                {
                    value = "R$" + value;
                }
                else
                {
                    value = "R$" + value.Insert(value.Substring(0, dotIndex).Length - 3, ".");
                }
            }
            else
            {
                if (value.Length < 4)
                {
                    value = "R$" + value + ",00";
                }
                else
                {
                    value = "R$" + value.Insert(value.Length - 3, ".") + ",00";
                }
            }

            return value;
        }
        public static string FormatDate(this DateTime date)
        {
            var dd = date.Day < 10 ? "0" + date.Day : date.Day.ToString();
            var MM = date.Month < 10 ? "0" + date.Month : date.Month.ToString();
            var yyyy = date.Year;

            var HH = date.AddHours(-3).Hour < 10 ? "0" + date.AddHours(-3).Hour : date.AddHours(-3).Hour.ToString();
            var mm = date.Minute < 10 ? "0" + date.Minute : date.Minute.ToString();

            var dateFormated = $"{dd}/{MM}/{yyyy} às {HH}:{mm}";

            return dateFormated;
        }
        public static DateTime StringToDatetime(string dateStr)
        {
            var date = DateTime.ParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return date;
        }
        public static string GetFileExtension(this string base64String)
        {
            var data = base64String.Substring(0, 5);

            return data.ToUpper() switch
            {
                "MQOWM" or "77U/M" => ".srt",
                "IVBOR" => ".png",
                "/9J/4" => ".jpg",
                "AAAAF" => ".mp4",
                "JVBER" => ".pdf",
                "AAABA" => ".ico",
                "UMFYI" => ".rar",
                "E1XYD" => ".rtf",
                "U1PKC" => ".txt",
                _ => string.Empty,
            };
        }
        public static int GetUserId(this ClaimsPrincipal User)
        {
            var userSid = User.Claims.Where(x => x.Type == ClaimTypes.Sid).FirstOrDefault().Value;
            var userId = int.Parse(userSid);

            return userId;
        }
    }
}
