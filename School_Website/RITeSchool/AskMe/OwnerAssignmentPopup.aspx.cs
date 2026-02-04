using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Data.SqlClient;

public partial class OwnerAssignmentPopup : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Owner Assignment saved successfully !!!";
    private const string S_SUBMIT_MESSAGE = "Owner Assignment %ACTION% successfully !!!";
    private const string S_DELETE_MESSAGE = "Owner Assignment deleted successfully !!!";
    
    #endregion

    public enum SubmitAction
    {
        Submit,
        Unsubmit
    }

    #region Data Member(s)

    private AskMeQuestionMasterBL moAskMeQuestionMasterBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill user role combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moAskMeQuestionMasterBL = new AskMeQuestionMasterBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillUserRoles();
                FillUsers();
                ReadQueryString();
                SetJavascriptAttributes();
                InitFields();
                FillOwnerDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event i used to to fill up user combo box according to selected user role.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUsers();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save owner assignment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            AskMeOwnerAssignment oAskMeOwnerAssignment = new AskMeOwnerAssignment
            {
                Id = Convert.ToInt32(hidId.Value),
                OwnerId = Convert.ToInt32(cmbUser.SelectedValue),
                QuestionId = Convert.ToInt32(hidQuestionId.Value)
            };

            moAskMeQuestionMasterBL.SetOwnerAssignment(oAskMeOwnerAssignment);
            lblMessage.Text = S_SAVE_MESSAGE;
            ClearFields();
            FillOwnerDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel current operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit owner assignment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool bIsSubmit = (btnSubmit.Text == SubmitAction.Submit.ToString() ? true : false);
            moAskMeQuestionMasterBL.SubmitOwnerAssignment(hidQuestionId.Value.ToInt(), bIsSubmit);

            lblMessage.Text = S_SUBMIT_MESSAGE.Replace("%ACTION%", (bIsSubmit ? "submitted" : "unsubmitted"));

            if (bIsSubmit)
            {
                btnSave.Enabled = false;
                hidIsOwnerAssignmentSubmitted.Value = Constants.S_YES;
                btnSubmit.Text = SubmitAction.Unsubmit.ToString();
            }
            else
            {
                btnSave.Enabled = true;
                hidIsOwnerAssignmentSubmitted.Value = Constants.S_NO;
                btnSubmit.Text = SubmitAction.Submit.ToString();
            }

            FillOwnerDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set fields state as per condition.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOwners_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                if (hidIsOwnerAssignmentSubmitted.Value == Constants.S_YES)
                {
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }

                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to edit / delete selected record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOwners_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iId = lstvwOwners.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    int iUserRoleId = lstvwOwners.DataKeys[e.Item.DisplayIndex]["UserRoleId"].ToInt();
                    int iOwnerId = lstvwOwners.DataKeys[e.Item.DisplayIndex]["OwnerId"].ToInt();

                    cmbUserRole.SelectedValue = iUserRoleId.ToString();
                    cmbUserRole_SelectedIndexChanged(cmbUserRole, null);

                    cmbUser.SelectedValue = iOwnerId.ToString();
                    hidId.Value = iId.ToString();
                    btnSave.Text = Constants.ButtonText.Update.ToString();
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moAskMeQuestionMasterBL.DeleteOwnerAssignment(iId);
                    if (hidId.Value == iId.ToString())
                        ClearFields();

                    lblMessage.Text = S_DELETE_MESSAGE;
                    FillOwnerDetails();
                }
            }
        }
        catch (SqlException ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.Font.Bold = false;
            lblMessage.Style.Add("TextAling","Left");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to intiialize felds.
    /// </summary>
    private void InitFields()
    {
        int iQuestionId = Convert.ToInt32(hidQuestionId.Value);
        AskMeQuestionMaster oAskMeQuestionMaster = AskMeQuestionMasterBL.GetQuestionDetails(miSchoolId, miAcademicYearId, 0, iQuestionId, miUserId);
        if (oAskMeQuestionMaster != null && oAskMeQuestionMaster.AskMeQuestionDetails != null)
        {
            lblDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
            lblTitle.Text = oAskMeQuestionMaster.Title;
            //cmbUserRole.SelectedValue = oAskMeQuestionMaster.UserRoleId.ToString();
            cmbUserRole_SelectedIndexChanged(cmbUserRole, null);
            cmbUser.SelectedValue = oAskMeQuestionMaster.OwnerUserId.ToString();
            lblCategories.Text = oAskMeQuestionMaster.CategoryNames;
            hidIsOwnerAssignmentSubmitted.Value = (oAskMeQuestionMaster.IsOwnerAssignmentSubmitted ? Constants.S_YES : Constants.S_NO);

            if (oAskMeQuestionMaster.IsOwnerAssignmentSubmitted)
            {
                btnSave.Enabled = false;
                btnSubmit.Text = SubmitAction.Unsubmit.ToString();
            }

            hidIsCommunicationStarted.Value = (oAskMeQuestionMaster.IsCommunicationStarted ? Constants.S_YES : Constants.S_NO);
        }
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to fill up user roles.
    /// </summary>
    private void FillUserRoles()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDtUserRoles = oMasterDataCollectionBL.GetAllUserRoles();
        DataRow[] dr = oDtUserRoles.Select("User_Role_Id IN (" + Constants.UserRoles.Admin.ToInt() + "," + Constants.UserRoles.Teacher.ToInt() + "," + Constants.UserRoles.Supervisor.ToInt() + ")");
        ControlUtility.FillDropDownList(dr.CopyToDataTable(), ref cmbUserRole, "User_Role_Id", "User_Role_Name", Constants.S_SELECT);
        cmbUserRole.SelectedValue = Constants.UserRoles.Teacher.ToInt().ToString();
        cmbUserRole_SelectedIndexChanged(cmbUserRole, null);
    }
    
    /// <summary>
    /// This method is used to fill users.
    /// </summary>
    private void FillUsers()
    {   
        List<AskMeOwnerAssignment> lstSubjectTeachers = moAskMeQuestionMasterBL.GetAllSubjectTeachers(cmbUserRole.SelectedValue.ToInt());
        ListSource.FillDropDownList(lstSubjectTeachers, cmbUser, "OwnerName", "OwnerId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        hidId.Value = Constants.S_ZERO;
        //cmbUserRole.ClearSelection();
        cmbUser.ClearSelection();
        btnSave.Text = Constants.ButtonText.Save.ToString();
    }

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        hidQuestionId.Value = QueryString["QuestionId"].ToString();
    }

    /// <summary>
    /// This method is used to fill owner details.
    /// </summary>
    private void FillOwnerDetails()
    {
        List<AskMeOwnerAssignment> lstOwners = moAskMeQuestionMasterBL.GetAllOwners(hidQuestionId.Value.ToInt());
        lstvwOwners.DataSource = lstOwners;
        lstvwOwners.DataBind();

        if (hidIsCommunicationStarted.Value == Constants.S_NO)
        {
            if (lstOwners.Count == 0)
                btnSubmit.Enabled = false;
            else
                btnSubmit.Enabled = true;
        }
        else
            btnSubmit.Enabled = false;
    }

    #endregion    
}