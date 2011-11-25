using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Xml;

/// <summary>
/// Summary description for WFS
/// </summary>

namespace Service.WFS
{
    public class WFS
    {
        //url http://localhost:8080/geoserver/wfs?
        public string URL;

        public WFS(string url)
        {
            URL = url;
        }


        //* GetCapabilities
        //request=getcapabilities&service=WFS&version=1.0.0
        public string GetCapabilities(CapRequest par)
        {
            string requestURL = URL + par.ToString();

            CapResponse response = new CapResponse(Utils.GetRequest(requestURL));

            return response.getInfo();
        }

        //* DescribeFeatureType
        //request=describefeaturetype&service=wfs&version=1.0.0&outputformat=xmlschema
        public void DescribeFeatureType()
        {
        }

        //* GetFeature

        //* GetFeatureWithLock

        //* LockFeature

        //* Transaction, with a subelement specifying the transaction type:

        //    * Insert

        //    * Update

        //    * Delete

    }

    #region GetCapabilities
    public class CapRequest
    {
        public string Request = "GetCapabilities";
        public string Service = "WFS";
        public string Version = "1.0.0";

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

        public CapResponse(string response)
        {
            Response = response;
        }

        public string getInfo()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Response);

            XmlNodeList nodeFeatureTypeList1 = doc.GetElementsByTagName("FeatureTypeList");
            XmlNodeList nodeFeatureTypeList = nodeFeatureTypeList1.Item(0).ChildNodes;
            //Operations
            //Roi cac FeatureType
            #region example
            /*
            <FeatureType>
              <Name>Test:Quan1_RG_Project</Name>
              <Title>Quan1_RG_Project</Title>
              <Abstract/>
              <Keywords/>
              <SRS>EPSG:23868</SRS>
              <LatLongBoundingBox minx="106.682" miny="10.753" maxx="106.714" maxy="10.797"/>
            </FeatureType>
            */
            #endregion
            List<FeatureType> featureTypeList = new List<FeatureType>();
            for (int i = 1; i < nodeFeatureTypeList.Count; i++)
            {
                XmlNode featureTypeNode = nodeFeatureTypeList.Item(i);
                XmlNodeList attrs = featureTypeNode.ChildNodes;

                FeatureType featureType;
                string _layer = "";
                string _srs = "";
                GeoBoundingBox geoBox = null;

                //Name of Layer
                foreach (XmlNode node in attrs)
                {
                    switch (node.Name)
                    {
                        case "Name":
                            _layer = node.InnerText;
                            break;
                        case "SRS":
                            _srs = node.InnerText;
                            break;
                        case "LatLongBoundingBox":
                            if (node.Attributes.Count < 4) break;
                            geoBox = new GeoBoundingBox(Convert.ToDouble(node.Attributes.Item(0).InnerText),    //w - minX
                                                           Convert.ToDouble(node.Attributes.Item(2).InnerText), //e - maxX
                                                           Convert.ToDouble(node.Attributes.Item(1).InnerText), //s - minY
                                                           Convert.ToDouble(node.Attributes.Item(3).InnerText));//n - maxY

                            break;

                        default:
                            break;

                    }
                }

                featureType = new FeatureType(_layer, _srs, geoBox);

                featureTypeList.Add(featureType);
            }

            return JsonConvert.SerializeObject(featureTypeList.ToArray<FeatureType>());
        }
    }

    #endregion

    #region DescrbeFeatureType
    public class FeatureTypeRequest
    {
        string Request = "DescribeFeatureType";
        string Service = "WFS";

        string Version = "1.0.0";

        public string OutputFormat = "XMLSCHEMA"; //only supported value
        public string TypeName; //list of feature type name

        public FeatureTypeRequest(List<string> typeNameList)
        {
            TypeName = "";
            int i;
            for (i = 0; i < typeNameList.Count - 1; i++)
            {
                TypeName += typeNameList[i]+",";
            }

            if (i < typeNameList.Count) TypeName += typeNameList[i];
        }

        public override string ToString()
        {
            return "Service=" + Service
                    + ",Request=" + Request
                    + ",Version=" + Version
                    + ",OutputFormat=" + OutputFormat
                    + ",TypeName=" + TypeName;
        }
    }
    public class FeatureTypeResponse
    {
        string Response;
        public FeatureTypeResponse(string response)
        {
            Response = response;
        }

        public string getInfo()
        {
            return Response;
        }

    }
    #endregion

}