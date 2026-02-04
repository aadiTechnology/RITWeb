using System;

public partial class RITeSchool_Common_StandardIX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lnkWorksheetI.Attributes.Add("onclick", "window.open('../downloads/Syllabus/Primary/syllabus of W-I  UT-I for IX .pdf','_new'); return false;");
        lnkStdTermI.Attributes.Add("onclick", "window.open('../downloads/Syllabus/Primary/syllabus of First term IX.pdf','_new'); return false;");
    }
}