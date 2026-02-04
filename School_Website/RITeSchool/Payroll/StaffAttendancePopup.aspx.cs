/* File Name - StaffAttendancePopup.aspx.cs
 * Created By - Sachin
 * Created Date - 10-Oct-2014
 * Class Description - This class is used to manage staff leaves.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using PayrollEntities;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Drawing;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Data.SqlClient;

public partial class StaffAttendancePopup : SchoolBase
{
    #region Data Member(s)

    private List<ConfiguredLeaves> mlstConfiguredLeaves;
    private StaffAttendanceBL moStaffAttendanceBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill staff groups combo box, fill staff leave details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStaffAttendanceBL = new StaffAttendanceBL();
            if (!IsPostBack)
            {
                FillStaffGroups();
                SetDefaultValues();
                FillStaffAttendanceDetails();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenP", "SetFieldState();", true);
                SetODQueryString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search staff according to selected staff group and given user name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
             DateTime dt = Convert.ToDateTime(txtDate.Text);
            lblDate.Text = dt.ToString(Constants.S_DATE_FORMAT);
            FillStaffAttendanceDetails();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenP", "SetFieldState();", true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtDate.Text);
            lblDate.Text = dt.ToString(Constants.S_DATE_FORMAT);
            FillStaffAttendanceDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cal_PaymentDate_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtDate.Text);
            lblDate.Text = dt.ToString(Constants.S_DATE_FORMAT);            
            FillStaffAttendanceDetails();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenP", "SetFieldState();", true);
            SetODQueryString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime dt = Convert.ToDateTime(txtDate.Text);
        lblDate.Text = dt.ToString(Constants.S_DATE_FORMAT);        
        FillStaffAttendanceDetails();
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenP", "SetFieldState();", true);
        SetODQueryString();
    }

    /// <summary>
    /// This event is used to update/set values for controls available in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DaywiseStaffAttendance oDaywiseStaffAttendance = e.Item.DataItem as DaywiseStaffAttendance;

                HiddenField hidLeaveBalance = e.Item.FindControl("hidLeaveBalance") as HiddenField;
                HiddenField hidLeaveDetails = e.Item.FindControl("hidLeaveDetails") as HiddenField;
                UpadteLeaveBalance(oDaywiseStaffAttendance, hidLeaveBalance, false);
                UpadteLeaveBalance(oDaywiseStaffAttendance, hidLeaveDetails, true);                

                Label lblStaffName = e.Item.FindControl("lblStaffName") as Label;
                Label lblDesignation = e.Item.FindControl("lblDesignation") as Label;
                if (lblStaffName != null && lblDesignation != null)
                {
                    // lblStaffName.Attributes.Add("onclick", "OpenPopup(" + e.Item.DisplayIndex + "); return false;");
                    lblStaffName.ToolTip = "Leave Balance - " + hidLeaveDetails.Value;
                    lblDesignation.ToolTip = "Leave Balance - " + hidLeaveDetails.Value;
                }

                DropDownList cmbLeave = e.Item.FindControl("cmbLeave") as DropDownList;
                if (cmbLeave != null)
                {
                    FillStaffLeaves(cmbLeave, "Present");
                    cmbLeave.Attributes.Add("onchange", "SetControlState(" + e.Item.DisplayIndex + ",1,0);");

                    string sColorCode = "White";
                    if (mlstConfiguredLeaves.Where(lv => lv.LeaveId == oDaywiseStaffAttendance.LeaveId).Any())
                        sColorCode = mlstConfiguredLeaves.Where(lv => lv.LeaveId == oDaywiseStaffAttendance.LeaveId).Select(lv => lv.ColorCode).FirstOrDefault();

                    cmbLeave.BackColor = Color.FromName(sColorCode);
                    cmbLeave.SelectedValue = oDaywiseStaffAttendance.LeaveId.ToString();
                    HiddenField hidUserId = e.Item.FindControl("hidUserId") as HiddenField;
                    int iUserId = lstvwUsers.DataKeys[e.Item.DisplayIndex]["UserId"].ToInt();                  
                }

                DropDownList cmbPartialLeave = e.Item.FindControl("cmbPartialLeave") as DropDownList;
                if (cmbPartialLeave != null)
                {
                    FillStaffLeaves(cmbPartialLeave, Constants.S_SELECT);
                    cmbPartialLeave.SelectedValue = oDaywiseStaffAttendance.PartialLeaveId.ToString();
                    cmbPartialLeave.Attributes.Add("onclick", "SetControlState(" + e.Item.DisplayIndex + ",0,0);");

                    string sColorCode = "White";

                    if (oDaywiseStaffAttendance.PartialLeaveId != 0)
                    {
                        if (mlstConfiguredLeaves.Where(lv => lv.LeaveId == oDaywiseStaffAttendance.LeaveId).Any())
                            sColorCode = mlstConfiguredLeaves.Where(lv => lv.LeaveId == oDaywiseStaffAttendance.PartialLeaveId).Select(lv => lv.ColorCode).FirstOrDefault();
                    }

                    cmbPartialLeave.BackColor = Color.FromName(sColorCode);
                }

                CheckBox chkHalfLeave = e.Item.FindControl("chkHalfLeave") as CheckBox;
                if (chkHalfLeave != null)
                {
                    chkHalfLeave.Checked = oDaywiseStaffAttendance.IsHalfLeave;
                    chkHalfLeave.Attributes.Add("onclick", "SetControlState(" + e.Item.DisplayIndex + ",0,1);");
                }



                CheckBox chkLateMark = e.Item.FindControl("chkLateMark") as CheckBox;
                if (chkLateMark != null)
                    chkLateMark.Checked = oDaywiseStaffAttendance.IsLateMark;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close popup and refresh parent screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            ClosePopup();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save leave details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtDate.Text);
            string sLeaveXml = base.GenerateXml(Populate());
            moStaffAttendanceBL.SaveDaywiseLeaves(miSchoolId, miUserId, dt, sLeaveXml);
            SendSMS();
            FillStaffAttendanceDetails();
            lblMessage.Text = "Staff Attendance is saved successfully !!!";
        }
       catch (SqlException ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display next day's leave details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtDate.Text);
            dt = dt.AddDays(1);
            txtDate.Text = dt.ToString(Constants.S_DATE_FORMAT);
            lblDate.Text = dt.ToString(Constants.S_DATE_FORMAT);
            FillStaffAttendanceDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display previous day's leave details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtDate.Text);
            dt = dt.AddDays(-1);
            txtDate.Text = dt.ToString(Constants.S_DATE_FORMAT);
            lblDate.Text = dt.ToString(Constants.S_DATE_FORMAT);
            FillStaffAttendanceDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to Send SMS to Staff for leave detail.
    /// </summary>
    private void SendSMS()
    {
        string sTemplateRegistrationId = string.Empty; //
        foreach (ListViewDataItem oItem in lstvwUsers.Items)
        {
            if (oItem.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkSendSMS = oItem.FindControl("chkSendSMS") as CheckBox;
                string sMobileNo = lstvwUsers.DataKeys[oItem.DisplayIndex]["MobileNo"].ToString();

                if (chkSendSMS.Checked == true && sMobileNo != string.Empty)
                {
                    string sLeaveType = string.Empty;
                    DropDownList cmbLeaveType = oItem.FindControl("cmbLeave") as DropDownList;
                    CheckBox chkHalfLeave = oItem.FindControl("chkHalfLeave") as CheckBox;
                    CheckBox chkLateMark = oItem.FindControl("chkLateMark") as CheckBox;
                    DropDownList cmbPartialLeave = oItem.FindControl("cmbPartialLeave") as DropDownList;

                    if (cmbLeaveType.SelectedValue != Constants.S_ZERO && chkHalfLeave.Checked == false)
                        sLeaveType = cmbLeaveType.SelectedItem.Text + "(1)";
                    else if (cmbLeaveType.SelectedValue != Constants.S_ZERO && chkHalfLeave.Checked && cmbPartialLeave.SelectedValue == Constants.S_ZERO)
                        sLeaveType = cmbLeaveType.SelectedItem.Text + "(0.5)";
                    else if (cmbLeaveType.SelectedValue != Constants.S_ZERO && chkHalfLeave.Checked && cmbPartialLeave.SelectedValue != Constants.S_ZERO)
                        sLeaveType = cmbLeaveType.SelectedItem.Text + "(0.5)" + " " + "&" + " " + cmbPartialLeave.SelectedItem.Text + "(0.5)";

                    int iUserId = lstvwUsers.DataKeys[oItem.DisplayIndex]["UserId"].ToInt();
                    Label UserName = oItem.FindControl("lblStaffName") as Label;

                    string sSubject = "LeaveDetailsSMS";
                    string sMessage = "You have used leave " + " " + sLeaveType + " " + "on date" + " " + txtDate.Text + " - " + "Account Department.";
                    SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
                    Hashtable oHTUsersMobileNo = new Hashtable();
                    if (oHTUsersMobileNo["TemplateRegistrationId"] != DBNull.Value)  
                        sTemplateRegistrationId = oHTUsersMobileNo["TemplateRegistrationId"].ToString();  
                    oHTUsersMobileNo[iUserId] = sMobileNo;
                    SMS oSMS = new SMS();
                    oSMS.InsertedByID = -9999;
                    oSMS.Sender = oSchoolBL.SMSSenderName;
                    oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
                    oSMS.SenderID = oSchoolBL.AdminId;
                    oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSubject;
                    oSMS.SMSText = sMessage;
                    oSMS.AcademicYearID = miAcademicYearId;
                    oSMS.SchoolID = miSchoolId;
                    oSMS.DisplayText = UserName.Text;
                    oSMS.ToManualNumbers = oHTUsersMobileNo;
                    oSMS.TemplateRegistrationId = sTemplateRegistrationId; //
                    oSMS.Send();
                    oHTUsersMobileNo.Clear();
                }
            }
        }
    }

    /// <summary>
    /// This method is used to populate leaves details.
    /// </summary>
    /// <returns></returns>
    private List<DaywiseStaffAttendance> Populate()
    {

        List<DaywiseStaffAttendance> lstAttendances = new List<DaywiseStaffAttendance>();
        foreach (ListViewDataItem oItem in lstvwUsers.Items)
        {
            if (oItem.ItemType == ListViewItemType.DataItem)
            {
                DropDownList cmbLeave = oItem.FindControl("cmbLeave") as DropDownList;
                DropDownList cmbPartialLeave = oItem.FindControl("cmbPartialLeave") as DropDownList;
                CheckBox chkHalfLeave = oItem.FindControl("chkHalfLeave") as CheckBox;
                CheckBox chkLateMark = oItem.FindControl("chkLateMark") as CheckBox;

                int iLeaveId = Convert.ToInt32(lstvwUsers.DataKeys[oItem.DisplayIndex]["LeaveId"]);

                //if (iLeaveId != 0 || (iLeaveId == 0 && (cmbLeave.SelectedValue != Constants.S_ZERO || cmbPartialLeave.SelectedValue != Constants.S_ZERO || chkHalfLeave.Checked || chkLateMark.Checked)))
                //{
                DaywiseStaffAttendance oDaywiseStaffAttendance = new DaywiseStaffAttendance
                {
                    Id = lstvwUsers.DataKeys[oItem.DisplayIndex]["Id"].ToInt(),
                    UserId = lstvwUsers.DataKeys[oItem.DisplayIndex]["UserId"].ToInt(),
                    IsHalfLeave = chkHalfLeave.Checked,
                    IsLateMark = chkLateMark.Checked,
                    LeaveId = cmbLeave.SelectedValue.ToInt(),
                    PartialLeaveId = cmbPartialLeave.SelectedValue.ToInt()
                };

                lstAttendances.Add(oDaywiseStaffAttendance);
                //}
            }
        }

        return lstAttendances;
    }

    /// <summary>
    /// This method is used to update leave balance.
    /// </summary>
    /// <param name="aoDaywiseStaffAttendance"></param>
    /// <param name="aohidLeaveBalance"></param>
    /// <param name="abIsNamePresent"></param>
    private void UpadteLeaveBalance(DaywiseStaffAttendance aoDaywiseStaffAttendance, HiddenField aohidLeaveBalance, bool abIsNamePresent)
    {
        StringBuilder oStringBuilder = new StringBuilder();
        if (aohidLeaveBalance != null)
        {
            string sLeaveBalance = aohidLeaveBalance.Value;
            string[] sArrLeaves = sLeaveBalance.Split(',');
            for (int iIndex = 0; iIndex < sArrLeaves.Length; iIndex++)
            {
                string[] sLeave = sArrLeaves[iIndex].Split(':');
                if (sLeave.Length >= 2)
                {
                    string sLeaveName = string.Empty;
                    if (mlstConfiguredLeaves.Where(lv => lv.LeaveId == aoDaywiseStaffAttendance.LeaveId).Any())
                        sLeaveName = mlstConfiguredLeaves.Where(lv => lv.LeaveId == aoDaywiseStaffAttendance.LeaveId).FirstOrDefault().ShortName;

                    if ((!abIsNamePresent && sLeave[0].ToInt() == aoDaywiseStaffAttendance.LeaveId) || (abIsNamePresent && sLeave[0].Trim() == sLeaveName))
                    {
                        decimal iLeaveCount = 0;
                        if (aoDaywiseStaffAttendance.LeaveId != 0)
                            iLeaveCount = 1;
                        if (aoDaywiseStaffAttendance.IsHalfLeave)
                            iLeaveCount = (decimal)0.5;
                        sLeave[1] = (sLeave[1].ToDecimal() + iLeaveCount).ToString();
                    }

                    if (abIsNamePresent)
                        oStringBuilder.Append(", " + (sLeave[0] + " : " + sLeave[1]));
                    else
                        oStringBuilder.Append("," + (sLeave[0] + ":" + sLeave[1]));
                }
            }
            aohidLeaveBalance.Value = oStringBuilder.Length > 0 ? oStringBuilder.ToString().Substring(1) : string.Empty;
        }
    }

    /// <summary>
    /// This method is used to fill up staff leave combo box.
    /// </summary>
    /// <param name="aoCmbLeave"></param>
    private void FillStaffLeaves(DropDownList aoCmbLeave, string asTopElement)
    {
        FillComboItem(aoCmbLeave, 0, asTopElement, "White");
        mlstConfiguredLeaves.ForEach(leave => FillComboItem(aoCmbLeave, leave.LeaveId, leave.ShortName, leave.ColorCode));
    }

    /// <summary>
    /// This method is used to fill up combo box.
    /// </summary>
    /// <param name="aoCmbLeave"></param>
    /// <param name="aiLeaveId"></param>
    /// <param name="asShortName"></param>
    /// <param name="asColorCode"></param>
    private void FillComboItem(DropDownList aoCmbLeave, int aiLeaveId, string asShortName, string asColorCode)
    {
        ListItem oListItem = new ListItem();
        oListItem.Text = asShortName;
        oListItem.Value = aiLeaveId.ToString();
        oListItem.Attributes.Add("style", "background-color:" + asColorCode);
        aoCmbLeave.Items.Add(oListItem);
    }

    /// <summary>
    /// This method is used set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        cmbStaffGroup.SelectedValue = QueryString["StaffGroupId"];
        txtName.Text = QueryString["Filter"];
        DateTime dt = new DateTime(QueryString["Year"].ToInt(), QueryString["MonthId"].ToInt(), 1);

        if (dt.Month == DateTime.Now.Month && dt.Year == DateTime.Now.Year)
            txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        else
            txtDate.Text = dt.ToString(Constants.S_DATE_FORMAT);

        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        //lnkPrevious.Attributes.Add("onclick", "if(!ShowChangeConfirmation()) return false;");
        //lnkNext.Attributes.Add("onclick", "if(!ShowChangeConfirmation()) return false;");
        //btnSearch.Attributes.Add("onclick", "if(!ShowChangeConfirmation()) return false;");

        lnkODDetails.Attributes.Add("onclick", "if(!OpenODDetailsPopup()) return false;");
        btnSave.Attributes.Add("onclick", "ResetMessage()");

        cmbLeaveHeader.Attributes.Add("onchange","SetLeaves(this)");

        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose, btnSearch });

        DateTime dtDate = Convert.ToDateTime(txtDate.Text);
        lblDate.Text = dtDate.ToString(Constants.S_DATE_FORMAT);
    }

    /// <summary>
    /// This method is sued to close popup.
    /// </summary>
    private void ClosePopup()
    {
        string sQuerystring = string.Format("MonthId={0}&Year={1}&StaffGroupId={2}&Filter={3}", hidMonthId.Value, hidYear.Value, cmbStaffGroup.SelectedValue, txtName.Text.Trim());
        sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
        sQuerystring = string.Format("SalaryDetailsUI.aspx?{0}", sQuerystring);
        hidQueryString.Value = sQuerystring;
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenP", "ClosePopup();", true);
    }

    /// <summary>
    /// This method is sued to fill up staff leaves in list view.
    /// </summary>
    private void FillStaffAttendanceDetails()
    {
        hidIsSalaryPublished.Value = Constants.S_ZERO;
        DateTime dt = Convert.ToDateTime(txtDate.Text);
        List<DaywiseStaffAttendance> lstAttendance = moStaffAttendanceBL.GetAll(miSchoolId, miAcademicYearId, dt, cmbStaffGroup.SelectedValue.ToInt(), txtName.Text.Trim());
        mlstConfiguredLeaves = moStaffAttendanceBL.ConfiguredLeaves;

        StringBuilder oStringBuilder = new StringBuilder();
        mlstConfiguredLeaves.ForEach(leave => oStringBuilder.Append("," + leave.LeaveId + ":" + leave.ColorCode));

        if (oStringBuilder.Length > 0)
            hidLeaveColors.Value = "0:white," + oStringBuilder.ToString().Substring(1);

        oStringBuilder.Clear();

        List<int> lstLeaveIds = (from id in mlstConfiguredLeaves where id.AllowZeroBalance.ToString() == "True" select id.LeaveId).ToList();
        hidAllowZeroBalance.Value = String.Join(",", lstLeaveIds);

        lstvwUsers.DataSource = lstAttendance;
        lstvwUsers.DataBind();

        if (lstAttendance.Count > 0)
        {
            ListSource.FillDropDownList(mlstConfiguredLeaves, cmbLeaveHeader, "ShortName", "LeaveId", Constants.S_SELECT);
            SetFieldState(true);
            if (moStaffAttendanceBL.IsSalaryPublished)
            {
                lstvwUsers.Enabled = false;
                btnSave.Enabled = false;
                cmbLeaveHeader.Enabled = false;
                hidIsSalaryPublished.Value = Constants.S_ONE;
            }
            else
            {
                lstvwUsers.Enabled = true;
                btnSave.Enabled = true;
                cmbLeaveHeader.Enabled = true;
            }
        }
        else
            SetFieldState(false);

        hidMonthId.Value = dt.Month.ToString();
        hidYear.Value = dt.Year.ToString();
    }

    private void SetFieldState(bool abState)
    {
        divContainer.Visible = abState;
        pnlHeader.Visible = abState;
        btnSave.Visible = abState;
    }

    /// <summary>
    /// This method is used to fill up staff group combo box.
    /// </summary>
    private void FillStaffGroups()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
        List<StaffGroupsEntity> staffGroups = oStaffGroupsBL.GetAllStaffGroups(miSchoolId);
        ListSource.FillDropDownList(staffGroups, cmbStaffGroup, "staffGroupsName", "staffGroupsId", Constants.S_ALL);
        if (cmbStaffGroup.Items.Count > 0)
            cmbStaffGroup.SelectedValue = hidStaffGroup.Value;
    }

    /// <summary>
    /// This method is used to set Query string for OD details.
    /// </summary>
    private void SetODQueryString()
    {
        string sQueryString = "&StaffGroupId=" + Convert.ToInt32(cmbStaffGroup.SelectedValue) +
                              "&Year=" + hidYear.Value;
        hidODQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
    }
    #endregion
}