using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class Program
    {
        private static void TestString(string str)
        {
            for (int i = 0; i < str.ToCharArray().Length; ++i)
            {
                Console.Write((int)str.ToCharArray()[i] + ",");
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            //string str = "Nguyễn Bặc";
            //TestString(str);
            //TestString("Nguyễn Bặc");
            //TestString(ConvertName(str));
            //Console.ReadKey();
            //return;

            SqlConnection myConnection = new SqlConnection("user id=sde;" +
                                       "password=sde;server=NewAge\\SqlExpress;" +
                                       "Trusted_Connection=yes;" +
                                       "database=fdb;");
            SqlConnection myConnection2 = new SqlConnection("user id=sde;" +
                                       "password=sde;server=NewAge\\SqlExpress;" +
                                       "Trusted_Connection=yes;" +
                                       "database=fdb;");

            try
            {
                myConnection.Open();
                myConnection2.Open();

                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select StreetName from dbo.Streets", myConnection);
                myReader = myCommand.ExecuteReader();
                String oldName;
                int k = 0;
                while (myReader.Read())
                {
                    k++;
                    oldName = (myReader["StreetName"].ToString());

                    myCommand = new SqlCommand(
                        "update dbo.Streets set StreetName = N'" + ConvertName(oldName) + "' where ID = " + k,
                        myConnection2
                    );
                    myCommand.ExecuteNonQuery();
                }

                myReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                myConnection.Close();
                myConnection2.Close();
            }
        }

        private static String ConvertName(String oldName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int len = oldName.Length;
            char[] array = oldName.ToCharArray();
            char ch;
            for (int i = 0; i < len; ++i)
            {
                ch = array[i];
                if (!IsSpecialCharacter(ch))
                {
                    if (i + 1 < len && IsSpecialCharacter(array[i + 1]))
                    {
                        // sac 769
                        // huyen 768
                        // nga 771
                        // nang 803
                        if (ch == 'a')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)225);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)224);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)227);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7841);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7843);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'â')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)7845);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)7847);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)7851);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7853);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7849);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'ă')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)7855);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)7857);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7863);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'o')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)243);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)245);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7885);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7887);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)242);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'ô')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)7889);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)7891);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)7895);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7897);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7893);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'i')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)237);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)236);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)297);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7883);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7881);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'e')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)233);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'ê')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)7871);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)7873);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)7877);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7879);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7875);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'ư')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)7913);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)7915);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)7919);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7921);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7917);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'u')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)250);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)249);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)361);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7909);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7911);
                                    i++;
                                    break;
                            }
                        }
                        else if (ch == 'y')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)253);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)7923);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)7929);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7927);
                                    i++;
                                    break;

                            }
                        }
                        else if (ch == 'ơ')
                        {
                            switch (array[i + 1])
                            {
                                case (char)769:
                                    stringBuilder.Append((char)7899);
                                    i++;
                                    break;
                                case (char)768:
                                    stringBuilder.Append((char)7901);
                                    i++;
                                    break;
                                case (char)771:
                                    stringBuilder.Append((char)7905);
                                    i++;
                                    break;
                                case (char)803:
                                    stringBuilder.Append((char)7909);
                                    i++;
                                    break;
                                case (char)777:
                                    stringBuilder.Append((char)7903);
                                    i++;
                                    break;
                            }
                        }
                        else
                        {
                            // special cases
                            if (ch == 'I' && array[i + 1] == 769)
                            {
                                stringBuilder.Append((char)205);
                                i++;
                            }
                            if (ch == 'Â' && array[i + 1] == 769)
                            {
                                stringBuilder.Append((char)7844);
                                i++;
                            }
                            if (ch == 'A' && array[i + 1] == 769)
                            {
                                stringBuilder.Append((char)193);
                                i++;
                            }
                        }
                    }
                    else
                    {
                        stringBuilder.Append(ch);
                    }
                }
            }

            return stringBuilder.ToString();
        }

        private static Boolean IsSpecialCharacter(char ch)
        {
            return (ch == 768 || ch == 769 || ch == 803 || ch == 771 || ch == 777);
        }
    }
}
