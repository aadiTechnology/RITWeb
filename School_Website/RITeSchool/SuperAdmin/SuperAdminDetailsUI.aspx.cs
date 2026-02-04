/* File Name = SuperAdminDetailsUI.aspx.cs
 * Created Date - 17 Aug 2011
 * Modified Date  - 18 Aug 2011
 * Created by - Vipul
 * Class Description - This class is defined to manage super admin details.*/

using System;
using System.Web.UI.WebControls;
using BusinessLogic;
using SuperAdminEntities;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Collections.Generic;

public partial class SuperAdminDetailsUI :SchoolBase
{

    #region "Constants"

    const string S_COMMAND_REMOVE = "Remove";
    const string S_COMMAND_UPDATE = "Update";
    const string S_UPDATE = "Update";
    const string S_SAVE = "Save";
    const string S_SORT = "Sort";
    const string S_SUCCESSFULL_SAVE = "Details updated successfully !!!";
    const string S_SUCCESSFULL_UPDATE = "Details saved successfully !!!";
    const string S_DUPLICATE_USERNAME_MESSAGE = "User name already exists.";

    #endregion "Constants"

    SuperAdminDetailsBL moSuperAdminDetailsBL;

    #region "Events"

    /// <summary>
    /// This event is used to fill super admin details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
                oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
                FillSuperAdminDetails();
                SetJavaScriptAttributres();
                SetDefaultValuesToControls();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save super admin details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text != S_UPDATE)
                Save();
            else
                UpdateSuperAdminDetails();
            FillSuperAdminDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set clear controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SetDefaultValuesToControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back to previous screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("ManagementFileSharingUI.aspx", false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add confirmation message while deleting existing super admin details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSuperAdminDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
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
    /// This event is used to edit or delete super admin details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSuperAdminDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != S_SORT)
            {
                lblUpdateSucess.Text = string.Empty;
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iSuperAdminDetailsId = Convert.ToInt32(lstvwSuperAdminDetails.DataKeys[iListIndex]["SuperAdminDetailsId"]);
                hidUserId.Value = Convert.ToString(lstvwSuperAdminDetails.DataKeys[iListIndex]["UserId"]);
                hidSuperAdminDetailsId.Value = Convert.ToString(iSuperAdminDetailsId);
                if (e.CommandName == S_COMMAND_REMOVE)
                {
                    DeleteSuperAdminDetails(iSuperAdminDetailsId);
                    FillSuperAdminDetails();
                }
                else if (e.CommandName == S_COMMAND_UPDATE)
                    LoadSuperAdminDetails(iSuperAdminDetailsId);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion "Events"

    #region "Public Methods"

    /// <summary>
    /// This method is used to fill super admin details.
    /// </summary>
    private void FillSuperAdminDetails()
    {
        moSuperAdminDetailsBL = new SuperAdminDetailsBL();
        lstvwSuperAdminDetails.DataSource = moSuperAdminDetailsBL.GetAll();
        lstvwSuperAdminDetails.DataBind();
    }

    /// <summary>
    /// This method is used to update super admin details.
    /// </summary>
    private void UpdateSuperAdminDetails()
    {
        moSuperAdminDetailsBL = new SuperAdminDetailsBL();
        moSuperAdminDetailsBL.SuperAdminDetails = PopulateSuperAdminDetails();
        if (hidSuperAdminDetailsId.Value != string.Empty)
            moSuperAdminDetailsBL.SuperAdminDetails.SuperAdminDetailsId = Convert.ToInt32(hidSuperAdminDetailsId.Value);
        if (hidUserId.Value != string.Empty)
            moSuperAdminDetailsBL.SuperAdminDetails.UserId = Convert.ToInt32(hidUserId.Value);
        if (!moSuperAdminDetailsBL.IsDuplicate())
        {
            moSuperAdminDetailsBL.Update();
            lblUpdateSucess.Text = S_SUCCESSFULL_SAVE;
            SetDefaultValuesToControls();
            btnSave.Text = S_SAVE;
        }
        else
            lblErrorMsg.Text = S_DUPLICATE_USERNAME_MESSAGE;
    }

    /// <summary>
    /// This method is used to save super admin details.
    /// </summary>
    private void Save()
    {
        moSuperAdminDetailsBL = new SuperAdminDetailsBL();
        moSuperAdminDetailsBL.SuperAdminDetails = PopulateSuperAdminDetails();
        if (!moSuperAdminDetailsBL.IsDuplicate())
        {
            moSuperAdminDetailsBL.Insert();
            lblUpdateSucess.Text = S_SUCCESSFULL_UPDATE;
            SetDefaultValuesToControls();
        }
        else
            lblErrorMsg.Text = S_DUPLICATE_USERNAME_MESSAGE;

    }

    /// <summary>
    /// This method is used to populate "SuperAdminDetails" object.
    /// </summary>
    /// <returns></returns>
    private SuperAdminDetails PopulateSuperAdminDetails()
    {
        SuperAdminDetails oSuperAdminDetails = new SuperAdminDetails();
        oSuperAdminDetails.FirstName = txtFirstName.Text.Trim();
        oSuperAdminDetails.MiddleName = txtMiddleName.Text.Trim();
        oSuperAdminDetails.LastName = txtLastName.Text.Trim();
        oSuperAdminDetails.SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue);
        oSuperAdminDetails.MobileNumber = txtMobileNo.Text.Trim();
        oSuperAdminDetails.UserName = txtUserName.Text.Trim();
        oSuperAdminDetails.Password = CommonUtility.GetEncryptedPassword(oSuperAdminDetails.UserName, txtPasswd.Text);
        oSuperAdminDetails.InsertedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oSuperAdminDetails.UpdatedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        return oSuperAdminDetails;
    }

    /// <summary>
    /// This method is used to set javascript attributes for buttons.
    /// </summary>
    private void SetJavaScriptAttributres()
    {
        btnSave.Attributes["onclick"] = "javascript:btnsaveonclick('" + btnSave.ClientID + "',this);";
        ApplyMouseHoverEffect(new List<Button> { btnSave, BtnCancel, btnBack });
    }

    /// <summary>
    /// This method is used to load super admin details.
    /// </summary>
    /// <param name="aiSuperAdminDetailsId"></param>
    private void LoadSuperAdminDetails(int aiSuperAdminDetailsId)
    {
        moSuperAdminDetailsBL = new SuperAdminDetailsBL(aiSuperAdminDetailsId);
        SuperAdminDetails oSuperAdminDetails = moSuperAdminDetailsBL.SuperAdminDetails;
        txtFirstName.Text = oSuperAdminDetails.FirstName;
        txtLastName.Text = oSuperAdminDetails.LastName;
        txtMiddleName.Text = oSuperAdminDetails.MiddleName;
        txtMobileNo.Text = oSuperAdminDetails.MobileNumber;
        txtUserName.Text = oSuperAdminDetails.UserName;
        string sPassword = CommonUtility.GetDecryptedPassword(oSuperAdminDetails.UserName, oSuperAdminDetails.Password);
        txtPasswd.Attributes.Add("value", sPassword);
        txtConfirmPasswd.Attributes.Add("value", sPassword);
        btnSave.Text = S_UPDATE;
    }

    /// <summary>
    /// This method is uesd to handle item updating event of listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSuperAdminDetails_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
    }

    /// <summary>
    /// This method is used to clear controls.
    /// </summary>
    private void SetDefaultValuesToControls()
    {
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtMiddleName.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtUserName.Text = string.Empty;
        txtPasswd.Attributes.Add("value", string.Empty);
        txtConfirmPasswd.Attributes.Add("value", string.Empty);
        btnSave.Text = S_SAVE;
    }

    /// <summary>
    /// This method is used to delete super admin details.
    /// </summary>
    /// <param name="aiSuperAdminDetailsId"></param>
    private void DeleteSuperAdminDetails(int aiSuperAdminDetailsId)
    {
        moSuperAdminDetailsBL = new SuperAdminDetailsBL();
        moSuperAdminDetailsBL.SuperAdminDetails.UpdatedById = miUserId;
        moSuperAdminDetailsBL.SuperAdminDetails.SuperAdminDetailsId = aiSuperAdminDetailsId;
        moSuperAdminDetailsBL.Delete();
        SetDefaultValuesToControls();
    }
    #endregion "Public Methods"
}
