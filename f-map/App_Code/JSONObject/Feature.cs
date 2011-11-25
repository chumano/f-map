using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Feature
/// </summary>
public class Feature
{
    public string Layer;
    public string FID;

    public Data []Attrs;
	public Feature(string name, string fid, List<Data> attrs)
	{
        Layer = name;
        FID = fid;
        Attrs = attrs.ToArray<Data>();
	}
}