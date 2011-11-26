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
                greateFile(fileName);
            }
        }

        public static void greateFile(string fileName)
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
