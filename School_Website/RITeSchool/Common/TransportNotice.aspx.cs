using System;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class TransportNotice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lnkAnnextureA.Attributes.Add("onclick", "window.open('../downloads/Transport/ANNEXURE - A  -instructions for bus contractor, drivers and attendants.pdf','_new'); return false;");
            lnkAnnextureB.Attributes.Add("onclick", "window.open('../downloads/Transport/Annexure - B - Bus Requirements.pdf','_new'); return false;");
            lnkAnnextureC.Attributes.Add("onclick", "window.open('../downloads/Transport/ANNEXURE - C - Declaration by Parent_V2.0.pdf','_new'); return false;");
            lnkAnnextureD.Attributes.Add("onclick", "window.open('../downloads/Transport/Annexure - D - Detailed Routes with Rates.pdf','_new'); return false;");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}
