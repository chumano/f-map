using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Data.OleDb;

namespace SmartLib
{
	/// <summary>
	/// Summary description for Helper.
	/// </summary>
	public class Helper
	{
		public Helper()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        public static string Convert_NumtoText(string Sonhap)
        {
            string result = "";
            switch (Sonhap.Length)
            {
                case 0: result = "";
                    break;
                case 1: result = Sodonvi(Sonhap);
                    break;
                case 2:
                    {

                        result = Sohangchuc(Sonhap);

                    }
                    break;
                case 3:
                    {
                        result = Sohangtram(Sonhap);
                    }
                    break;
                case 4:
                    {
                        result = Sohangngan(Sonhap);

                    }
                    break;
                case 5:
                    {
                        if (Sonhap.Substring(2, 3).Equals("000"))
                        {
                            result = Sohangchuc(Sonhap.Substring(0, 2)) + " ngàn ";
                        }
                        else
                            result = Sohangchuc(Sonhap.Substring(0, 2)) + " ngàn " + Sohangtram(Sonhap.Substring(2, 3));
                    }
                    break;
                case 6:
                    {
                        if (Sonhap.Substring(3, 3).Equals("000"))
                        {
                            result = Sohangtram(Sonhap.Substring(0, 3)) + " ngàn ";
                        }
                        else
                            result = Sohangtram(Sonhap.Substring(0, 3)) + " ngàn " + Sohangtram(Sonhap.Substring(3, 3));

                    }
                    break;
                case 7:
                    {
                        //if (Sonhap.Substring(3, 4).Equals("0000"))
                        //{
                        // result = Sodonvi(Sonhap.Substring(0, 1)) + " triệu ";
                        //}
                        //else
                        result = Sodonvi(Sonhap.Substring(0, 1)) + " triệu " + Sohangtram(Sonhap.Substring(1, 3)) + " ngàn " + Sohangtram(Sonhap.Substring(4, 3));
                    }
                    break;
                case 8:
                    {
                        if (Sonhap.Substring(2, 6).Equals("000000"))
                        {
                            result = Sohangchuc(Sonhap.Substring(0, 2)) + " triệu ";
                        }
                        else
                            result = Sohangchuc(Sonhap.Substring(0, 2)) + " triệu " + Sohangtram(Sonhap.Substring(2, 3)) +
                            " ngàn " + Sohangtram(Sonhap.Substring(5, 3));
                    }
                    break;
                case 9:
                    {
                        if (Sonhap.Substring(3, 6).Equals("000000"))
                        {
                            result = Sohangtram(Sonhap.Substring(0, 3)) + " triệu ";
                        }
                        else
                            result = Sohangtram(Sonhap.Substring(0, 3)) + " triệu " + Sohangtram(Sonhap.Substring(3, 3)) +
                            " ngàn " + Sohangtram(Sonhap.Substring(6, 3));
                    }
                    break;
                case 10:
                    {
                        if (Sonhap.Substring(1, 9).Equals("000000000"))
                        {
                            result = Sodonvi(Sonhap.Substring(0, 1)) + " tỷ ";
                        }
                        else
                            result = Sodonvi(Sonhap.Substring(0, 1)) + " tỷ " + Sohangtram(Sonhap.Substring(1, 3)) + " triệu " + Sohangtram(Sonhap.Substring(4, 3)) +
                            " ngàn " + Sohangtram(Sonhap.Substring(7, 3));
                    }
                    break;
                case 11:
                    {
                        if (Sonhap.Substring(2, 9).Equals("000000000"))
                        {
                            result = Sohangchuc(Sonhap.Substring(0, 2)) + " tỷ ";
                        }
                        else
                            result = Sohangchuc(Sonhap.Substring(0, 2)) + " tỷ " + Sohangtram(Sonhap.Substring(2, 3)) + " triệu " + Sohangtram(Sonhap.Substring(5, 3)) +
                            " ngàn " + Sohangtram(Sonhap.Substring(6, 3));
                    }
                    break;
                case 12:
                    {
                        if (Sonhap.Substring(3, 9).Equals("000000000"))
                        {
                            result = Sohangtram(Sonhap.Substring(0, 3)) + " tỷ ";
                        }
                        else
                            result = Sohangtram(Sonhap.Substring(0, 3)) + " tỷ " + Sohangtram(Sonhap.Substring(3, 3)) + " triệu " + Sohangtram(Sonhap.Substring(6, 3)) +
                            " ngàn " + Sohangtram(Sonhap.Substring(9, 3));
                    }
                    break;

            }
            return result.Replace(" ", " ").Replace("mươi một", "mươi mốt").Replace("mươi năm", "mươi lăm");
        }
        public static string ChuanHoa(string st)
        {
            string st1 = "";
            return st1.Trim();
        }
        public static string Sohangngan(string So)
        {
            string result = "";
            if (So.Equals("1000"))
                result = " một ngàn ";
            else
            {
                result = Sodonvi(So.Substring(0, 1)) + " ngàn ";
                if (So.Substring(1, 1).Equals("0"))
                {

                    if (So.Substring(2, 1).Equals("0"))
                    {
                        if (!So.Substring(3, 1).Equals("0"))
                        {
                            result += Sodonvi(So.Substring(1, 1)) + " trăm ";
                            result += " lẻ " + Sodonvi(So.Substring(So.Length - 1));
                        }
                    }
                    else
                    {
                        result += Sodonvi(So.Substring(1, 1)) + " trăm ";
                        result += Sohangchuc(So.Substring(2, 2));
                    }
                }
                else
                    result += Sohangtram(So.Substring(1, 3));
            }
            return result;


        }
        public static string Sohangtram(string So)
        {
            string result = "";
            if (So.Equals("100"))
                result = " một trăm ";
            else
            {
                result += Sodonvi(So.Substring(0, 1)) + " trăm ";
                if (So.Substring(1, 1).Equals("0"))
                {
                    if (!So.Substring(2, 1).Equals("0"))
                        result += " lẻ " + Sodonvi(So.Substring(2, 1));
                }
                else
                    result += Sohangchuc(So.Substring(1, 2));
            }
            return result;

        }
        public static string Sohangchuc(string So)
        {
            string result = "";
            if (So.Equals("10"))
                result = " mười ";
            else
            {
                if (So.Substring(0, 1).Equals("1"))
                    result += " mười " + Sodonvi(So.Substring(1, 1));
                else
                {
                    result += Sodonvi(So.Substring(0, 1)) + " mươi ";
                    if (!So.Substring(1, 1).Equals("0"))
                        result += Sodonvi(So.Substring(1, 1));


                }
            }
            return result;
        }
        public static string Sodonvi(string So)
        {
            string result = "";
            switch (So)
            {
                case "0": result += " không ";
                    break;
                case "1": result += " một ";
                    break;
                case "2": result += " hai ";
                    break;
                case "3": result += " ba ";
                    break;
                case "4": result += " bốn ";
                    break;
                case "5": result += " năm ";
                    break;
                case "6": result += " sáu ";
                    break;
                case "7": result += " bảy ";
                    break;
                case "8": result += " tám ";
                    break;
                case "9": result += " chín ";
                    break;

            }
            return result;
        }
       
