using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class Program
    {
        private static Dictionary<char, char> dic = new Dictionary<char,char>();

        static void Main(string[] args)
        {
            int i = 0;
            char[] newChars = newValues.ToCharArray();
            foreach (char ch in oldValues.ToCharArray()) {
                dic.Add(ch, newChars[i++]);
            }

            //Console.WriteLine(convertName("khải"));
            //Console.ReadKey();
            //return;

            SqlConnection myConnection = new SqlConnection("user id=sde;" +
                                       "password=sde;server=NewAge\\SqlExpress;" +
                                       "Trusted_Connection=yes;" +
                                       "database=sde;");
            SqlConnection myConnection2 = new SqlConnection("user id=sde;" +
                                       "password=sde;server=NewAge\\SqlExpress;" +
                                       "Trusted_Connection=yes;" +
                                       "database=sde;");

            try
            {
                myConnection.Open();
                myConnection2.Open();

                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select * from sde.sde.Quan1", myConnection);
                myReader = myCommand.ExecuteReader();
                String oldName;
                int k = 0;
                while (myReader.Read())
                {
                    k++;
                    oldName = (myReader["TenConDuong"].ToString());

                    myCommand = new SqlCommand("update sde.sde.Quan1 set TenConDuong2 = '" + convertName(oldName) + "' where Shape = " + k,
                        myConnection2);
                    myCommand.ExecuteNonQuery();

                    //k++;
                    //oldName = (myReader["TenNha"].ToString());

                    //myCommand = new SqlCommand("update sde.sde.Quan1 set TenNha2 = '" + convertName(oldName) + "' where Shape = " + k,
                    //    myConnection2);
                    //myCommand.ExecuteNonQuery();
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

            Console.WriteLine("Finish");
            Console.ReadKey();
        }

        //private static String oldValues = "áàạảãíìịỉĩéèẹẻẽếềệểễùụủũắằặẳẵứừựửữớờợởỡốồộổỗđÁÀẠẢÃÍÌỊỈĨÉÈẸẺẼẾỀỆỂỄÙỤỦŨẮẰẶẲẴỨỪỰỬỮỚỜỢỞỠỐỒỘỔỖĐ";
        //private static String newValues = "aaaaaiiiiieeeeeeeeeeuuuuaaaaauuuuuoooooooooodAAAAAIIIIIEEEEEEEEEEUUUUAAAAAUUUUUOOOOOOOOOOD";
        private static String oldValues = "âăươêđôáàảãạấầẩẫậắằẳẵặứừửữựớờởỡợếềểễệốồổỗộíìĩịỉéèẻẽẹóòỏõọúùủũụ";
        private static String newValues = "aauoedoaaaaaaaaaaaaaaauuuuuoooooeeeeeoooooiiiiieeeeeooooouuuuu";
        private static String convertName(String oldName) {
            StringBuilder stringBuilder = new StringBuilder();
            oldName = oldName.ToLower();
            int len = oldName.Length;
            char[] array = oldName.ToCharArray();
            char ch;
            for (int i = 0; i < len; ++i) {
                ch = array[i];
                if (dic.ContainsKey(ch)) {
                    stringBuilder.Append(dic[ch]);
                } else if ( !(ch == 768 || ch == 769 || ch == 803 || ch == 771 || ch == 777) ) {
                    stringBuilder.Append(ch);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
