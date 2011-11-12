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
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using SmartLib;

public partial class Server : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["action"] != null && Request["action"].Trim() != "")
        {
            switch (Request["action"])
            {
                case "GetInfo":
                    // http://localhost:8080/geoserver/wms?REQUEST=GetFeatureInfo&BBOX={0}&SERVICE=WMS&VERSION=1.1.1&X={1}&Y={2}
                    // &INFO_FORMAT=text/plain&QUERY_LAYERS={3}&FEATURE_COUNT=50&Layers={4}&WIDTH={5}&HEIGHT={6}&srs=EPSG:4326
                    string url = String.Format(Config.URL_GET_INFO,
                                                Request["bbox"],
                                                Request["x"],
                                                Request["y"],
                                                Request["layer_name"],
                                                Request["layer_name"],
                                                Request["width"],
                                                Request["height"]);
                    GetInfo(url);
                    break;

                case "GetMap":
                    GetMap(Request["map_id"]);
                    break;

                case "GetWards":
                    GetWards(Request["district_id"]);
                    break;
            }
        }
    }

    private string DoGetRequest(string url)
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

    #region GetInfo API

    private void GetInfo(string url)
    {
        string response = DoGetRequest(url);
        string[] array = response.Split(new char[] { '\r', '\n' });

        FeatureInfoItemModel[] featureInfoItems = new FeatureInfoItemModel[3];

        featureInfoItems[0] = new FeatureInfoItemModel();
        featureInfoItems[0].Name = "Mã hành chính";
        featureInfoItems[0].Value = ParseFeatureInfoItem(array[4]);

        featureInfoItems[1] = new FeatureInfoItemModel();
        featureInfoItems[1].Name = "Tên";
        featureInfoItems[1].Value = ParseFeatureInfoItem(array[6]);

        featureInfoItems[2] = new FeatureInfoItemModel();
        featureInfoItems[2].Name = "Số hộ";
        featureInfoItems[2].Value = ParseFeatureInfoItem(array[12]);
        
        Response.Write(JsonConvert.SerializeObject(featureInfoItems));
    }

    private string ParseFeatureInfoItem(string item)
    {
        return item.Substring(item.IndexOf('=') + 2);
    }

    #endregion

    #region GetMap API

    private void GetMap(string mapId)
    {
        // TODO test 
        // mapId = "1";

        // Get map bound
        DataTable dt = Helper.GetDataTable("select * from MapView where ID = " + mapId);

        DataRow row = dt.Rows[0];
        BoundModel bound = new BoundModel();
        bound.MinX = (double)row["MinX"];
        bound.MaxX = (double)row["MaxX"];
        bound.MinY = (double)row["MinY"];
        bound.MaxY = (double)row["MaxY"];

        // Get layers for map
        // dt = Helper.GetDataTable("select * from Layer, LayerMap where " + mapId + " = LayerMap.MapID and LayerMap.LayerID = Layer.ID");
        dt = Helper.GetDataTable("select * from vw_map where MapID = " + mapId);
        LayerModel[] layers = new LayerModel[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; ++i)
        {
            row = dt.Rows[i];

            layers[i] = new LayerModel();
            layers[i].LayerName = (string)row["LayerName"];
            layers[i].Layer = (string)row["Layer"];
            layers[i].StyleName = (string)row["StyleName"];
        }

        // Write the response
        MapInfoModel mapInfo = new MapInfoModel();
        mapInfo.layers = layers;
        mapInfo.bound = bound;
        string response = JsonConvert.SerializeObject(mapInfo);
        Response.Write(response);
    }

    #endregion

    #region GetWards API

    private void GetWards(string districtId)
    {
        DataTable dt = Helper.GetDataTable2("select * from QUAN" + districtId + "_RG_HCXA");
        string[] wards = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; ++i)
        {
            wards[i] = (string)dt.Rows[i]["TEN"];
        }

        Response.Write(JsonConvert.SerializeObject(wards));
    }

    #endregion

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

}