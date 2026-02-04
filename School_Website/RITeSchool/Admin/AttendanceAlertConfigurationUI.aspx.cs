using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Web.UI.WebControls;
using BookEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Admin;
using Utility;
using System.Linq;

public partial class AttendanceAlertConfigurationUI : SchoolBase
{
    #region -- CONSTANT(s) --

    private const string S_MESSAGE = "Missing attendance alert configuration for selected user is %OPERATION% successfully!!!";
    private const string S_USER_SUCCESS_EXISTS = "Missing attendance alert configuration for selected user is already exist.";   

    #endregion -- CONSTANT(s) --

    #region --Events(s)--
    /// <summary>
    /// This method is used to load data in listview firsttime.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
          
            if (!IsPostBack)
            {
                RefreshValue();
                SetJavaScriptAttributes();
                FillRoleCombo();
                if (CheckPreCondition())
                    FillAttendanceAlertConfigListview();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValue();
                }
               
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called upon changing the role.As per the selected role users gets added.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbRole.SelectedValue.ToInt() > Constants.I_ZERO)
            {
                FillUsers(cmbRole.SelectedValue.ToInt());
                cmbUsers.Enabled = true;
            }
            else
            {
                cmbUsers.ClearSelection();
                cmbUsers.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the edit and delete commands of listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAttendanceAlertConfig_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                hidConfigId.Value = lstvwAttendanceAlertConfig.DataKeys[oCurrentItem.DisplayIndex]["ConfigId"].ToString();
                switch (e.CommandName)
                {
                    case Constants.S_COMMAND_UPDATE:
                        int iRoleId = lstvwAttendanceAlertConfig.DataKeys[oCurrentItem.DisplayIndex]["RoleId"].ToInt();
                        if (iRoleId != 0)
                        {
                            FillUsers(iRoleId);                            
                            SetAttendanceConfigDetails(hidConfigId.Value.ToInt());
                            cmbUsers.Enabled = false;
                            cmbRole.Enabled = false;
                        }
                        break;

                        //Following case is used to delete the configured person.
                    case Constants.S_COMMAND_REMOVE:
                        int iUserId = Convert.ToInt32(lstvwAttendanceAlertConfig.DataKeys[oCurrentItem.DisplayIndex]["UserId"]);
                        AttendanceAlertConfigBL oAttendanceAlertConfigBL = new AttendanceAlertConfigBL(miSchoolId, miAcademicYearId);
                        AttendanceAlertConfigDetails oAttendanceAlertConfigDetails = new AttendanceAlertConfigDetails { UserId = iUserId, InsertedById = miUserId};
                        oAttendanceAlertConfigBL.Delete(oAttendanceAlertConfigDetails);
                        SetMessage(Resources.LocalizedResources.Deleted, false);
                        cmbRole.Enabled = true;
                        ClearFields();
                        break;
                }

                FillAttendanceAlertConfigListview();
            }
            lblErrorMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is called to save configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            AttendanceAlertConfigDetails oAttendanceAlertConfigDetails = new AttendanceAlertConfigDetails
            {
                UserId=cmbUsers.SelectedValue.ToInt(),
                NoOfDays=Convert.ToInt16(txtDays.Text.Trim()),                
                InsertedById=miUserId
            };
            AttendanceAlertConfigBL oAttendanceAlertConfigBL = new AttendanceAlertConfigBL(miSchoolId,miAcademicYearId,hidConfigId.Value.ToInt());
            int iCount = oAttendanceAlertConfigBL.Save(oAttendanceAlertConfigDetails);

            if (hidConfigId.Value.ToInt() > Constants.I_ZERO && iCount == Constants.I_TWO)
                SetMessage(Resources.LocalizedResources.Updated, false);
            else if (hidConfigId.Value.ToInt() == Constants.I_ZERO && iCount == Constants.I_ZERO)
                SetMessage(Resources.LocalizedResources.Added, false);
            else
                lblErrorMsg.Text = Resources.LocalizedResources.MsgUserAlereadyExist;

            FillAttendanceAlertConfigListview();
            ClearFields();
            cmbRole.Enabled = true;
            if (ReadQueryString() != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AttendanceAlertConfiguration));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwAttendanceAlertConfig_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is called to clear all the fields and hidden variables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();           
            cmbRole.Enabled = true;
            lblErrorMsg.Text = string.Empty;
        }
        catch(Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion --Events(s)--

    #region --Method(s)--

    /// <summary>
    /// This method is used to fill the users.
    /// </summary>
    private void FillUsers(int aiRoleId)
    {
        List<AttendanceAlertConfigDetails> olstAttendanceAlertConfigDetails = MasterDataCollectionBL.GetUsers(aiRoleId, miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(olstAttendanceAlertConfigDetails, cmbUsers, "UserName", "UserId", Constants.S_SELECT);
    }
    /// <summary>
    /// This method is called to fill roles in combo.
    /// </summary>
    private void FillRoleCombo()
    {
        List<UserRoles> olstRoles = MasterDataCollectionBL.GetUserRoles();
        olstRoles = olstRoles.Where(rl => rl.User_Role_Id != Convert.ToInt32(Constants.UserRoles.ExAdmin)).ToList();
        ListSource.FillDropDownList(olstRoles, cmbRole, "User_Role_Name", "User_Role_Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is called to set appropriate messages.
    /// </summary>
    /// <param name="asMessage"></param>
    /// <param name="abIsErrorMessage"></param>
    private void SetMessage(string asOperation, bool abIsErrorMessage)
    {
        lblErrorMsg.Text = string.Empty;
        lblUpdateMessage.Text = Resources.LocalizedResources.MSgAttadanceAlert.Replace("%OPERATION%", asOperation.ToLower());
        lblUpdateMessage.Font.Bold = true;
        if (abIsErrorMessage)
            lblUpdateMessage.ForeColor = Color.Red;
        else
            lblUpdateMessage.ForeColor = Color.Blue;
    }

    /// <summary>
    /// This method is called to set the javascript attributes on pageload.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Attendance_Related));
        cmbRole.Focus();
    }

    /// <summary>
    /// This method is called to clear the fields.
    /// </summary>
    private void ClearFields()
    {
        hidConfigId.Value = Constants.S_ZERO;
        cmbRole.ClearSelection();
        cmbUsers.ClearSelection();
        cmbUsers.Enabled = false;
        txtDays.Text = string.Empty;
    }

    /// <summary>
    /// This method is called to fill the listview.
    /// </summary>
    private void FillAttendanceAlertConfigListview()
    {
        AttendanceAlertConfigBL oAttendanceAlertConfigBL = new AttendanceAlertConfigBL(miSchoolId, miAcademicYearId);
        lstvwAttendanceAlertConfig.DataSource = oAttendanceAlertConfigBL.GetAll();
        lstvwAttendanceAlertConfig.DataBind();
        if (lstvwAttendanceAlertConfig.Items.Count == 0)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AttendanceAlertConfiguration));
    }

    /// <summary>
    /// This method reads the querystring.
    /// </summary>
    /// <returns></returns>
    private string ReadQueryString()
    {
       return QueryString["Is_Configured"];
    }

    /// <summary>
    /// This method checks the precondition for the page.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AttendanceAlertConfiguration);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
            upnlListView.Visible = true;
        }
        else
        {
            trPrecondition.Visible = true;
            divErr.InnerHtml = sLinks;
            upnlListView.Visible = false;
        }

        return bReturn;
    }

    /// <summary>
    /// This is used to getting the listview details.
    /// </summary>
    /// <param name="aiConfigId"></param>
    private void SetAttendanceConfigDetails(int aiConfigId)
    {
        AttendanceAlertConfigBL oAttendanceAlertConfigBL = new AttendanceAlertConfigBL(miSchoolId,miAcademicYearId,aiConfigId);
        AttendanceAlertConfigDetails oAttendanceConfigDetails = oAttendanceAlertConfigBL.GetDetails();
        cmbRole.SelectedValue = oAttendanceConfigDetails.RoleId.ToString();
        cmbUsers.SelectedValue = oAttendanceConfigDetails.UserId.ToString();
        txtDays.Text = oAttendanceConfigDetails.NoOfDays.ToString();
    }

    private void RefreshValue()
    {
        hidValBlankTimeSpan.Value = Resources.LocalizedResources.ValBlankTimeSpan;
        hidAlertDeleteUser.Value = Resources.LocalizedResources.AlertDeleteUser;
        hidValTimeSpan.Value = Resources.LocalizedResources.ValTimeSpan;
    }
    #endregion --Method(s)--
}