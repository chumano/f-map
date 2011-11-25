using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GeoBoudingBox
/// </summary>
public class GeoBoundingBox
{
    //<westBoundLongitude>12.999446822650462</westBoundLongitude>
    //<eastBoundLongitude>13.308182612644663</eastBoundLongitude>
    //<southBoundLatitude>46.722110379286</southBoundLatitude>
    //<northBoundLatitude>46.91359611878293</northBoundLatitude>
    public double West;
    public double East;
    public double South;
    public double North;

	public GeoBoundingBox(double w, double e, double s, double n)
	{
        West = w;
        East = e;
        South = s;
        North = n;
	}

    public GeoBoundingBox(string bbox)
    {
        string[] str = bbox.Split(',');
        West = Convert.ToDouble(str[0]);
        East = Convert.ToDouble(str[1]);
        South = Convert.ToDouble(str[2]);
        North = Convert.ToDouble(str[3]);
    }
}