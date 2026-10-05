using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utility;
using BusinessLogic.PayrollBL;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Payroll;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;

public partial class ApplyLeaveUI : SchoolBase
{
    #region Property(s)

    private const string S_UPDATE = "Update";
    private const string S_SAVE = "Submit";
    private const string S_FOLDER_PATH = @"\RITeSchool\UPLOADS\LeaveDocuments\";


    #endregion

    #region Data Member(s)

    UserApplyLeaveDetailsBL moUserApplyLeaveDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This evevt is used to load the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUserApplyLeaveDetailsBL = new UserApplyLeaveDetailsBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }

                if (QueryString["UserId"] != null && QueryString["UserId"].ToString() != string.Empty)
                    hidUserId.Value = QueryString["UserId"].ToString();
                else
                    hidUserId.Value = miUserId.ToString();

                FillUsersCombo();
                FillLeavesCombo();
                if (QueryString["CategoryId"] == Constants.S_ONE)
                {
                    tblRemark.Visible = false;
                    a.Visible = false;
                    hidCategoryId.Value = QueryString["CategoryId"];
                }

                if (QueryString["CategoryId"] != null)
                    hidCategoryId.Value = QueryString["CategoryId"];

                SetJavascriptAttribute();

                if (QueryString["Id"] != null && QueryString["Id"].ToString() != Constants.S_ZERO)
                {
                    int iId = QueryString["Id"].ToInt();
                    int iCategoryId = QueryString["CategoryId"].ToInt();
                    hidConfigId.Value = iId.ToString();
                    FillLeaveDetails(iId, iCategoryId);
                }
                else
                {
                    GetLeaveTypeWiseLeaveBalance(hidUserId.Value.ToInt());
                    SetBackdatedLeaveRestriction();
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event is used to submit the leaves.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                UserApplyLeaveDetails oUserApplyLeaveDetails = Populate();
                moUserApplyLeaveDetailsBL.Save(oUserApplyLeaveDetails);
                if (btnSubmit.Text == "Submit")
                    base.DisplayMessage("Record Saved Successfully.", false, tdMessage);

                FillUsersCombo();
                FillLeavesCombo();

                ClearFields();
                MasterPage oMasterPage = (MasterPage)this.Master;
                oMasterPage.RedirectToNextPage("~/RITeSchool/Payroll/LeaveDeatilsUI.aspx");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event is used to approve the leave.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            ProcessLeaveApproval(false); 
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    ///  This event is used to final approve the leave.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnFinalApprove_Click(object sender, EventArgs e)
    {
        try
        {
            ProcessLeaveApproval(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear all the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString = "CategoryId=" + hidCategoryId.Value;
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/RITeSchool/Payroll/LeaveDeatilsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to reject the  leave.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            
            LeaveApprovalDetails oLeaveApprovalDetails = PopulateSubmitStatus(Constants.LeaveStatuses.Rejected);
            moUserApplyLeaveDetailsBL.SaveLeaveApprovalDetails(oLeaveApprovalDetails,false);
            base.DisplayMessage("Leave request rejected Successfully!!!", false, tdMessage);             
            string sQueryString = "CategoryId=" + hidCategoryId.Value;
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/RITeSchool/Payroll/LeaveDeatilsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update leave record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdateLeaveRecord_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                moUserApplyLeaveDetailsBL.UpdateLeaveRecordinPayroll(hidConfigId.Value.ToInt(), ddlleavetype.SelectedValue.ToInt(), txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), txtTotalDays.Text.ToDecimal());
                //base.DisplayMessage("Leave Record updated Successfully.", false, tdMessage);

                string sQueryString = "CategoryId=" + hidCategoryId.Value;
                Response.Redirect("~/RITeSchool/Payroll/LeaveDeatilsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString), false);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to validate date overlapping case.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DateOverlapping_Validate(object sender, ServerValidateEventArgs e)
    {
        try
        {
            e.IsValid = moUserApplyLeaveDetailsBL.ValidateDateOverlapping(txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), hidUserId.Value.ToInt(), hidConfigId.Value.ToInt());
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to validate date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Date_Validate(object sender, ServerValidateEventArgs e)
    {
        try
        {
            string sMessage = moUserApplyLeaveDetailsBL.ValidateDates(txtStartDate.Text.ToDateTime(), ddlleavetype.SelectedValue.ToInt(), hidConfigId.Value.ToInt());
            if (sMessage != string.Empty)
            {
                CustomValidator cv = sender as CustomValidator;
                cv.ErrorMessage = sMessage;
                e.IsValid = false;
            }
            else
                e.IsValid = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event blocks applying a restricted leave type (CL/SL/LA-ED/OD) when the start
    /// date is older than the configured RestrictLeaveApplyCount days. Skipped for CategoryId 4/5
    /// and when the setting is 0 (validator stays disabled in those cases).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RestrictBackdatedLeave_Validate(object sender, ServerValidateEventArgs e)
    {
        try
        {
            int iRestrictDays = Settings.RestrictLeaveApplyCount;
            string sLeaveShortName = ddlleavetype.SelectedItem != null ? ddlleavetype.SelectedItem.Text : string.Empty;

            DateTime dtEndDate;
            // If the start date is missing/unparseable, let the RequiredField/date validators handle it.
            if (!DateTime.TryParse(txtStartDate.Text, out dtEndDate))
            {
                e.IsValid = true;
                return;
            }

            bool bRestricted = moUserApplyLeaveDetailsBL.IsBackdatedLeaveRestricted(sLeaveShortName, dtEndDate, iRestrictDays);
            if (bRestricted)
            {
                CustomValidator cv = sender as CustomValidator;
                cv.ErrorMessage = "Leave type '" + sLeaveShortName + "' cannot be applied for a end date older than " + iRestrictDays + " day(s). Only LWP is allowed for such dates.";
                e.IsValid = false;
            }
            else
                e.IsValid = true;
        }
        catch (Exception ex)
        {
            // On unexpected failure, do not silently pass this rule: mark invalid so the apply is blocked.
            e.IsValid = false;
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cvFileUpload_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!fuDocumentPhoto.HasFile)
        {
            // Non-mandatory field
            args.IsValid = true;
            return;
        }

        CustomValidator oCustomValidator = source as CustomValidator;
        string[] allowedExtensions = { ".bmp", ".jpg", ".jpeg", ".pdf", ".png" };
        string extension = System.IO.Path.GetExtension(fuDocumentPhoto.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
        {
            oCustomValidator.ErrorMessage = "Attachment file should be of type : BMP, JPG, JPEG, PDF, PNG.";
            args.IsValid = false;
            return;
        }

        int maxSize = 5 * 1024 * 1024; // 5 MB        
        if (fuDocumentPhoto.PostedFile.ContentLength > maxSize)
        {
            oCustomValidator.ErrorMessage = "Atttachment size should not be more than 5 MB.";
            args.IsValid = false;
            return;
        }

        args.IsValid = true;
        return;
    }


    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is ued to populate leave details.
    /// </summary>
    /// <returns></returns>
    private UserApplyLeaveDetails Populate()
    {
        UserApplyLeaveDetails oUserApplyLeaveDetails = new UserApplyLeaveDetails()
        {
            Id = hidPaymentId.Value.ToInt(),
            StartDate = Convert.ToDateTime(txtStartDate.Text),
            EndDate = Convert.ToDateTime(txtEndDate.Text),
            TotalDays = Convert.ToDecimal(txtTotalDays.Text),
            ChargeHandoverTo = ddlUserName.SelectedValue.ToInt(),
            Description = txtDescription.Text,
            LeaveId = ddlleavetype.SelectedValue.ToInt(),
            UserId = hidUserId.Value.ToInt(),
            DocumnetPhoto = GetFileName()
        };
        return oUserApplyLeaveDetails;
    }

    /// <summary>
    /// This method is ued to fill the user leave details.
    /// </summary>
    /// <param name="iId"></param>
    /// <param name="iCategoryId"></param>
    private void FillLeaveDetails(int iId, int iCategoryId)
    {
        UserApplyLeaveDetails oUserApplyLeaveDetails = moUserApplyLeaveDetailsBL.GetLeaveDetailsCategory(iId, hidUserId.Value.ToInt(),miUserId);
        if (iCategoryId == 1)
        tblRemark.Visible = false;
          
        else if (iCategoryId == 4 || iCategoryId == 3)
        {
            txtRemark.Enabled = false;
            btnApprove.Enabled = false;
            btnReject.Enabled = false;
            btnFinalApprove.Enabled = false;
            txtRemark.Text = oUserApplyLeaveDetails.ApproverRemark;
        }
        else if (iCategoryId == 5)
        {
            txtRemark.Text = oUserApplyLeaveDetails.ApproverRemark;
        }
        else
        {
            tblRemark.Visible = true;
            if (oUserApplyLeaveDetails.LeaveId != null && oUserApplyLeaveDetails.LeaveId != Constants.I_ZERO)
            {
                if (oUserApplyLeaveDetails.ApproverRemark != string.Empty)
                {
                    txtRemark.Text = oUserApplyLeaveDetails.ApproverRemark;
                    txtRemark.Enabled = false;
                    btnApprove.Enabled = false;
                    btnReject.Enabled = false;
                }
            }
        }
        txtStartDate.Text = oUserApplyLeaveDetails.StartDate.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = oUserApplyLeaveDetails.EndDate.ToString(Constants.S_DATE_FORMAT);
        ddlleavetype.SelectedValue = oUserApplyLeaveDetails.LeaveId.ToString();
        txtTotalDays.Text = (oUserApplyLeaveDetails.TotalDays).ToString();
        ddlUserName.SelectedValue = oUserApplyLeaveDetails.ChargeHandoverTo.ToString();
        txtDescription.Text = oUserApplyLeaveDetails.Description;
        if (!string.IsNullOrEmpty(oUserApplyLeaveDetails.DocumnetPhoto))
        {
            btnView.Visible = true;
            string fileUrl = ResolveUrl("~/RITeSchool/UPLOADS/LeaveDocuments/" + oUserApplyLeaveDetails.DocumnetPhoto);
            btnView.Attributes["onclick"] ="window.open('" + fileUrl + "', '_blank', 'height=600,width=800'); return false;";
        }
        else
        {
            btnView.Visible = false;
        }
        txtStartDate.Enabled = false;
        txtEndDate.Enabled = false;        
        ddlUserName.Enabled = false;
        txtDescription.Enabled = false;
        txtTotalDays.Enabled = false;
        btnSubmit.Visible = false;
        //a.Visible = false;
        btnCancel.Visible = false;
        fuDocumentPhoto.Visible = false;

        if (QueryString["HasFullAccess"] != null && QueryString["HasFullAccess"].ToString() == Constants.S_YES)
        {
            ddlleavetype.Enabled = true;
            btnUpdateLeaveRecord.Visible = true;
            txtStartDate.Enabled = true;
            txtEndDate.Enabled = true;
            txtTotalDays.Enabled = true;
            custValDateValidate.Enabled = true;
            trRemark.Visible = false;
            btnApprove.Visible = false;
            btnReject.Visible = false;
            trSeparator.Visible = false;
        }
        else
            ddlleavetype.Enabled = false;


        //if (oUserApplyLeaveDetails.IsFinalApprover)
        //    btnApprove.Text = "Final Approve";
        //else
        //    btnApprove.Text = "Approve";

        if (oUserApplyLeaveDetails.IsFinalApprover)
            btnFinalApprove.Visible = true;

        if (oUserApplyLeaveDetails.LastApproverUserId == miUserId)
            btnApprove.Visible = false;
        
        GetLeaveTypeWiseLeaveBalance(oUserApplyLeaveDetails.UserId);
    }
    
    /// <summary>
    /// This method is used to fill users combobox.
    /// </summary>
    private void FillUsersCombo()
    {
        DataTable dt = moUserApplyLeaveDetailsBL.GetStaffName(hidUserId.Value.ToInt());
        ListSource.FillDropDownList(dt, ddlUserName, "UserName", "UserId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is ued to fill users combobox.
    /// </summary>
    private void FillLeavesCombo()
    {
        DataTable dt = StaffLeavesBL.GetAll(miSchoolId);
        DataTable dtLeave = dt.Select("SchoolId<>-9999").CopyToDataTable();
        ListSource.FillDropDownList(dtLeave, ddlleavetype, "ShortName", "LeaveId", Constants.S_SELECT);

    }

    /// <summary>
    /// This method is ued to collect leave details for submit.
    /// </summary>
    /// <param name="aoStatusId"></param>
    /// <returns></returns>
    private LeaveApprovalDetails PopulateSubmitStatus(Constants.LeaveStatuses aoStatusId)
    {
        LeaveApprovalDetails oLeaveApprovalDetails = new LeaveApprovalDetails()
        {
            Remark = txtRemark.Text.Trim(),
            UserLeaveDetailsId = hidConfigId.Value.ToInt(),
            ReportingUserId = miUserId,
            StatusId = aoStatusId.ToInt()

        };
        return oLeaveApprovalDetails;
    }

    /// <summary>
    /// This method is used to set javascript attributes to controls.
    /// </summary>
    private void SetJavascriptAttribute()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSubmit });
        //ddlleavetype.Attributes.Add("onchange", "SelectAll()");
        btnCancel.Attributes.Add("onclick", "if(ClearControls()) {return false;} ");
        string sQueryString = "CategoryId=" + hidCategoryId.Value;
        btnBack.PostBackUrl = "~/RITeSchool/Payroll/LeaveDeatilsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);

        lblMessage.Text = string.Empty;
        valSumError.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;

        ValSumApprove.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumError.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumUpdateLeaveRecord.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        lblUserName.Text = QueryString["UserName"].ToString();
    }

    /// <summary>
    /// This method is used to clear all fields.
    /// </summary>
    private void ClearFields()
    {
        ddlleavetype.ClearSelection();
        txtStartDate.Text = string.Empty;
        txtEndDate.Text = string.Empty;
        txtDescription.Text = string.Empty;
        ddlUserName.ClearSelection();

        if (btnSubmit.Text == "Submit")
            btnSubmit.Text = S_SAVE;
    }

    /// <summary>
    /// This method is used to get login user leave balance details.
    /// </summary>
    private void GetLeaveTypeWiseLeaveBalance(int aiUserId)
    {
        List<LeaveBalance> lstLeaveBalance = moUserApplyLeaveDetailsBL.GetLeaveTypeWiseLeaveBalance(aiUserId);
        var jsSerializer = new JavaScriptSerializer();
        hidUserLeaveDetails.Value = jsSerializer.Serialize(lstLeaveBalance);

        StringBuilder obj = new StringBuilder();
        
        foreach (LeaveBalance leave in lstLeaveBalance)
            obj.Append(", " + leave.LeaveName + "(" + (leave.IsUnpaid ? "Unpaid" : leave.Balance.ToString()) + ")");

        if (obj.Length > 0)
            lblLeaveBalance.Text = "Leave Balance : " + obj.ToString().Substring(2);        
    }

    /// <summary>
    /// This method activates the back-dated leave restriction for the Submit (apply) flow.
    /// The restriction is skipped when RestrictLeaveApplyCount is 0 or when the screen is
    /// opened with CategoryId 4 or 5. Populates hidden fields so client-side validation
    /// stays in sync with the server-side rule.
    /// </summary>
    private void SetBackdatedLeaveRestriction()
    {
        int iRestrictDays = Settings.RestrictLeaveApplyCount;
        string sCategoryId = hidCategoryId.Value;
        List<string> lstAllowedLeaveTypes = UserApplyLeaveDetailsBL.GetBackdatedAllowedLeaveShortNames();
        bool bRestrictionActive = iRestrictDays > 0 && sCategoryId != "4" && sCategoryId != "5";

        cvRestrictBackdatedLeave.Enabled = bRestrictionActive;
        hidRestrictLeaveApplyCount.Value = iRestrictDays.ToString();
        hidRestrictLeaveActive.Value = bRestrictionActive ? Constants.S_ONE : Constants.S_ZERO;

        var jsSerializer = new JavaScriptSerializer();
        hidRestrictedLeaveTypes.Value = jsSerializer.Serialize(lstAllowedLeaveTypes);

        // Show the informational note only when the restriction is active and allowed types exist.
        if (bRestrictionActive && lstAllowedLeaveTypes.Count > 0)
        {
            lblRestrictLeaveNote.Text = "Only Leave Type(s) " + string.Join(", ", lstAllowedLeaveTypes.ToArray()) + " can be applied for a End Date older than " + iRestrictDays + " day(s).";
            trRestrictLeaveNote.Visible = true;
        }
    }
    /// <summary>
    /// these method is used to get file name.
    /// </summary>
    /// <returns></returns>
    private string GetFileName()
    {
        string sFileName = string.Empty;
        HttpFileCollection oCollection = Request.Files;

        if (oCollection.Count > 0)
        {
            HttpPostedFile aoAttachment = oCollection[0];

            if (!aoAttachment.FileName.Trim().Equals(string.Empty))
            {
                sFileName = aoAttachment.FileName;
                string sPath = base.BasePath + S_FOLDER_PATH + sFileName;

                if (File.Exists(sPath))
                {
                    sFileName = CommonUtility.GetFileNameForRenaming(sFileName);
                    sPath = base.BasePath + S_FOLDER_PATH + sFileName;
                }

                aoAttachment.SaveAs(sPath);
            }
        }

        return sFileName;
    }

    /// <summary>
    ///  This event is used  approve the leave.
    /// </summary>
    /// <param name="IsFromFinalApproval"></param>
    private void ProcessLeaveApproval(bool IsFromFinalApproval)
    {
        LeaveApprovalDetails oLeaveApprovalDetails = PopulateSubmitStatus(Constants.LeaveStatuses.Approved);
        moUserApplyLeaveDetailsBL.SaveLeaveApprovalDetails(oLeaveApprovalDetails, IsFromFinalApproval);
        base.DisplayMessage("Leave request approved Successfully!!!", false, tdMessage);
        string sQueryString = "CategoryId=" + hidCategoryId.Value;
        MasterPage oMasterPage = (MasterPage)this.Master;
        oMasterPage.RedirectToNextPage("~/RITeSchool/Payroll/LeaveDeatilsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
    }
    
    #endregion    
}
