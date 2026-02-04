using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class ResetTimeTableUI :SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        try
        {            
            SetJavaScriptAttributes();
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                RefreshValues();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button>{ btnBack, btnReset});
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        char cResetSubjectTeacher=chkResetSubjectTeacher.Checked?'Y':'N';
        char cResetClassTeacher = chkResetClassTeacher.Checked?'Y':'N';
        if(chkResetTimeTable.Checked)
            SchoolTimeTableMasterBL.ResetTimetable(miSchoolId, miAcademicYearId, 0, 0);
        SuperAdminBL.Reset(miSchoolId, miAcademicYearId, cResetSubjectTeacher, cResetClassTeacher);
        lblReset.Visible = true;
        lblReset.Text = Resources.LocalizedResources.MsgResetTimeTable2;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master;
            oSuperAdminMasterPage.RedirectToNextPage("~/SuperAdmin/ScreensUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #region " Private Methods "

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidMsgResetTimeTable1.Value = Resources.LocalizedResources.MsgResetTimeTable1;
    }

    #endregion " Private Methods "
}
