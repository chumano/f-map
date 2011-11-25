using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

/// <summary>
/// Summary description for Utils
/// </summary>
public class Utils
{
	public Utils()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static string GetRequest(string url)
    {
        // Create web request
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        // Set value for request headers
        request.Method = "GET";
        request.ProtocolVersion = HttpVersion.Version11;
        request.AllowAutoRedirect = false;
        request.Accept = "*/*";
        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
        request.Headers.Add("Accept-Language", "en-us");
        request.KeepAlive = true;

        // Get response for http web request
        HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
        StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());

        // Return response string
        return responseStream.ReadToEnd();
    }
}