        public static void insertLog(string username, string thaotac, DateTime thoidiem, string tablename, int rowId, string functionfile)
        {
            string insertStr = "Insert into tbl_DBLog(Username, ThaoTac, ThoiDiem, TableName, RowId, FunctionFile) VALUES(N'" + username + "', N'" + thaotac + "', '" + String.Format("{0:G}", thoidiem) + "', N'" + tablename + "', " + rowId + ",N'" + functionfile + "')";
            Helper.ExecuteNonQuery(insertStr, Helper.getConnectionString());
        }
       
        public static DataTable GetDataTable(string query)
        {

            OleDbConnection MyOleDbConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            OleDbDataAdapter MyOleDbDataAdapter = new OleDbDataAdapter();
            MyOleDbDataAdapter.SelectCommand = new OleDbCommand(query, MyOleDbConnection);

            DataTable myDataTable = new DataTable();

            MyOleDbConnection.Open(); try {MyOleDbDataAdapter.Fill(myDataTable); }
            finally { MyOleDbConnection.Close(); }

            return myDataTable;
        }
        public static DataTable GetDataTable2(string query)
        {

            OleDbConnection MyOleDbConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString2"].ConnectionString);
            OleDbDataAdapter MyOleDbDataAdapter = new OleDbDataAdapter();
            MyOleDbDataAdapter.SelectCommand = new OleDbCommand(query, MyOleDbConnection);

            DataTable myDataTable = new DataTable();

            MyOleDbConnection.Open(); try { MyOleDbDataAdapter.Fill(myDataTable); }
            finally { MyOleDbConnection.Close(); }

            return myDataTable;
        }
        public static bool CheckRight(string tableName, string thaotac, string username)
        {
            return true;
            //string sqlStr = "Select " + thaotac + " From vw_PhanQuyen WHERE Username=N'" + username + "' AND Ten=N'" + tableName+"'";
            //DataTable myData = Helper.GetDataTable(sqlStr);
            //if (myData.Rows.Count > 0)
            //    if ((bool)myData.Rows[0][0])
            //        return true;
            //return false;
        }
        public static string getUploadLogoFolder()
        {
            return ConfigurationSettings.AppSettings["Upload"];
        }
		public static string getConfigValue(string name)
		{
			return ConfigurationSettings.AppSettings[name];
		}

