using System;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class RITeSchool_Syllabus_SyllabusNursery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lnkNurseryJune.Attributes.Add("onclick", "window.open('../downloads/Syllabus/Nursury/NurJuneAndJuly2011.pdf','_new'); return false;");
            lnkNurseryAugSep.Attributes.Add("onclick", "window.open('../downloads/Syllabus/Nursury/NurAugAndSep.pdf','_new'); return false;");            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}