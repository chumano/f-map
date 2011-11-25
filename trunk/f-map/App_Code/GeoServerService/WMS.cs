using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Newtonsoft.Json;

/// <summary>
/// Summary description for WMS
/// </summary>

namespace Service.WMS
{
    public class WMS
    {
        //url : http://localhost:8080/geoserver/wms?
        string URL;

        public WMS(string url)
        {
            URL = url;
            //
            // TODO: Add constructor logic here
            //
        }

        //********GetCapabilities************
        //VERSION=1.1.1&REQUEST=GetCapabilities&SERVICE=WMS
        public string GetCapabilities(CapRequest par)
        {
            string requestURL = URL + par.ToString();

            CapResponse response = new CapResponse(Utils.GetRequest(requestURL));

            return response.getInfo();
        }

        //***********GetMap************
        //bbox=-130,24,-66,50&styles=population&Format=image/png&request=GetMap&layers=topp:states
        //&width=550&height=250&srs=EPSG:4326
        //SERVICE=WMS&VERSION=1.1.1&REQUEST=GetMap&LAYERS=States,Cities&STYLES=&SRS=EPSG:4326&BBOX=-124,21,-66,49&WIDTH=600&HEIGHT=400&FORMAT=image/png
        public void GetMap(MapRequest par)
        {
        }

        //***********GetFeatureInfo***********
        //SERVICE=WMS&VERSION=1.1.1&REQUEST=GetFeatureInfo&SRS=EPSG:4326&BBOX=-117,38,-90,49
        //&WIDTH=600&HEIGHT=400&QUERY_LAYERS=States&X=200&Y=150
        public string GetFeatrueInfo(FeatureInfoRequest par)
        {
            string requestURL = URL + par.ToString();

            FeatureResponse response = new FeatureResponse(Utils.GetRequest(requestURL));

            return response.getInfo(1);
        }

    }

    #region GetCapabilities
    public class CapRequest
    {
        string Service = "WMS"; //default
        string Request = "GetCapabilities"; //default

        string Version = ""; //option     -- 1.0.0, 1.1.0, and 1.1.1.

        string UpdateSequence = ""; //option

        public CapRequest(string version, string updateSeq)
        {
            Version = version;
            UpdateSequence = updateSeq;
        }

        public CapRequest()
        {
        }

        override public string ToString()
        {
            string result = "";
            //Version
            if (Version != "") result += "version=" + Version;
            //Service
            if (result != "") result += "&";
            result += "service=" + Service;
            //Request
            if (result != "") result += "&";
            result += "request=" + Request;

            return result;
        }
    }

    public class CapResponse
    {
        string Response;
        public CapResponse(string responseStr)
        {
            Response = responseStr;
        }

        public string getInfo()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Response);

