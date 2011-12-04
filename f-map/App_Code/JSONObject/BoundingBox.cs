using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BoundingBox
/// </summary>
public class BoundingBox
{
    //<BoundingBox CRS="EPSG:32633" minx="347649.93086859107" miny="5176214.082539256" maxx="370725.976428591" maxy="5196961.352859256"/>
	public double MinX;
    public double MinY;
    public double MaxX;
    public double MaxY;

	public BoundingBox(double minx, double miny, double maxx, double maxy)
	{
        MinX = minx;
        MinY = miny;
        MaxX = maxx;
        MaxY = maxy;
	}

    public BoundingBox(string bbox)
    {
        string []str = bbox.Split(',');
        MinX = Convert.ToDouble(str[0]);
        MinY = Convert.ToDouble(str[1]);
        MaxX = Convert.ToDouble(str[2]);
        MaxY = Convert.ToDouble(str[3]);
    }

    public override string ToString()
    {
        return MinX + "," + MinY + "," + MaxX + "," + MaxY;
    }
}