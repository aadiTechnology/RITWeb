using System;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class RITeSchool_Syllabus_SyllabusJrKg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lnkJuniorKGJune.Attributes.Add("onclick", "window.open('../downloads/Syllabus/JuniorKG/Jr.KGuneAndJuly2011.pdf','_new'); return false;");
            lnkJuniorKGAugSept.Attributes.Add("onclick", "window.open('../downloads/Syllabus/JuniorKG/JrKGAugAndSEp.pdf','_new'); return false;");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}