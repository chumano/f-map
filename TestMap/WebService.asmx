<%@ WebService Language="C#" Class="Samples.AspNet.WebService" %>
 
using System;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using System.Net;
using System.IO;
 
namespace Samples.AspNet
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        private string _xmlString =
            @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                <message>
                    <content>
                        Welcome to the asynchronous communication layer world!
                    </content>
                </message>";

        // This method returns an XmlDocument type.
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlDocument GetXmlDocument()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(_xmlString);
            return xmlDoc;
        }
      
        // This method uses GET instead of POST.
        // For this reason its input parameters
        // are sent by the client in the
        // URL query string.
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string EchoStringAndDate(DateTime dt, string s)
        {
            return samplePostRequest();
            // return s + ":" + dt.ToString();
        }

        private String samplePostRequest()
        {
            String url = "http://localhost:8080/geoserver/wfs?outputFormat=json";
            String postData = "<wfs:GetFeature service=\"WFS\" version=\"1.1.0\"  xmlns:topp=\"http://www.openplans.org/topp\"  xmlns:wfs=\"http://www.opengis.net/wfs\"  xmlns:ogc=\"http://www.opengis.net/ogc\"  xmlns:gml=\"http://www.opengis.net/gml\"  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"  xsi:schemaLocation=\"http://www.opengis.net/wfs http://schemas.opengis.net/wfs/1.1.0/wfs.xsd\">  <wfs:Query typeName=\"thesis:SDE.SDE.QUAN1_GT_NEN1\">    <wfs:PropertyName>Shape</wfs:PropertyName>    <ogc:Filter>        <ogc:PropertyIsLike escape=\"\\\" singleChar=\"_\" wildCard=\"%\">		    <ogc:PropertyName>TEN</ogc:PropertyName>    	    <ogc:Literal>%nh</ogc:Literal>	   </ogc:PropertyIsLike>   </ogc:Filter>  </wfs:Query></wfs:GetFeature>";

            HttpWebRequest request = null;
            Uri uri = new Uri(url);
            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = postData.Length;
            using (Stream writeStream = request.GetRequestStream())
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] bytes = encoding.GetBytes(postData);
                writeStream.Write(bytes, 0, bytes.Length);
            }
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream, System.Text.Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
            }
            return result;
        }
        
        [WebMethod]
        public string GetServerTime()
        {

            string serverTime =
                String.Format("The current time is {0}.", DateTime.Now);

            return serverTime;
           
        }
      
        [WebMethod]
        public string Add(int a, int b)
        {

            int addition = a + b;
            string result = 
                String.Format("The addition result is {0}.", 
                    addition.ToString());

            return result;

        }
     
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml, 
            XmlSerializeString = true)]
        public string GetString()
        {
            return "Hello World";           
        }
      
    }

}
