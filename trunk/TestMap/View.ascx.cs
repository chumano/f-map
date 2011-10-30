//
// DotNetNuke - http://www.dotnetnuke.com
// Copyright (c) 2002-2005
// by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//
using DotNetNuke;
using DotNetNuke.Security.Roles;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using System.Web;

namespace DotNetNuke.Modules
{
    public partial class AJAXToolkit : DotNetNuke.Entities.Modules.PortalModuleBase
    {

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Determine if AJAX is installed
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
                // Create a reference to the Script Manager
                ScriptManager objScriptManager = ScriptManager.GetCurrent(this.Page);
                // Add a reference to the web service
                ServiceReference objServiceReference = new ServiceReference();
                objServiceReference.Path = @"~/DesktopModules/TestMap/WebService.asmx";
                objScriptManager.Services.Add(objServiceReference);

                ScriptReference objScriptReference = new ScriptReference();
                objScriptReference.Path = @"~/DesktopModules/TestMap/CallWebServiceMethods.js";
                objScriptManager.Scripts.Add(objScriptReference);

                objScriptReference = new ScriptReference();
                objScriptReference.Path = @"~/DesktopModules/TestMap/OpenLayers.js";
                objScriptManager.Scripts.Add(objScriptReference);

                objScriptReference = new ScriptReference();
                objScriptReference.Path = @"~/DesktopModules/TestMap/MyJScript.js";
                objScriptManager.Scripts.Add(objScriptReference);

                objScriptReference = new ScriptReference();
                objScriptReference.Path = @"~/DesktopModules/TestMap/Map.js";
                objScriptManager.Scripts.Add(objScriptReference);
            }

        }

    }
}

