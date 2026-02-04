using System;

public partial class RITeSchool_Common_StandardVI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lnkStdTermI.Attributes.Add("onclick", "window.open('../downloads/Syllabus/Primary/SyllabusVII.pdf','_new'); return false;");
    }
}
