using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MapInfo
/// </summary>
public class MapInfo
{
	public MapInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public Layer[] layers { get; set; }
    public Bound bound { get; set; }
}