		public static void setConfigValue(string name, string value)
		{
			if (ConfigurationSettings.AppSettings[name]!=null)
				ConfigurationSettings.AppSettings[name]=value;
		}
        public static bool checkExist(string TableName, string whereStr, out int errorCode)
        {
            string strSql = "select * "
                            + " from " + TableName
                            + " where " + whereStr;
            SqlConnection myCon = new SqlConnection(getConnectionString());
            SqlCommand myCmd = new SqlCommand(strSql, myCon);
            SqlDataReader drd;
            try
            {
                myCon.Open();
                drd = myCmd.ExecuteReader();
                errorCode = 0;
                if (drd.HasRows == true)
                {
                    drd.Close();
                    myCmd.Connection.Close();
                    myCon.Close();
                    return true;
                }
                drd.Close();
            }
            catch (Exception e)
            {
                e.ToString();
                errorCode = 1;
            }
            myCmd.Connection.Close();
            myCon.Close();
            return false;
        }
		public static string getConnectionString()
		{
			return getConfigValue("ConnectionString");
		}

		/**
		 * Des: Thuc thi cau truy van va tra ve doi tuong chua result. Tu dong dong cau noi.
		 *		ErrorCode duoc gan bang 1 neu viec kiem tra thanh cong, nguoc lai errorCode se co
		 *      gia tri bang 0.
		 */
		public static SqlDataReader executeQuery(string strSql, string conStr, out int errorCode) 
		{
			SqlDataReader result = null;
			SqlConnection myCon = new SqlConnection(conStr);
			SqlCommand myCmd = new SqlCommand(strSql, myCon);
			try
			{
			myCon.Open();
			result = myCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
			errorCode = 1;
			} 
			catch (Exception e)
			{
				errorCode = 0;
			}
			return result;
		}

		/**
		 * Des: Thuc thi cau truy van va tra ve doi tuong chua result.
		 *		ErrorCode duoc gan bang 1 neu viec kiem tra thanh cong, nguoc lai errorCode se co
		 *      gia tri bang 0.
		 */
		public static SqlDataReader executeQueryNoClose(string strSql, string conStr, out int errorCode) 
		{
			SqlDataReader result = null;
			SqlConnection myCon = new SqlConnection(conStr);
			SqlCommand myCmd = new SqlCommand(strSql, myCon);
            try
            {
                myCon.Open();
                result = myCmd.ExecuteReader();
                errorCode = 1;
            }
            catch (Exception e)
            {
                errorCode = 0;
            }
            finally
            {

            }
			return result;
		}

		/**
		 * Des: Thuc thi cau truy van dang khong tra ve ket qua (update,...).
		 *		Tra ve true neu thuc hien thanh cong, nguoc lai tra ve false.
		 */
		public static bool ExecuteNonQuery(string strSql, string conStr)
		{
            SqlConnection myCon = null;
            SqlCommand myCmd = null;
            try
            {
                myCon = new SqlConnection(conStr);
                myCmd = new SqlCommand(strSql, myCon);
                myCon.Open();
                myCmd.ExecuteNonQuery();
               
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                myCmd.Connection.Close();
                myCon.Close();
            }
            return true;
		}

