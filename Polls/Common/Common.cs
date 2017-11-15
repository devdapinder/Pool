using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Polls.Common
{
    public class CommonUtility
    {
        public static string ApirUrl = string.Empty;
        public static string FullImageBaseUrl { get; set; }
        public static string ThumbnailBaseUrl { get; set; }

        public static T DoSerialize<T>(string jsonstring)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            T serializeObject;
            serializeObject = ser.Deserialize<T>(jsonstring);
            return serializeObject;
        }


    }
    public static class LoggedInUserDetails
    {
        public static string Username { get; set; }
        public static bool IsEmailVerified { get; set; }
    }
}