            XmlNodeList nodeLayerList = doc.GetElementsByTagName("Layer");
            //Cai Layer dau tien chua nhieu cai Layer con co o dang sau
            //Nen chi lay các layer o sau , bo cai dau tien
            #region example
            /*
             * <Layer queryable="1">
                <Name>Test:NHA_Q1_Project</Name>    //0
                <Title>NHA_Q1_Project</Title>       //1
                <Abstract/>                         //2
                <KeywordList/>                      //3
                <CRS>EPSG:23868</CRS>               //4
                <CRS>CRS:84</CRS>                   //5
                <EX_GeographicBoundingBox>          //6
                  <westBoundLongitude>106.682</westBoundLongitude>
                  <eastBoundLongitude>106.713</eastBoundLongitude>
                  <southBoundLatitude>10.754</southBoundLatitude>
                  <northBoundLatitude>10.797</northBoundLatitude>
                </EX_GeographicBoundingBox>
                <BoundingBox CRS="EPSG:23868" minx="683914.611" miny="1189261.303" maxx="687340.714" maxy="1193989.768"/>
                                                    //7 
                <Style>                             //8 ++
                  <Name>polygon</Name>
                  <Title>Default Polygon</Title>
                  <Abstract>A sample style that draws a polygon</Abstract>
                  <LegendURL width="20" height="20">
                    <Format>image/png</Format>
                    <OnlineResource xmlns:xlink="http://www.w3.org/1999/xlink" xlink:type="simple" xlink:href="http://localhost:8080/geoserver/ows?service=WMS&amp;request=GetLegendGraphic&amp;format=image%2Fpng&amp;width=20&amp;height=20&amp;layer=NHA_Q1_Project"/>
                  </LegendURL>
                </Style>
              </Layer>
             */
            #endregion
            List<Layer> layerList = new List<Layer>();
            for (int i = 1; i < nodeLayerList.Count; i++)
            {
                XmlNode layerNode = nodeLayerList.Item(i);
                XmlNodeList attrs = layerNode.ChildNodes;

                Layer layer;
                string _name = "";
                string _layer = "";
                string _crs = "";
                string _style ="";
                GeoBoundingBox geoBox = null ;
                BoundingBox box = null;
                //Name of Layer
                foreach (XmlNode node in attrs)
                {
                    switch(node.Name)
                    {
                        case "Name":
                            _layer = node.InnerText;
                            break;
                        case "Title":
                            _name = node.InnerText;
                            break;
                        case "CRS":
                            if(_crs=="") _crs = node.InnerText;
                            break;
                        case "EX_GeographicBoundingBox":
                             geoBox = new GeoBoundingBox(Convert.ToDouble(node.ChildNodes.Item(0).InnerText),
                                                            Convert.ToDouble(node.ChildNodes.Item(1).InnerText),
                                                            Convert.ToDouble(node.ChildNodes.Item(2).InnerText),
                                                            Convert.ToDouble(node.ChildNodes.Item(3).InnerText));
               
                            break;
                        case "BoundingBox":
                            box = new BoundingBox(Convert.ToDouble(node.Attributes.Item(1).Value),
                                                    Convert.ToDouble(node.Attributes.Item(2).Value),
                                                    Convert.ToDouble(node.Attributes.Item(3).Value),
                                                    Convert.ToDouble(node.Attributes.Item(4).Value));

                            break;
                        case "Style":
                            if(_style=="") _style = node.ChildNodes.Item(0).InnerText;
                            break;
                        default:
                            break;
                            
                    }
                }
               
                
              
                layer = new Layer(_name, _layer, _style, _crs);
                layer.GeoBox = geoBox;
                layer.Box = box;

                layerList.Add(layer);
            }