		/**
		 * Des: Thuc hien danh sach cac lenh khong truy van theo thu tu.
		 *		Tra ve true neu tat ca cac lenh deu thanh cong, nguoc lai tra ve false
		 *		va cac lenh se duoc rollback neu co the.
		 */
		public static bool doTransactionNonQuery(string[] list_of_sql, string conStr)
		{
			SqlConnection myCon = new SqlConnection(conStr);
			myCon.Open();

			// Start a local transaction.
			SqlTransaction myTrans = myCon.BeginTransaction();

			// Enlist the command in the current transaction.
			SqlCommand myCmd = myCon.CreateCommand();
			myCmd.Transaction = myTrans;

			try
			{
				for (int i=0; i<list_of_sql.Length; i++)
				{
					myCmd.CommandText = list_of_sql[i];
					myCmd.ExecuteNonQuery();
				}
				myTrans.Commit();
				return true;
			}
			catch(Exception e)
			{
				try
				{
					myTrans.Rollback();
				}
				catch (SqlException ex)
				{
				}
			}
			finally
			{
				myCon.Close();
			}
			return false;
		}

		/**
		 * Des: Tra ve so hang ket qua thoa cau truy van.
		 *      Neu truy van thuc hien thanh cong errorCode=1, nguoc lai = 0.
		 */
		public static int executeQuery(string sql, out int errorCode)
		{
			int result = 0;
			SqlConnection myCon = new SqlConnection(getConnectionString());
			SqlCommand myCmd = new SqlCommand(sql, myCon);
			try
			{
				myCon.Open();
				result = myCmd.ExecuteNonQuery();
				errorCode = 1;
			} 
			catch (Exception e) 
			{
				errorCode=0;
			}

			myCmd.Connection.Close();
			myCon.Close();
			return result;
		}
	
		/**
		 * Des: Tra ve doi tuong DataSet chua du lieu.
		 *      Neu truy van thuc hien thanh cong errorCode=1, nguoc lai = 0.
		 */
		public static DataSet getDataSet(string strSql, string conStr, out int errorCode)
		{
			SqlConnection myCon = new SqlConnection(conStr);
			SqlCommand myCmd = new SqlCommand(strSql, myCon);
			DataSet myDataSet = new DataSet();
			try 
			{
				myCon.Open();
				System.Data.SqlClient.SqlDataAdapter myAdapter = new System.Data.SqlClient.SqlDataAdapter(strSql, myCon);
				myAdapter.Fill(myDataSet, strSql);
				errorCode = 1;
			} 
			catch (Exception e)
			{
				errorCode=0;
				return null;
			}
			return myDataSet;
		}

		/**
		 * Des: Kiem tra xem gia tri val co ton tai trong cot colname cua bang tablename hay khong.
		 *      Neu ton tai ham tra ve true, nguoc lai tra ve false.
		 *      ErrorCode duoc gan bang 1 neu viec kiem tra thanh cong, nguoc lai errorCode se co
		 *      gia tri bang 0.
		 */
		public static bool checkValue(string val, string colname, string tablename,out int errorCode)
		{
			string strSql = "select " + colname 
				+ " from " + tablename 
				+ " where " + colname + "='" 
				+ val.Replace("'","''") + "';";

			if (executeQuery(strSql, out errorCode)>0 && errorCode==1) return true;
			return false;
		}

		/**
		 * Des: This is a compile with checkValue and executeQuery. 
		 *      Replace later.
		 */
		public static bool checkExist(string val, string colname, string tablename,out int errorCode)
		{
			string strSql = "select " + colname 
				+ " from " + tablename 
				+ " where " + colname + "=N'" 
				+ val.Replace("'","''") + "';";

			string myConStr = ConfigurationSettings.AppSettings["connectionstring"];
			SqlConnection myCon = new SqlConnection(myConStr);
			SqlCommand myCmd = new SqlCommand(strSql, myCon);
			SqlDataReader drd;
            try
            {
                myCon.Open();
                errorCode = 1;
                drd = myCmd.ExecuteReader();
                if (drd.HasRows == true) return true;
            }
            catch (Exception e)
            {
                errorCode = 0;
            }
            finally
            {
                myCmd.Connection.Close();
                myCon.Close();
            }
			return false;
		}

