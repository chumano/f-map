using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ConfigureLayerNames
{
    class Program
    {
        private static Dictionary<char, char> dic = new Dictionary<char, char>();

        static void Main(string[] args)
        {
            SqlConnection myConnection = new SqlConnection(
                "user id=sde;" +
                "password=sde;server=NewAge\\SqlExpress;" +
                "Trusted_Connection=yes;" +
                "database=fdb;"
            );

            try
            {
                string[] districtNames = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "TB", "TP", "BT", "BTH", "GV", "PN", "TD", "BC"};
                string[] layerTypes = {"RG", "NHA", "GT"};

                myConnection.Open();
                SqlCommand myCommand = null;
                String query = null;
                
                // delete old layers
                myCommand = new SqlCommand("delete from Layer", myConnection);
                myCommand.ExecuteNonQuery();

                // add new layers
                int k = 0;
                const string INSERT_LAYER_QUERY = "insert into dbo.Layer (ID, LayerName, Layer, StyleName, Tbl_Name) values ({0}, '{1}', '{2}', '{3}', '{4}')";
                string prefix = null;
                for (int i = 0; i < districtNames.Length; ++i) {
                    if (i == (districtNames.Length - 1))
                    {
                        prefix = "HUYEN";
                    }
                    else
                    {
                        prefix = "QUAN";
                    }

                    for (int j = 0; j < layerTypes.Length; ++j) {
                        k++;

                        query = String.Format(INSERT_LAYER_QUERY, 
                            k, 
                            prefix + districtNames[i] + "_" + layerTypes[j], 
                            "sde:" + prefix + districtNames[i] + "_" + layerTypes[j],
                            j % layerTypes.Length == 0 ? (prefix + districtNames[i]) : "polygon",
                            prefix + districtNames[i] + "_" + layerTypes[j] + "_PROJECT"
                        );
                        myCommand = new SqlCommand(query, myConnection);
                        myCommand.ExecuteNonQuery();
                    }
                }

                // delete old layer map
                myCommand = new SqlCommand("delete from LayerMap", myConnection);
                myCommand.ExecuteNonQuery();

                // insert new layer map
                const string INSERT_LAYER_MAP_QUERY = "insert into dbo.LayerMap (MapID, LayerID, Enable, Basic, OrderL) values ({0}, {1}, '{2}', '{3}', {4})";
                // mapID = 0
                int count = 0;
                for (int i = 1; i <= districtNames.Length * layerTypes.Length; ++i)
                {
                    // "RG", "NHA", "GT"
                    if (i % layerTypes.Length != 0)
                    {
                        count++;
                        query = String.Format(INSERT_LAYER_MAP_QUERY,
                            0,
                            i,
                            i % layerTypes.Length == 2 ? "False" : "True",
                            i % layerTypes.Length == 1 ? "True" : "False",
                            count
                        );
                        myCommand = new SqlCommand(query, myConnection);
                        myCommand.ExecuteNonQuery();
                    }
                }
                // mapID != 0
                // MapID, LayerID, Enable, Basic, OrderL
                for (int mapId = 1; mapId <= districtNames.Length; ++mapId)
                {
                    // "RG", "NHA", "GT"
                    for (int i = layerTypes.Length - 1; i >= 0; --i)
                    {
                        query = String.Format(INSERT_LAYER_MAP_QUERY,
                            mapId,
                            mapId * layerTypes.Length - i,
                            "True",
                            i % layerTypes.Length == 1 ? "True" : "False",
                            layerTypes.Length - i
                        );
                        myCommand = new SqlCommand(query, myConnection);
                        myCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                myConnection.Close();
            }

            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
