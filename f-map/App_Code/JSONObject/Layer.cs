using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Layer
/// </summary>
public class Layer
{
    public string LayerName; //ten tuong trung
    public string GeoLayer;    //ten de lay du lieu- sde:NHA
    public string StyleName;

    public GeoBoundingBox GeoBox;
    public BoundingBox Box;

    public string CRS;
	public Layer(string lName, string layer, string styleName, string crs)
	{
        LayerName = lName;
        GeoLayer = layer;
        StyleName = styleName;
        CRS = crs;
	}

}