		/**
		 * Des: This is a compile with checkValue and executeQuery. 
		 *      Replace later.
		 */
		public static bool checkExist(int val, string colname, string tablename,out int errorCode)
		{
			string strSql = "select " + colname 
				+ " from " + tablename 
				+ " where " + colname + "=" + val + ";";

			string myConStr = ConfigurationSettings.AppSettings["connectionstring"];
			SqlConnection myCon = new SqlConnection(myConStr);
			SqlCommand myCmd = new SqlCommand(strSql, myCon);
			SqlDataReader drd;
            try
            {
                myCon.Open();
                errorCode = 1;
                drd = myCmd.ExecuteReader();
                if (drd.HasRows == true) return true;
            }
            catch (Exception e)
            {
                errorCode = 0;
            }
            finally
            {
                myCmd.Connection.Close();
                myCon.Close();
            }
			return false;
		}

		public static void executeStoreProcedure(string procedure_name, string[] args)
		{
			string myConStr = ConfigurationSettings.AppSettings["connectionstring"];
			SqlConnection myCon = new SqlConnection(myConStr);
			SqlCommand myCmd = new SqlCommand(procedure_name, myCon);
			myCmd.CommandType = System.Data.CommandType.StoredProcedure;

			myCmd.Parameters.Add("@TestParam", System.Data.SqlDbType.Int, 10);
			myCmd.Parameters["@TestParam"].Value = "Testing";

			myCon.Open();
			myCmd.ExecuteNonQuery();
		}

		// valList: separate by comma
		public static bool deleteAll(string valList, string colname, string tablename)
		{
			string strSql = "delete " + tablename
				+ " where " + colname + " in ('" + valList.Replace("'","''").Replace(",","','") + "')";
			return ExecuteNonQuery(strSql, getConnectionString());
		}

		public static bool deleteAll(ArrayList valList, string colname, string tablename)
		{
			// make list
			string list = "";
			for (int i=0; i<valList.Count; i++)
				list += "," + valList[i];
			if (!list.Equals("")) 
			{
				list = list.Substring(1);
				string strSql = "delete " + tablename
					+ " where " + colname + " in ('" + list.Replace("'","''").Replace(",","','") + "')";
				return ExecuteNonQuery(strSql, getConnectionString());
			}
			return false;
		}

        public static bool deleteAllData(string tablename, string strFilter)
        {
            string strSql = "delete from " + tablename + (strFilter.Equals("")? "": " WHERE " + strFilter);
            return ExecuteNonQuery(strSql, getConnectionString());
        }
        
        public static String GetName(String idName, String idValue, String name, String tableName)
        {
            int err = 0;
            String sqlStr = "Select " + name + " From " + tableName + " Where " + idName + "='" + idValue+"'";
            SqlDataReader drd = Helper.executeQuery(sqlStr, Helper.getConnectionString(), out err);
            if (drd != null && drd.Read())
            {
                return (drd["" + name] == DBNull.Value ? "" : (String)drd["" + name]);
            }
            return "";
        }
        public static string getFilename(System.Web.UI.HtmlControls.HtmlInputFile ctrlname)
        {
            string s1;
            int pos;
            s1 = ctrlname.PostedFile.FileName;
            if (s1 == "")
                return "";
            pos = s1.LastIndexOf("\\") + 1;
            string sChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int lenRandom = 20; //Random 1 chuoi voi len = 10
            Random rd = new Random(unchecked((int)DateTime.Now.Ticks));
            string fileName = string.Empty;
            for (int i = 0; i < lenRandom; i++)
                fileName += sChars[rd.Next(0, sChars.Length - 1)].ToString();

            s1 = s1.Substring(pos);
            s1 = s1.Replace(s1.Substring(0, s1.IndexOf(".")), fileName);
            return s1.Substring(pos);
        }
        public static void upload(System.Web.UI.HtmlControls.HtmlInputFile ctrlname, string dir_from_root, string filename, System.Web.HttpRequest request)
        {
            string path = request.ServerVariables["APPL_PHYSICAL_PATH"];
            path = path + dir_from_root + filename;
            ctrlname.PostedFile.SaveAs(path);
        }
	}
}
