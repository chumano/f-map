using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Net;
using System.IO;

public partial class Server : System.Web.UI.Page
{
    private String sampleGetRequest()
    {
        HttpWebRequest request = null;

        string url = "http://localhost:8080/geoserver/wms?bbox=-130,24,-66,50&styles=population&Format=image/png&request=GetMap&layers=topp:states&width=550&height=250&srs=EPSG:4326";

        // Create web request

        request = (HttpWebRequest)WebRequest.Create(url);

        // Set value for request headers

        request.Method = "GET";

        request.ProtocolVersion = HttpVersion.Version11;

        request.AllowAutoRedirect = false;

        request.Accept = "*/*";

        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";

        request.Headers.Add("Accept-Language", "en-us");

        request.KeepAlive = true;

        StreamReader responseStream = null;

        HttpWebResponse webResponse = null;

        string webResponseStream = string.Empty;

        // Get response for http web request

        webResponse = (HttpWebResponse)request.GetResponse();

        responseStream = new StreamReader(webResponse.GetResponseStream());

        // Read web response into string

        webResponseStream = responseStream.ReadToEnd();

        return webResponseStream;
    }

    private String samplePostRequest()
    {
        String url = "http://localhost:8080/geoserver/wfs";
        String postData = "<wfs:GetFeature service=\"WFS\" version=\"1.1.0\"  xmlns:topp=\"http://www.openplans.org/topp\"  xmlns:wfs=\"http://www.opengis.net/wfs\"  xmlns:ogc=\"http://www.opengis.net/ogc\"  xmlns:gml=\"http://www.opengis.net/gml\"  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"  xsi:schemaLocation=\"http://www.opengis.net/wfs http://schemas.opengis.net/wfs/1.1.0/wfs.xsd\">  <wfs:Query typeName=\"topp:states\">    <wfs:PropertyName>topp:STATE_NAME</wfs:PropertyName>    <wfs:PropertyName>topp:PERSONS</wfs:PropertyName>    <ogc:Filter>        <ogc:PropertyIsLike escape=\"\\\" singleChar=\"_\" wildCard=\"%\">		    <ogc:PropertyName>topp:STATE_NAME</ogc:PropertyName>    	    <ogc:Literal>%New%</ogc:Literal>	   </ogc:PropertyIsLike>   </ogc:Filter>  </wfs:Query></wfs:GetFeature>";

        HttpWebRequest request = null;
        Uri uri = new Uri(url);
        request = (HttpWebRequest)WebRequest.Create(uri);
        request.Method = "POST";
        request.ContentType = "text/xml";
        request.ContentLength = postData.Length;
        using (Stream writeStream = request.GetRequestStream())
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] bytes = encoding.GetBytes(postData);
            writeStream.Write(bytes, 0, bytes.Length);
        }
        string result = string.Empty;
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
                {
                    result = readStream.ReadToEnd();
                }
            }
        }
        return result;
    }

    MSSQLConnection conn = new MSSQLConnection("BanDoSG", "sa", "371319855");
    protected void Page_Load(object sender, EventArgs e)
    {
        // Put user code to initialize the page here
        if (Request["action"] != null && Request["action"].Trim() != "")
        {
            switch (Request["action"])
            {
                case "TenDuong":
                    //processGetTenDuong();

                    

                    Response.Write(samplePostRequest());

                    break;
                
                default:
                    Response.Write("chumano -kaka");
                    break;
            }
        }

    }

    protected void processGetTenDuong()
    {
        if (Request.QueryString.Count == 0)
            return;

        string ten = Request.QueryString["u"].ToString();

        string sqlStr = "Select TenConDuong from ConDuong Where TenConDuong like " +"'" + ten + "%'";
        DataTable tbl = conn.Select(sqlStr);

        if (tbl.Rows.Count > 0)
        {
            string jSon = JSON_DataTable(tbl);
            Response.Write(jSon);
        }
        else
            Response.Write("hello");
    }

    public string JSON_DataTable(DataTable dt)
    {

        StringBuilder JsonString = new StringBuilder();

        JsonString.Append("{ ");
        JsonString.Append("\"TABLE\":[{ ");
        JsonString.Append("\"ROW\":[ ");

        for (int i = 0; i < dt.Rows.Count; i++)
        {

            JsonString.Append("{ ");
            JsonString.Append("\"COL\":[ ");

            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (j < dt.Columns.Count - 1)
                {
                    JsonString.Append("{" + "\"DATA\":\"" +
                                      dt.Rows[i][j].ToString() + "\"},");
                }
                else if (j == dt.Columns.Count - 1)
                {
                    JsonString.Append("{" + "\"DATA\":\"" +
                                      dt.Rows[i][j].ToString() + "\"}");
                }
            }
            /*end Of String*/
            if (i == dt.Rows.Count - 1)
            {
                JsonString.Append("]} ");
            }
            else
            {
                JsonString.Append("]}, ");
            }
        }
        JsonString.Append("]}]}");
        return JsonString.ToString();
    }
}