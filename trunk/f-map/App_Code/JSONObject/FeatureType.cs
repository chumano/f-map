using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FeatureType
/// </summary>
public class FeatureType
{
	public string Layer;
    public string SRS;

    public GeoBoundingBox GeoBBox;
	public FeatureType(string name, string srs, GeoBoundingBox geoBox)
	{
        Layer = name;
        SRS = srs;
        GeoBBox = geoBox;
	}
}