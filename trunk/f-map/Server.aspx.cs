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
                //case "Test":
                //    Test t = new Test();
                //    string s = t.run();
                //    Response.Write(s);
                //    break;

                // get all MapView objects
                // action=GetMapView
                case "GetMapView":
                    getMapView();
                    break;

                // get all street names
                // action=GetAllStreet
                case "GetAllStreet":
                    getAllStreetName();
                    break;

                // get information when click on map
                // action=GetInfo&bbox=&x=&y=&layer_name=&width=&height=
                case "GetInfo":
                    string url = String.Format(Config.URL_GET_INFO,
                                                Request["bbox"],
                                                Request["x"],
                                                Request["y"],
                                                Request["layer_name"],
                                                Request["layer_name"],
                                                Request["width"],
                                                Request["height"]);

                    BoundingBox bbox = new BoundingBox(Request["bbox"]);
                    Service.WMS.FeatureInfoRequest req = new Service.WMS.FeatureInfoRequest(
                                                Request["layer_name"],
                                                bbox,
                                                Convert.ToDouble(Request["width"]),
                                                Convert.ToDouble(Request["height"]),
                                                Convert.ToDouble(Request["x"]),
                                                Convert.ToDouble(Request["y"])
                    );

                    Response.Write(wms.GetFeatrueInfo(req));
                    break;

                // get information of a map (a district or city)
                // action=GetMap&map_id=
                case "GetMap":
                    GetMap(Request["map_id"]);
                    break;

                // get all wards of a district
                // action=GetWards&district_id=
                case "GetWards":
                    GetWards(Request["district_id"]);
                    break;

                // search address by so nha, ten duong khong dau, ma phuong
                // action=SearchAddress&NoAdd=&StrNoName=&IDWard=
                case "SearchAddress":
                    List<Point> plist = checkAddress(Request["NoAdd"].ToString(), Request["StrNoName"].ToString(), Request["IDWard"].ToString());
                    if (plist.Count == 0) //ko thay
                    {
                        CheckAddressResult rsl = new CheckAddressResult(false, plist);
                        Response.Write(JsonConvert.SerializeObject(rsl));
                    }
                    else
                    {
                        CheckAddressResult rsl = new CheckAddressResult(true, plist);
                        Response.Write(JsonConvert.SerializeObject(rsl));
                    }
                    break;
            }
        }
    }

    // perform a HTTP GET and return a reponse string
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

    private void GetMap(string mapId)
    {
        // TODO test 
        // mapId = "1";

        // Get map bound
        DataTable dt = Helper.GetDataTable("select * from MapView where ID = " + mapId );

        DataRow row = dt.Rows[0];
        // double minx, double miny, double maxx, double maxy
        BoundingBox bound = new BoundingBox((double)row["MinX"], (double)row["MinY"], (double)row["MaxX"], (double)row["MaxY"]);

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

    private void GetWards(string districtId)
    {
        string[] districtNames = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "BT", "BTH", "GV", "PN", "TP", "TD", "TB", "BC" };

        //toanthanh
        if (districtId == "0") return;

        string prefix = null;
        if (int.Parse(districtId) == districtNames.Length)
        {
            prefix = "HUYEN";
        }
        else
        {
            prefix = "QUAN";
        }

        DataTable dt = Helper.GetDataTable2("select * from " + prefix + districtNames[int.Parse(districtId)-1] + "_RG_PROJECT");
        string[] wards = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; ++i)
        {
            wards[i] = (string)dt.Rows[i]["TEN"];
        }

        Response.Write(JsonConvert.SerializeObject(wards));
    }

    //private String samplePostRequest()
    //{
    //    String url = "http://localhost:8080/geoserver/wfs";
    //    String postData = "<wfs:GetFeature service=\"WFS\" version=\"1.1.0\"  xmlns:topp=\"http://www.openplans.org/topp\"  xmlns:wfs=\"http://www.opengis.net/wfs\"  xmlns:ogc=\"http://www.opengis.net/ogc\"  xmlns:gml=\"http://www.opengis.net/gml\"  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"  xsi:schemaLocation=\"http://www.opengis.net/wfs http://schemas.opengis.net/wfs/1.1.0/wfs.xsd\">  <wfs:Query typeName=\"topp:states\">    <wfs:PropertyName>topp:STATE_NAME</wfs:PropertyName>    <wfs:PropertyName>topp:PERSONS</wfs:PropertyName>    <ogc:Filter>        <ogc:PropertyIsLike escape=\"\\\" singleChar=\"_\" wildCard=\"%\">		    <ogc:PropertyName>topp:STATE_NAME</ogc:PropertyName>    	    <ogc:Literal>%New%</ogc:Literal>	   </ogc:PropertyIsLike>   </ogc:Filter>  </wfs:Query></wfs:GetFeature>";

    //    HttpWebRequest request = null;
    //    Uri uri = new Uri(url);
    //    request = (HttpWebRequest)WebRequest.Create(uri);
    //    request.Method = "POST";
    //    request.ContentType = "text/xml";
    //    request.ContentLength = postData.Length;
    //    using (Stream writeStream = request.GetRequestStream())
    //    {
    //        UTF8Encoding encoding = new UTF8Encoding();
    //        byte[] bytes = encoding.GetBytes(postData);
    //        writeStream.Write(bytes, 0, bytes.Length);
    //    }
    //    string result = string.Empty;
    //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
    //    {
    //        using (Stream responseStream = response.GetResponseStream())
    //        {
    //            using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
    //            {
    //                result = readStream.ReadToEnd();
    //            }
    //        }
    //    }
    //    return result;
    //}

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
        string sqlStr = "SELECT ID, Name,NoName From MapView Order by ID";
        DataTable tbl = Helper.GetDataTable(sqlStr);
        if (tbl.Rows.Count > 0)
        {
            List<MapView> list = new List<MapView>();
            foreach (DataRow row in tbl.Rows)
            {
                MapView view = new MapView(Convert.ToInt32(row[0]), row[1].ToString(), row[2].ToString());
                
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

    private List<Point> checkAddress(string NoAdd, string StrNoName, string IDWard)
    {
        List<Point> pList = new List<Point>();
        string sqlStr = "Select B.Tbl_Name, A.IDStreet"
                    +" From StreetInfo A, Districts B, Streets C"
                    + " WHERE A.IDStreet=C.IDStreet"
                        + " AND A.IDDistrict=B.IDDistrict "
                        + " AND C.NoName = N'" + StrNoName 
                        + "' AND A.IDWard=N'" + IDWard + "'";
        DataTable tbl = Helper.GetDataTable(sqlStr);

        string _oid = "1", tblName = "NHA_Q1";
        DataTable tbl2 = null;
        for (int i = 0; i < tbl.Rows.Count; i++)
        {
            tblName = tbl.Rows[i][0].ToString();
            string _MaDuong = tbl.Rows[i][1].ToString();
            tbl2 = Helper.GetDataTable2("Select X, Y, SoNha From " + tblName 
                                        + " WHERE MaPhuong=N'" + IDWard 
                                        + "' AND IDConDuong='" + _MaDuong + "' AND SoNha='" + NoAdd + "'");
            if (tbl2.Rows.Count > 0)
            {
                //tim thay co
                double x = Convert.ToDouble(tbl2.Rows[0]["X"]);
                double y = Convert.ToDouble(tbl2.Rows[0]["Y"]);
                Point p = new Point(x, y);
                pList.Add(p);
                return pList;
            }
        }

        //không có địa chỉ giống thì tìm gần giống        
        for (int i = 0; i < tbl.Rows.Count; i++)
        {
            tblName = tbl.Rows[i][0].ToString();
            string _MaDuong = tbl.Rows[i][1].ToString();
            string _SoNha = "";

            int min = 50000;
            tbl2 = Helper.GetDataTable2("Select objectid, SoNha, X, Y From " + tblName 
                                        + " WHERE MaPhuong=N'" + IDWard + "' AND IDConDuong='" + _MaDuong + "'");
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
                        _oid = tbl2.Rows[j]["objectid"].ToString();
                        tbl2 = Helper.GetDataTable2("Select X, Y From " + tblName + " WHERE objectid='" + _oid + "'");
                        if (tbl2.Rows.Count > 0)
                        {
                            //tim thay co
                            double x = Convert.ToDouble(tbl2.Rows[0]["X"]);
                            double y = Convert.ToDouble(tbl2.Rows[0]["Y"]);
                            Point p = new Point(x, y);
                            pList.Add(p);
                            return pList;
                        }
                    }
                    else if (!OddOrEven(Math.Abs(_SoNhaInt - _NoAddInt)) && (min > Math.Abs(_SoNhaInt - _NoAddInt)))
                    {
                        min = Math.Abs(_SoNhaInt - _NoAddInt);
                        _oid = tbl2.Rows[j]["objectid"].ToString();
                    }
                }
            }
        }

        ////tìm địa chỉ gần địa chỉ cần tìm nhất
        tbl2 = Helper.GetDataTable2("Select X, Y, SoNha From " + tblName + " WHERE  objectid='" + _oid + "'");
        if (tbl2.Rows.Count > 0)
        {
            //tim thay co
            double x = Convert.ToDouble(tbl2.Rows[0]["X"]);
            double y = Convert.ToDouble(tbl2.Rows[0]["Y"]);
            Point p = new Point(x, y);
            pList.Add(p);
            return pList;
        }
        else
        {
            return pList;
        }

    }
}