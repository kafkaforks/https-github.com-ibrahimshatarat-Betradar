using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Internal;
using Npgsql;
using RestSharp;
using RestSharp.Extensions;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;

namespace Betradar.Classes
{
    public class MainAttribute : Attribute
    {

    }
    public static class AttributesHelperExtension
    {
        public static string ToDescription(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }

   
    public static class Helpers
    {
    }
}
