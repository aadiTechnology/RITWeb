using System;

public partial class RITeSchool_Common_SyllabusPrimary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lnkStdTerm.Attributes.Add("onclick", "window.open('../downloads/Syllabus/Primary/StdII_Term2.pdf','_new'); return false;");
    }
}
