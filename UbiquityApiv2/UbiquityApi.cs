using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using dab.Library.Net;

namespace dab.Library.Api.UbiquityV2
{
    public class UbiquityApi
    {
        public int Timeout { get; set; }
        public string Server { get; set; }
        public string UserAuth { get; set; }

        private string UserAgent = "dabberz Ubiquity API 2.0 Client C#/1.0";

        public UbiquityApi(int resellerId, string remoteId, string remoteKey)
        {
            this.UserAuth = resellerId + ":" + remoteId + ":" + remoteKey;
            this.Server = "http://api.ubiquityservers.com/v2/api.php";
            this.Timeout = 30;

            wc.Timeout = this.Timeout * 1000;
            wc.Headers["UserAgent"] = this.UserAgent;
            wc.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(this.UserAuth));

        }

        public dynamic Call(string method, NameValueCollection arguments)
        {
            string url = this.Server;

            var parameters = string.Join("&", (from key in arguments.AllKeys 
                            from value in arguments.GetValues(key) 
                                select string.Format("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value))
                        ).ToArray());


            var result = wc.DownloadString(url + "?method=" + method + "&" + parameters);
            Console.WriteLine("Response: {0}", wc.ResponseHeaders["content-type"]);

            return JsonConvert.DeserializeObject(result);
        }

        WebDownload wc = new WebDownload();
    }
}
