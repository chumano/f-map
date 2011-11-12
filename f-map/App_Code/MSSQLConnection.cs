using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

/// <summary>
/// Summary description for MSSQLConnection
/// </summary>
public class MSSQLConnection
{
    private string _dbName;
    private string _user;
    private string _pass;

    public System.String strConnection;//connection string

    public string DBName
    {
        get { return _dbName; }
        set
        {
            _dbName = value;
            updateConnection();
        }
    }

    public string UserName
    {
        get { return _user; }
        set
        {
            _user = value;
            updateConnection();
        }
    }

    public string Password
    {
        get { return _pass; }
        set
        {
            _pass = value;
            updateConnection();
        }
    }


    public MSSQLConnection(string dbName, string user, string pass)
    {
        _dbName = dbName;
        _user = user;
        _pass = pass;
        //Provider=System.Data.OleDb;Data Source=CHUNO\SQLEXPRESS;Password=371319855;User ID=sa;Initial Catalog=VIS
        strConnection = ""//@"Provider=System.Data.OleDb;"
                + @"Data Source=CHUNO\SQLEXPRESS;"
                + @"Password=" + pass + ";"
                + @"User ID=" + user + ";"
                + @"Initial Catalog=" + dbName;

        //strConnection = @"Provider=Microsoft.Jet.OLEDB.4.0;" +

        // @"Data Source=\dir\Mydb.mdb; User Id=dbUser; Password=pass";
    }

    protected void updateConnection()
    {
        strConnection = ""// @"Provider=SQLOLEDB;"
                + @"Data Source= CHUNO\SQLEXPRESS;"
                + @"Password=" + _pass + ";"
                + @"User ID=" + _user + ";"
                + @"Initial Catalog=" + _dbName;
    }


    //Used for database SELECT queries
    public DataTable Select(string selectQuery)
    {
        if (selectQuery.Length <= 1)
        {
            return null;
        }
        //create new connection
        OleDbConnection MyOleDbConnection = new OleDbConnection(@"Provider=SQLOLEDB;" + strConnection);
        OleDbDataAdapter MyOleDbDataAdapter = new OleDbDataAdapter();
        MyOleDbDataAdapter.SelectCommand = new OleDbCommand(selectQuery, MyOleDbConnection);

        DataTable myDataTable = new DataTable();

        MyOleDbConnection.Open(); try { MyOleDbDataAdapter.Fill(myDataTable); }
        finally { MyOleDbConnection.Close(); }

        return myDataTable;

    }//end Select

    //Used to INSERT, DELETE, UPDATE sql statments

    public void Modify(string mod)
    {

        SqlConnection myCon = new SqlConnection(strConnection);
        SqlCommand myCmd = new SqlCommand(mod, myCon);
        myCmd.CommandType = System.Data.CommandType.Text;

        try
        {
            myCon.Open();
            myCmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
        }
        finally
        {
            myCon.Close();
        }

    }//end Modify

}