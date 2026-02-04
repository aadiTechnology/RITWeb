/* -----------------------------------------------------------------------------------------------
 *	FileName	: RemarksConfigurationUI.aspx.cs
 *	Author		: Vishal B. Shah
 *	Date		: 3-Dec-2011
 *	Description	: This is the code behind file for the Remarks configuration screen,
 *				  which is used to configure what is available to enter remarks for each student.
 * -----------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Resources;

public partial class RemarksConfigurationUI : SchoolBase
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #region -- CONSTANT(s) -- 
    private const string S_EDIT = "EDIT_ROW";
    private const string S_DELETE = "DELETE_ROW";

    #endregion -- CONSTANT(s) --

    #region -- EVENT(s) --

    /// <summary>
    /// This procedeure is used to load default data in grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            if (!IsPostBack)
            {
                hidSave.Value = "Save";
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();  
                }
                DesignSettingAccordingLanguage();
                DisplayRemarks();
                Initialize();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is used to save the remarks for the students.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            RemarksConfigurationBL oRemarkconfig = new RemarksConfigurationBL();
            RemarksConfig oRemarksConfig = new RemarksConfig
            {
                Id = hidRemarksConfigId.Value == string.Empty ? 0 : Convert.ToInt32(hidRemarksConfigId.Value),
                Name = txtRemarkName.Text.Trim(),
                SortOrder = Convert.ToInt32(txtSortOrder.Text),
                AcademicYearId = miAcademicYearId,
                SchoolId = miSchoolId,
                InsertedById = miUserId,
                UpdatedById = miUserId
            };
            if (hidSave.Value == "Save")
            {
                RemarksConfigurationBL.Save(oRemarksConfig);
                if (!IsConfigured())
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.RemarksConfiguration));
            }
            else if (hidSave.Value == "Update")
            {
                RemarksConfigurationBL.Update(oRemarksConfig);
            }
            SetMessage(hidSave.Value == "Save" ? Resources.LocalizedResources.MsgRemarkConfigurationSavedSuccess : Resources.LocalizedResources.MsgRemarkConfigurationUpdatedSuccess, false);

            DisplayRemarks();
            ClearControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle differrent events like edit,delete.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRemarks_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRemarksConfigId = Convert.ToInt32(lstvwRemarks.DataKeys[oCurrentItem.DisplayIndex]["Id"]);
                hidRemarksConfigId.Value = lstvwRemarks.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToString();
                hidRowNo.Value = (oCurrentItem.DisplayIndex + 1).ToString();
                RemarksConfig oRemarksConfig;
                switch (e.CommandName)
                {
                    case S_EDIT:
                        LoadRemarlDetails(iRemarksConfigId);
                        break;
                    case S_DELETE:
                        oRemarksConfig = new RemarksConfig { Id = iRemarksConfigId, SchoolId = miSchoolId, AcademicYearId = miAcademicYearId, UpdatedById = miUserId };
                        RemarksConfigurationBL.Delete(oRemarksConfig);
                        SetMessage(Resources.LocalizedResources.MsgRemarksConfigurationDeletedSuccessfully, false);
                        ClearControls();
                        break;
                }
                DisplayRemarks();
                if (lstvwRemarks.Items.Count == 0)
                    DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.RemarksConfiguration));
            }
            lblErrorMsg.Text = string.Empty;
        }
        catch (SqlException oEx)
        {
            lblErrorMsg.Text = oEx.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is used to bind confirmation event to delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRemarks_ItemDataBound(object sender, ListViewItemEventArgs e)
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
    /// This event is used to bind the rowcount to the hidden field.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRemarks_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = lstvwRemarks.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Thus event will clear all the fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }



    #endregion -- EVENT(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This function is used to load remark details.
    /// </summary>
    /// <param name="aiRemarksConfigId"></param>
    private void LoadRemarlDetails(int aiRemarksConfigId)
    {
        RemarksConfig oRemarksConfig = RemarksConfigurationBL.GetRemarkDetails(miSchoolId, miAcademicYearId, aiRemarksConfigId);
        txtRemarkName.Text = oRemarksConfig.Name;
        txtSortOrder.Text = oRemarksConfig.SortOrder.ToString();
        btnSave.Text = Resources.LocalizedResources.Update;
        hidSave.Value = "Update";
    }

    /// <summary>
    /// This procedure is used to display remarks.
    /// </summary>
    private void DisplayRemarks()
    {
        lstvwRemarks.DataSource = RemarksConfigurationBL.GetConfig(miSchoolId, miAcademicYearId);
        lstvwRemarks.DataBind();
    }

    /// <summary>
    /// This procedure is used to set appropriate message after any operation.
    /// </summary>
    /// <param name="asMessage"></param>
    /// <param name="IsError"></param>
    private void SetMessage(string asMessage, bool IsError)
    {
        lblUpdateMessage.Text = asMessage;
        lblUpdateMessage.ForeColor = IsError ? Color.Red : Color.Blue;
        if (IsError)
            lblUpdateMessage.Font.Bold = true;
    }

    /// <summary>
    /// This procedure is used to initialize default fields.
    /// </summary>
    private void Initialize()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related));
        txtRemarkName.Focus();
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    /// <returns></returns>
    private bool IsConfigured()
    {
        if (!QueryString["Is_Configured"].IsNull())
            return QueryString["Is_Configured"].ToString() == Constants.C_YES.ToString();
        else
            return false;
    }

    /// <summary>
    /// This method is used for clear the fields.
    /// </summary>
    private void ClearControls()
    {
        hidRowNo.Value = Constants.S_ZERO;
        lblErrorMsg.Text = string.Empty;
        txtRemarkName.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        btnSave.Text = Resources.LocalizedResources.Save;
        hidSave.Value = "Save";
        hidRemarksConfigId.Value = string.Empty;
    }

    /// <summary>
    /// This method is used to set design according to selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidAreYouSureYouWantToDeleteThisRemarkAndAllReleatedTemplates.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisRemarkAndAllReleatedTemplates;
        hidRemarkTypeShouldNotBeDuplicated.Value = Resources.LocalizedResources.RemarkTypeShouldNotBeDuplicated;
        hidSortOrderShouldNotBeDuplicated.Value = Resources.LocalizedResources.SortOrderShouldNotBeDuplicated;
        hidSortOrderShouldNotBeZero.Value = Resources.LocalizedResources.SortOrderShouldNotBeZero;
        btnSave.Text = oResourceManager.GetString(hidSave.Value.Replace(" ", string.Empty));
    }
    #endregion -- PRIVATE METHOD(s) --
}