            return JsonConvert.SerializeObject(layerList.ToArray<Layer>());

        }
    }
    #endregion

    #region GetMap
    public class MapRequest
    {
        string Service = "WMS"; //default
        string Request = "GetMap"; //default

        string Version = ""; //option     -- 1.0.0, 1.1.0, and 1.1.1.

        string SRS = "EPRG:4326";

        public string Layers; //layer1,layer2,..
        public string Styles; //style1,style2,...

        public string Format= "image/jpeg";  //-------image/png,image/jpeg, application/vnd.google-earth.kml+xml,...

        public BoundingBox BBox; //minx,miny,maxx,maxy
        public string Width;
        public string Height;

        public string BGColor = "0xffffff"; //0xffeeff
        public string Transparent = "false"; //true/false

        public string SLD;//sld_url

        string Exceptions;  // exception_format, default : application/vnd.ogc.se_xml
        // others: pplication/vnd.ogc.inimage and application/vnd.ogc.se_blank
        //notuse
        string Reaspect;//true/false
        string ServiceName;
        public MapRequest()
        {
        }
    }

    #endregion

    #region GetFeatureInfo
    public class FeatureInfoRequest
    {
        string Service = "WMS"; //default
        string Request = "GetFeatureInfo"; //default

        string Version = ""; //option     -- 1.0.0, 1.1.0, and 1.1.1.

        public BoundingBox BBox;
        public double Width;
        public double Height;

        public string Layers;
        public string Query_Layers; //layer list

        public double X, Y;

        string Info_Format = "application/vnd.ogc.gml"; //output format
        //The default value is application/vnd.ogc.wms_xml. 
        //Other : text/html, and text/plain.
        //@@: text/plain, application/vnd.ogc.gml, text/html

        int Feature_count = 1; //So feature tra ve/ layer
                           // default la 1

        //khong dung
        string Exceptions;
        string ServiceName;
        public FeatureInfoRequest(string layers, BoundingBox box, double w, double h, double x, double y)
        {
            Layers = Query_Layers = layers;
            BBox = box;
            Width = w; Height = h;
            X = x; Y = y;

        }

        //bbox=-130,24,-66,50&info_format=application/vnd.ogc.gml&
        //request=GetFeatureInfo&layers=topp:states&query_layers=topp:states&width=550&height=250&x=170&y=160
        override public string ToString()
        {
            string result = "";
            //Version
            if (Version != "") result += "version=" + Version;
            //Service
            if (result != "") result += "&";
            result += "service=" + Service;
            //Request
            result += "&request=" + Request;
            //BoundingBox
            result += "&bbox=" + BBox.ToString();
            //Width
            result += "&width=" + Width;
            //Height
            result += "&height=" + Height;
            //x
            result += "&x=" + X;
            //y
            result += "&y=" + Y;
            //info_format
            result += "&info_format=" + Info_Format;
            //Feature_count
            result += "&feature_count=" + Feature_count;
            //layer
            result += "&layers=" + Layers;
            //query_layers
            result += "&query_layers=" + Query_Layers;
            //SRS = EPSG:4326
            result += "&SRS=EPSG:4326";
            return result;
        }
    }

    public class FeatureResponse
    {
        string Response;
        public FeatureResponse(string response)
        {
            Response = response;
        }
          //<gml:featureMember>
          //  <topp:states fid="states.11">
          //    <topp:the_geom>
          //      <.......>
          //    </topp:the_geom>
          //    <topp:STATE_NAME>Arizona</topp:STATE_NAME>
          //    <topp:STATE_FIPS>04</topp:STATE_FIPS>
          //    <topp:SUB_REGION>Mtn</topp:SUB_REGION>
          //    <topp:STATE_ABBR>AZ</topp:STATE_ABBR>
          //  </topp:states>
          //</gml:featureMember>
        public string getInfo(int kindFormat)
        {
            if (kindFormat == 0)   //text/plain
                return getInfoFromText();
            if (kindFormat == 1) //xml
                return getInfoFromXML();
            return "Chua ho tro";
        }

        private string getInfoFromXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Response);

            XmlNodeList featureNodeList = doc.GetElementsByTagName("gml:featureMember");

            List<Feature> features = new List<Feature>();
            foreach (XmlNode featureNode in featureNodeList)
            {
                XmlNodeList featureIDNodeList = featureNode.ChildNodes;
                foreach (XmlNode featureIDNode in featureIDNodeList)
                {
                    //Lay ten cua layer
                    string name = featureIDNode.Name;
                    string fid = featureIDNode.Attributes.Item(0).Value;
                    List<Data> attrs = new List<Data>();
                    //bo thuoc tinh geom
                    XmlNodeList attrNodeList = featureIDNode.ChildNodes;
                    foreach (XmlNode attrNode in attrNodeList)
                    {
                        if (!attrNode.Name.Contains("the_geom"))
                        {
                            Data dt = new Data(attrNode.Name, attrNode.InnerText);
                            attrs.Add(dt);
                        }
                    }

                    Feature feature = new Feature(name, fid, attrs);
                    features.Add(feature);
                }
            }

            return JsonConvert.SerializeObject(features.ToArray());
        }

        private string getInfoFromText()
        {
            /*
            Results for FeatureType 'QUAN1_RG':
            --------------------------------------------
            OBJECTID = 2
            MAHC = 7.602674E7
            TEN = PhÆ°á»ng Báº¿n NghÃ©
            CAPHC = xÃ£
            KIEUHC = phÆ°á»n
            SOHO = 4967
            Shape = [GEOMETRY (MultiPolygon) with 145 points]
            --------------------------------------------
             */

            return "";
        }
    }
    #endregion

}