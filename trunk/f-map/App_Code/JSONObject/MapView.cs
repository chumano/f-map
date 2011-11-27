using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MapView
/// </summary>
public class MapView
{
    public int ID;
    public string Name;
    public string NoName;
	public MapView(int id, string name, string nm)
	{
        ID = id;
        Name = name;
        NoName = nm;
	}
}