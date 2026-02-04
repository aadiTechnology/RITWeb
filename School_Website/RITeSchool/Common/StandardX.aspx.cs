using System;

public partial class RITeSchool_Common_StandardX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lnkWorksheetI.Attributes.Add("onclick", "window.open('../downloads/Syllabus/Primary/syllabus of W-I  UT-I for  X.pdf','_new'); return false;");
        lnkStdTermI.Attributes.Add("onclick", "window.open('../downloads/Syllabus/Primary/syllabus of First term X.pdf','_new'); return false;");
    }
}
