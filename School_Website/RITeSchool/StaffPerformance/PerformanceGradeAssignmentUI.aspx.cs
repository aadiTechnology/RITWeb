using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using StaffPerformanceEntity;
using System.Threading;
using SchoolEntities;

/// <summary>
/// This class is used to select and save data frelated to staff performance grade assignment.
/// </summary>
public partial class PerformanceGradeAssignmentUI : SchoolBase
{
    #region Constant(s)

    private const string S_SELECT = "Select";
    private const string S_INVITEE = "Invitee"; 

    #endregion
    
    #region Data Member(s)

    private ReportingConfigurationBL moReportingConfigurationBL;
   
    #endregion

    #region Event(s)


    /// <summary>
    /// This event is used to fill up year, skill combo boxes and fill parameter list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moReportingConfigurationBL = new ReportingConfigurationBL(miSchoolId,miUserId);
            if (!IsPostBack)
            {
                SetJavaScriptAttributes();
                FillYearCombobox();
                FillReportingUserListView();
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to bind data of selected year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillReportingUserListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to select and redirect to staff performanced evaluation screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPerformanceGradeAssignment_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == Constants.S_COMMAND_SELECT)
            {
                int iUserId = Convert.ToInt32(lstvwPerformanceGradeAssignment.DataKeys[e.Item.DisplayIndex]["UserId"]);
                string sQueryString = string.Format("UserId={0}&Year={1}&Status={2}", iUserId, cmbYear.SelectedValue.ToInt(),(optSubmitted.Checked?1:2));
                string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                string sEncryptedQueryString = string.Format("../StaffPerformance/PerformanceEvaluationUI.aspx?{0}", sEncrypt);
                Response.Redirect(sEncryptedQueryString,true);
            }
            else if (e.CommandName == S_INVITEE)
            {                
                hidReportingUserId.Value = lstvwPerformanceGradeAssignment.DataKeys[e.Item.DisplayIndex]["UserId"].ToString();
                List<PerformanceReportingConfig> lstInvitee = moReportingConfigurationBL.GetAllInviteeOfGivenUser(hidReportingUserId.Value.ToInt(), cmbYear.SelectedValue.ToInt());
                lstvwInvitee.DataSource = lstInvitee;
                lstvwInvitee.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "OpenPopup", "OpenPopup()", true);
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event im used to hide or show invitee link on listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPerformanceGradeAssignment_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                bool bIsSupervisor = Convert.ToBoolean(lstvwPerformanceGradeAssignment.DataKeys[e.Item.DisplayIndex]["IsSupervisor"]);
                if (!bIsSupervisor)
                {
                    ImageButton oimgBtnInvitee = e.Item.FindControl("imgBtnInvitee") as ImageButton;
                    oimgBtnInvitee.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save invitee of given user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkSelect;
            StringBuilder sbUserIds = new StringBuilder();
            string sUserIds = string.Empty;

            foreach(ListViewDataItem oCurrentItem in lstvwInvitee.Items)
            {               
                chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                if (chkSelect.Checked)
                    sbUserIds = sbUserIds.Append("," + lstvwInvitee.DataKeys[oCurrentItem.DisplayIndex]["UserId"].ToString());
            }

            if (sbUserIds.ToString().StartsWith(","))
                sUserIds = sbUserIds.ToString().Substring(1);

            moReportingConfigurationBL.SendRequestToInvitee(sUserIds, hidReportingUserId.Value.ToInt(), cmbYear.SelectedValue.ToInt());
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill staff list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optSubmitted_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillReportingUserListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill staff list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optPending_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillReportingUserListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill up year combo box.
    /// </summary>
    private void FillYearCombobox()
    {   
        List<AcademicYear> lstYears = SchoolWiseAcademicYearMasterBL.GetAllYears(miSchoolId);
        ListSource.FillDropDownList(lstYears, cmbYear, "Year", "Id", string.Empty);        
        cmbYear.SelectedValue = QueryString["Year"] == null ? DateTime.Now.Year.ToString() : QueryString["Year"];
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnClosePopUp });
        valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        if (QueryString["Status"] == null)
            optPending.Checked = true;
        else
        {
            if (QueryString["Status"].ToString() == Constants.S_ONE)
                optSubmitted.Checked = true;
            else
                optPending.Checked = true;
        }
    }

    /// <summary>
    /// this method is used to fill reporting config listview.
    /// </summary>
    private void FillReportingUserListView()
    {
        List<PerformanceReportingConfig> lstReportingUserList = moReportingConfigurationBL.GetAllReportingConfigs(miUserId, cmbYear.SelectedValue.ToInt(), optPending.Checked);
        lstvwPerformanceGradeAssignment.DataSource = lstReportingUserList;
        lstvwPerformanceGradeAssignment.DataBind();
    }

    #endregion
}