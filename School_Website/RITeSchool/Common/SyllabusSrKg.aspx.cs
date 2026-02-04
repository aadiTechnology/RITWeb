using System;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class RITeSchool_Syllabus_SyllabusSrKg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lnkSeniorKGJune.Attributes.Add("onclick", "window.open('../downloads/Syllabus/SeniorKG/Sr.KGJuneAndJuly2011.pdf','_new'); return false;");
            lnkSeniorKGAugSept.Attributes.Add("onclick", "window.open('../downloads/Syllabus/SeniorKG/SrKGAugAndSep.pdf','_new'); return false;");            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}