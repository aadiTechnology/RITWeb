using System;
using System.Reflection;
using BusinessLogic.Exceptions;
using Utility;

public partial class StudentAnnualResultPrint : StudentResult
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            base.SetpanelMember(Containers);
            checkModeAndSetcontrols();
        }
        catch (Exception ex)
        {
            lblErrorsMsg.Visible = true;
            lblErrorsMsg.Text = ex.Message;
            tblMsg.Visible = true;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set print mode if this screen is going to open for printing mode.
    /// </summary>
    /// <returns></returns>
    private Boolean checkModeAndSetcontrols()
    {
		if (QueryString.Count > 0 && QueryString["mode"] != null && QueryString["mode"] == "print")
        {
            S_CSS_PRINT_PREFIX = "P";
            menmPagemode = Constants.PageMode.Print;
            int iStudentId = 0;
			int iStdDivId = 0;
				if (!QueryString["StandardDivisionId"].IsNullOrEmpty())
					iStdDivId = QueryString["StandardDivisionId"].ToInt();
			if (!QueryString["iStudId"].IsNullOrEmpty())
				iStudentId = QueryString["iStudId"].ToInt();
			base.ShowProgressSheet(iStdDivId, iStudentId);
            return false;
        }
        return true;
    }
}
