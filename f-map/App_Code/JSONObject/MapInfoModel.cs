using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MapInfo
/// </summary>
public class MapInfoModel
{
	public MapInfoModel()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public LayerModel[] layers { get; set; }
    public BoundingBox bound { get; set; }
}
