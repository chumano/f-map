using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Test
/// </summary>
public class Test
{
    Service.WMS.WMS wms = new Service.WMS.WMS("http://localhost:8080/geoserver/wms?");
    Service.WFS.WFS wfs = new Service.WFS.WFS("http://localhost:8080/geoserver/wms?");

	public Test()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string run()
    {
        #region WMS
        //----GetCap-----
        //Service.WMS.CapRequest req = new Service.WMS.CapRequest();
        //return wms.GetCapabilities(req);

        //-----GetFeature-----106.682, 10.753, 106.714, 10.797 
        Service.WMS.FeatureInfoRequest req = new Service.WMS.FeatureInfoRequest("topp:states", new BoundingBox(-130, 24, -66, 50),
                                                        550, 250, 170, 160);
        //Service.WMS.FeatureInfoRequest req1 = new Service.WMS.FeatureInfoRequest("sde:Quan1_RG", new BoundingBox(106.682, 10.753, 106.714, 10.797),
        //                                                971, 634 , 547,234);
        return wms.GetFeatrueInfo(req);
        #endregion

        #region WFS
        //----GetCap-----
        //Service.WFS.CapRequest req = new Service.WFS.CapRequest();
        //return wfs.GetCapabilities(req);

        #endregion
    }

}