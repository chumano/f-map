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
using System.Text.RegularExpressions;

public partial class Server : System.Web.UI.Page
{
    Service.WMS.WMS wms = new Service.WMS.WMS("http://localhost:8080/geoserver/wms?");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["action"] != null && Request["action"].Trim() != "")
        {
            switch (Request["action"])
            {
                case "Test":
                    Test t = new Test();
                    string s = t.run();
                    Response.Write(s);
                 
                    break;
                case "GetMapView":
                    getMapView();
                    break;
                case "GetAllStreet":
                    getAllStreetName();
                    break;
                case "GetInfo":
                    //// http://localhost:8080/geoserver/wms?REQUEST=GetFeatureInfo&BBOX={0}&SERVICE=WMS&VERSION=1.1.1&X={1}&Y={2}
                    //// &INFO_FORMAT=text/plain&QUERY_LAYERS={3}&FEATURE_COUNT=50&Layers={4}&WIDTH={5}&HEIGHT={6}&srs=EPSG:4326
                    string url = String.Format(Config.URL_GET_INFO,
                                                Request["bbox"],
                                                Request["x"],
                                                Request["y"],
                                                Request["layer_name"],
                                                Request["layer_name"],
                                                Request["width"],
                                                Request["height"]);
                    //GetInfo(url);
                    BoundingBox bbox = new BoundingBox(Request["bbox"]);
                    Service.WMS.FeatureInfoRequest req = new Service.WMS.FeatureInfoRequest(Request["layer_name"],bbox,
                                                Convert.ToDouble(Request["width"]),Convert.ToDouble(Request["height"]),
                                                Convert.ToDouble(Request["x"]),Convert.ToDouble(Request["y"]));
                    Response.Write(wms.GetFeatrueInfo(req));
                    break;

                case "GetMap":
                    GetMap(Request["map_id"]);
                    break;

                case "GetWards":
                    GetWards(Request["district_id"]);
                    break;

                case "SearchAddress":
                    SearchAddress(Request["keyword"]);
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

    #region GetInfo about a point clicked API

    private void GetInfo(string url)
    {
        // TODO: try catch error 

        string response = DoGetRequest(url);
        string[] array = response.Split(new char[] { '\r', '\n' });

        FeatureInfoItemModel[] featureInfoItems = new FeatureInfoItemModel[3];

        featureInfoItems[0] = new FeatureInfoItemModel();
        featureInfoItems[0].name = "Mã hành chính";
        featureInfoItems[0].value = ParseFeatureInfoItem(array[4].Replace("E7", "").Replace(".", ""));

        featureInfoItems[1] = new FeatureInfoItemModel();
        featureInfoItems[1].name = "Tên";
        featureInfoItems[1].value = ParseFeatureInfoItem(array[6]);

        featureInfoItems[2] = new FeatureInfoItemModel();
        featureInfoItems[2].name = "Số hộ";
        featureInfoItems[2].value = ParseFeatureInfoItem(array[12]);
        
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
        DataTable dt = Helper.GetDataTable("select * from MapView where ID = " + mapId );

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
        string[] districtNames = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "BT", "BTH", "GV", "PN", "TP", "TD", "TB", "BC" };

        //toanthanh
        if (districtId == "0") return;

        string prefix = null;
        if (int.Parse(districtId) == districtNames.Length - 1)
        {
            prefix = "HUYEN";
        }
        else
        {
            prefix = "QUAN";
        }

        //hardcode
        DataTable dt = Helper.GetDataTable2("select * from " + prefix + districtNames[int.Parse(districtId)] + "_RG_PROJECT");
        string[] wards = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; ++i)
        {
            wards[i] = (string)dt.Rows[i]["TEN"];
        }

        Response.Write(JsonConvert.SerializeObject(wards));
    }

    #endregion

    #region SearchAddress API

    private void SearchAddress(string keyword)
    {
        // 268 ly thuong kiet
        keyword = NameUtil.GetInstance().Convert(keyword);
        string[] keys = keyword.Split(',');
        string query = null;
        if (keys.Length == 2)
        {
            query = String.Format("select distinct SoNha, TenConDuong, X, Y from QUAN1 where SoNha like '{0}' and TenConDuong2 like '{1}'", keys[0].Trim(), keys[1].Trim());
        }
        else
        {
            query = String.Format("select distinct SoNha, TenConDuong, X, Y from QUAN1 where SoNha like '%{0}%' or TenConDuong2 like '%{1}%'", keyword.Trim(), keyword.Trim());
        }
        DataTable dt = Helper.GetDataTable2(query);
        int len = dt.Rows.Count;
        AddressModel[] addresses = new AddressModel[len];
        DataRow row = null;
        for (int i = 0; i < len; ++i)
        {
            row = dt.Rows[i];

            addresses[i] = new AddressModel();
            addresses[i].SoNha = (string)row["SoNha"];
            addresses[i].TenDuong = (string)row["TenConDuong"];
            addresses[i].X = (decimal)row["X"];
            addresses[i].Y = (decimal)row["Y"];
        }

        string response = JsonConvert.SerializeObject(addresses);
        Response.Write(response);
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

    // ----------------------------------------------------------
    protected void getAllStreetName()
    {
        string sqlStr = "SELECT NoName, StreetName, WardName, IDWard, DistrictName From vw_address";
        DataTable tbl = Helper.GetDataTable(sqlStr);
        if (tbl.Rows.Count > 0)
        {
            List<StreetInfo> list = new List<StreetInfo>();
            foreach (DataRow row in tbl.Rows)
            {
                StreetInfo addr = new StreetInfo();
                addr.NoName = row[0].ToString();
                addr.StreetName = row[1].ToString();
                addr.WardName = row[2].ToString();
                addr.IDWard = row[3].ToString();
                addr.DistrictName = row[4].ToString();

                list.Add(addr);
            }

            Response.Write(JsonConvert.SerializeObject(list));
        }
    }

    private void getMapView()
    {
        string sqlStr = "SELECT ID, Name From MapView Order by ID";
        DataTable tbl = Helper.GetDataTable(sqlStr);
        if (tbl.Rows.Count > 0)
        {
            List<MapView> list = new List<MapView>();
            foreach (DataRow row in tbl.Rows)
            {
                MapView view = new MapView(Convert.ToInt32(row[0]), row[1].ToString());
                
                list.Add(view);
            }

            Response.Write(JsonConvert.SerializeObject(list));
        }
    }

    // -----------------------------------------------------------

    private int getNumberFromText(string txt)
    {
        System.Text.RegularExpressions.Regex re = new Regex(@"\d+");
        Match m = re.Match(txt);
        if (m.Success)
        {
            return Convert.ToInt32(m.Value);
        }
        else
        {
            return 50000;
        }
    }

    private bool OddOrEven(int number)
    {
        return ((number & 1) != 0) ? true : false;
    } 

    private string checkAddress(string NoAdd, string StrAdd, string Ward)
    {
        string sqlStr = "Select B.Tbl_Name, A.IDStreet"
                    +" From StreetInfo A, District B, Streets C"
                    + " WHERE A.IDStreet=C.IDStreet"
                        + " AND A.IDDistrict=B.IDDistrict "
                        + " AND C.NoName = N'" + StrAdd 
                        + "' AND A.IDWard=N'" + Ward + "'";
        DataTable tbl = Helper.GetDataTable(sqlStr);

        string _oid = "1", tblName = "NHA_Q1";
        DataTable tbl2 = null;
        for (int i = 0; i < tbl.Rows.Count; i++)
        {
            tblName = tbl.Rows[i][0].ToString();
            string _MaDuong = tbl.Rows[i][1].ToString();
            tbl2 = Helper.GetDataTable2("Select X, Y, SoNha From " + tblName 
                                        + " WHERE MaPhuong=N'" + Ward 
                                        + "' AND IDConDuong='" + _MaDuong + "' AND SoNha='" + NoAdd + "'");
            if (tbl2.Rows.Count > 0)
            {
                //tim thay co
                return tbl2.Rows[0]["X"].ToString() + "," + tbl2.Rows[0]["Y"].ToString();
            }
        }

        //không có địa chỉ giống thì tìm gần giống        
        for (int i = 0; i < tbl.Rows.Count; i++)
        {
            tblName = tbl.Rows[i][0].ToString();
            string _MaDuong = tbl.Rows[i][1].ToString();
            string _SoNha = "";

            int min = 50000;
            tbl2 = Helper.GetDataTable2("Select oid, SoNha, X, Y From " + tblName 
                                        + " WHERE MaPhuong=N'" + Ward + "' AND IDConDuong='" + _MaDuong + "'");
            for (int j = 0; j < tbl2.Rows.Count; j++)
            {
                _SoNha = tbl2.Rows[j]["SoNha"].ToString();
                if (_SoNha.IndexOf('/') == -1)
                {
                    int _SoNhaInt = getNumberFromText(_SoNha);
                    int _NoAddInt = getNumberFromText(NoAdd);
                    if (_SoNhaInt == _NoAddInt)
                    {
                        //ko tìm ra địa chỉ đúng thì tìm ngoài đầu hẻm
                        _oid = tbl2.Rows[j]["oid"].ToString();
                        tbl2 = Helper.GetDataTable2("Select X, Y From " + tblName + " WHERE oid='" + _oid + "'");
                        if (tbl2.Rows.Count > 0)
                            //tim thay co
                            return tbl2.Rows[0]["X"].ToString() + "," + tbl2.Rows[0]["Y"].ToString();
                    }
                    else if (!OddOrEven(Math.Abs(_SoNhaInt - _NoAddInt)) && (min > Math.Abs(_SoNhaInt - _NoAddInt)))
                    {
                        min = Math.Abs(_SoNhaInt - _NoAddInt);
                        _oid = tbl2.Rows[j]["oid"].ToString();

                    }
                }
            }
        }
        ////tìm địa chỉ gần địa chỉ cần tìm nhất
        tbl2 = Helper.GetDataTable2("Select X, Y, SoNha From " + tblName + " WHERE  oid='" + _oid + "'");
        if (tbl2.Rows.Count > 0)
        {
            //tim thay co
            return tbl2.Rows[0]["X"].ToString() + "," + tbl2.Rows[0]["Y"].ToString();
        }
        else
            return "";

    }

}