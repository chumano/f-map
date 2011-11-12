using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Config
/// </summary>
public class Config
{
    public static string URL_GET_INFO = "http://localhost:8080/geoserver/wms?REQUEST=GetFeatureInfo&BBOX={0}&SERVICE=WMS&VERSION=1.1.1&X={1}&Y={2}&INFO_FORMAT=text/plain&QUERY_LAYERS={3}&FEATURE_COUNT=50&Layers={4}&WIDTH={5}&HEIGHT={6}&srs=EPSG:4326";

	public Config()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}