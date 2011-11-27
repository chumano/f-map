using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CheckAddressResult
/// </summary>
public class CheckAddressResult
{
    public bool Found;
    public Point[] Points;
	public CheckAddressResult(bool f, List<Point> l)
	{
        Found = f;
        Points = l.ToArray();
	}
}

public class Point{
    public double X,Y;

    public Point(double x, double y)
    {
        X= x; Y= y;
    }
}