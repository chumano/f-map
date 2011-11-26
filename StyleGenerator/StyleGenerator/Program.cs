using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Drawing;

namespace StyleGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] districtNames = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "BT", "BTH", "GV", "PN", "TP", "TD", "TB", "BC" };
            string[] colorCodes = new string[districtNames.Length];
            colorCodes[0] = "#e7e4d3";
            colorCodes[12] = "#e7e4d3";
            colorCodes[18] = "#e7e4d3";
            colorCodes[17] = "#e7e4d3";

            colorCodes[1] = "#ebf4fa";
            colorCodes[2] = "#ebf4fa";
            colorCodes[7] = "#ebf4fa";
            colorCodes[10] = "#ebf4fa";
            colorCodes[14] = "#ebf4fa";

            colorCodes[4] = "#f1efe2";
            colorCodes[6] = "#f1efe2";
            colorCodes[8] = "#f1efe2";
            colorCodes[16] = "#f1efe2";
            colorCodes[13] = "#f1efe2";

            colorCodes[3] = "#e1e1e1";
            colorCodes[5] = "#e1e1e1";
            colorCodes[9] = "#e1e1e1";
            colorCodes[11] = "#e1e1e1";
            colorCodes[19] = "#e1e1e1";
            colorCodes[15] = "#e1e1e1";

            //toanthanh
            for (int i = 0; i < districtNames.Length; i++)
            {
                string prefix = null;
                if (i == districtNames.Length-1)
                {
                    prefix = "HUYEN";
                }
                else
                {
                    prefix = "QUAN";
                }

                string fileName = prefix + districtNames[i];
                createFile2(fileName, colorCodes[i]);
            }
        }

        private static void createFile2(string fileName, string colorCode)
        {
            string lines = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?> <StyledLayerDescriptor version=\"1.0.0\" xmlns=\"http://www.opengis.net/sld\" xmlns:ogc=\"http://www.opengis.net/ogc\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:gml=\"http://www.opengis.net/gml\" xsi:schemaLocation=\"http://www.opengis.net/sld http://schemas.opengis.net/sld/1.0.0/StyledLayerDescriptor.xsd\">";
            lines += "<NamedLayer><UserStyle><FeatureTypeStyle>";

            lines += "<Rule>";
            lines += "<PolygonSymbolizer>";
            lines += String.Format(
                "<Fill><CssParameter name=\"fill\">{0}</CssParameter></Fill>",
                colorCode
            );
            lines += "</PolygonSymbolizer>";
            lines += "<TextSymbolizer><Label><ogc:PropertyName>TEN</ogc:PropertyName></Label><Font><CssParameter name=\"font-family\">Arial</CssParameter><CssParameter name=\"font-size\">11</CssParameter><CssParameter name=\"font-style\">normal</CssParameter></Font><LabelPlacement><PointPlacement><AnchorPoint><AnchorPointX>0.5</AnchorPointX><AnchorPointY>0.5</AnchorPointY></AnchorPoint></PointPlacement></LabelPlacement><Fill><CssParameter name=\"fill\">#000000</CssParameter></Fill><VendorOption name=\"autoWrap\">60</VendorOption><VendorOption name=\"maxDisplacement\">150</VendorOption></TextSymbolizer>";
            lines += "</Rule>";

            lines += "</FeatureTypeStyle></UserStyle></NamedLayer></StyledLayerDescriptor>";

            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter("F:\\LuanVanMoi\\Data files\\map styles\\toan thanh\\" + fileName + "_TT.xml");
            file.WriteLine(lines);

            file.Close();
        }

        private static void createFile(string fileName)
        {
            string lines = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?> <StyledLayerDescriptor version=\"1.0.0\" xmlns=\"http://www.opengis.net/sld\" xmlns:ogc=\"http://www.opengis.net/ogc\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:gml=\"http://www.opengis.net/gml\" xsi:schemaLocation=\"http://www.opengis.net/sld http://schemas.opengis.net/sld/1.0.0/StyledLayerDescriptor.xsd\">";
            lines += "<NamedLayer><UserStyle><FeatureTypeStyle>";

            SqlConnection myConnection = new SqlConnection("user id=sde;" +
                                       "password=sde;server=NewAge\\SqlExpress;" +
                                       "Trusted_Connection=yes;" +
                                       "database=sde;");

            try
            {


                myConnection.Open();

                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select * from sde." + fileName + "_RG_PROJECT", myConnection);
                myReader = myCommand.ExecuteReader();
                int mahc;
                Random random = new Random();
                while (myReader.Read())
                {
                    mahc = Convert.ToInt32(myReader["MAHC"]);

                    lines += "<Rule>";

                    lines += "<ogc:Filter><ogc:PropertyIsEqualTo>";
                    lines += "<ogc:PropertyName>MAHC</ogc:PropertyName>";
                    lines += String.Format("<ogc:Literal>{0}</ogc:Literal>", mahc);
                    lines += "</ogc:PropertyIsEqualTo></ogc:Filter>";

                    lines += "<PolygonSymbolizer>";
                    lines += String.Format(
                        "<Fill><CssParameter name=\"fill\">{0}</CssParameter><CssParameter name=\"fill-opacity\">0.7</CssParameter></Fill>",
                        String.Format("#{0:X6}", random.Next(0x1000000))
                    );
                    lines += "</PolygonSymbolizer>";

                    lines += "</Rule>";
                }

                lines += "<Rule><Title>Boundary</Title><LineSymbolizer><Stroke><CssParameter name=\"stroke-width\">0.2</CssParameter></Stroke></LineSymbolizer><TextSymbolizer><Label><ogc:PropertyName>TEN</ogc:PropertyName></Label><Font><CssParameter name=\"font-family\">TimesNewRoman</CssParameter><CssParameter name=\"font-style\">Normal</CssParameter><CssParameter name=\"font-size\">14</CssParameter></Font><LabelPlacement><PointPlacement><AnchorPoint><AnchorPointX>0.5</AnchorPointX><AnchorPointY>0.5</AnchorPointY></AnchorPoint></PointPlacement></LabelPlacement></TextSymbolizer></Rule>";
                lines += "</FeatureTypeStyle></UserStyle></NamedLayer></StyledLayerDescriptor>";

                myReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                myConnection.Close();
            }

            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter("F:\\LuanVanMoi\\Data files\\map styles\\"+fileName+".xml");
            file.WriteLine(lines);

            file.Close();
        }
    }
}
