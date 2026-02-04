// File Name  : RemarksCategoryUI.aspx.cs
// Description : This class is used to add Remarks Category
// Created By : Sharvari
// Date       : 15 Oct 12

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using BusinessLogic;
using SchoolEntities;
using Utility;
using System.Data.SqlClient;
using System.Drawing;
using System.Resources;

public partial class RemarksCategoryUI : SchoolBase
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #region -- CONSTANT(s) --

    private const string S_EDIT = "EDIT_ROW";
    private const string S_DELETE = "DELETE_ROW";
    #endregion -- CONSTANT(s) --

    #region Events

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
                if(Session[Constants.S_SESSION_LANGUAGE]!= null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
                DisplayRemarks();
                Initialize();
            }
            lblErrorMsg.Text = string.Empty;
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
    /// This method is used to save Remarks Category
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            RemarksCategoryBL oRemarksCategory = new RemarksCategoryBL();
            RemarksConfig oRemarksConfig = new RemarksConfig
            {
                Id = hidRemarksCategoryId.Value == string.Empty ? 0 : Convert.ToInt32(hidRemarksCategoryId.Value),
                Name = txtRemarkName.Text.Trim(),
                SortOrder = Convert.ToInt32(txtSortOrder.Text),
                AcademicYearId = miAcademicYearId,
                SchoolId = miSchoolId,
                InsertedById = miUserId,
                UpdatedById = miUserId
            };
            if (hidRemarksCategoryId.Value == string.Empty)
            {
                RemarksCategoryBL.Save(oRemarksConfig, 1);
                if (!IsConfigured() || lstvwRemarksCategory.Items.Count == Constants.I_ZERO)            //This is because initially there are no records in listview but IsConfigured in querystring is no
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.RemarksCategory));
            }
            else
            {
                RemarksCategoryBL.Save(oRemarksConfig, 0);
            }
            SetMessage(hidRemarksCategoryId.Value == string.Empty ? Resources.LocalizedResources.RemarkCategorySavedSuccessfully : Resources.LocalizedResources.RemarkCategoryUpdatedSuccessfully, false);

            DisplayRemarks();
            ClearControls();
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

    /// <summary>
    /// This event is used to handle differrent events like edit,delete.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRemarksCategory_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRemarksConfigId = Convert.ToInt32(lstvwRemarksCategory.DataKeys[oCurrentItem.DisplayIndex]["Id"]);
                hidRemarksCategoryId.Value = lstvwRemarksCategory.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToString();
                hidRowNo.Value = (oCurrentItem.DisplayIndex + 1).ToString();
                RemarksConfig oRemarksConfig;
                switch (e.CommandName)
                {
                    case S_EDIT:
                        LoadRemarkCategoryDetails(iRemarksConfigId);
                        break;
                    case S_DELETE:
                        oRemarksConfig = new RemarksConfig { Id = iRemarksConfigId, SchoolId = miSchoolId, AcademicYearId = miAcademicYearId, UpdatedById = miUserId };
                        RemarksCategoryBL.Delete(oRemarksConfig);
                        SetMessage(Resources.LocalizedResources.RemarkCategoryDeletedSuccessfully, false);
                        ClearControls();
                        if (lstvwRemarksCategory.Items.Count == Constants.I_ONE)
                            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.RemarksCategory));
                        break;
                }
                DisplayRemarks();
            }
            lblErrorMsg.Text = string.Empty;
        }
        catch (SqlException)
        {
            lblErrorMsg.Text = Resources.LocalizedResources.CanNotBeDeletedSinceItIsAssociatedWithTemplate;
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
    protected void lstvwRemarksCategory_ItemDataBound(object sender, ListViewItemEventArgs e)
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
    protected void lstvwRemarksCategory_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = lstvwRemarksCategory.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events

    #region Private methods

    /// <summary>
    /// This function is used to load remark details.
    /// </summary>
    /// <param name="aiRemarksConfigId"></param>
    private void LoadRemarkCategoryDetails(int aiRemarksCategoryId)
    {
        RemarksConfig oRemarksConfig = RemarksCategoryBL.GetRemarkDetails(miSchoolId, miAcademicYearId, aiRemarksCategoryId);
        txtRemarkName.Text = oRemarksConfig.Name;
        txtSortOrder.Text = oRemarksConfig.SortOrder.ToString();
        hidRemarksCategoryId.Value = aiRemarksCategoryId.ToString();
        btnSave.Text = Resources.LocalizedResources.Update;
        hidSave.Value = "Update";
    }

    ///// <summary>
    ///// This method is used to decrypt querystring.
    ///// </summary>
    ///// <returns></returns>
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
        hidRemarksCategoryId.Value = string.Empty;
        btnSave.Text = Resources.LocalizedResources.Save;
        hidSave.Value = "Save";
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
    /// This procedure is used to display remarks.
    /// </summary>
    private void DisplayRemarks()
    {
        lstvwRemarksCategory.DataSource = RemarksCategoryBL.GetConfig(miSchoolId, miAcademicYearId);
        lstvwRemarksCategory.DataBind();
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
    /// This method is used to set design according to selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidRemarkCategoryShouldNotBeDuplicated.Value = Resources.LocalizedResources.RemarkCategoryShouldNotBeDuplicated;
        hidSortOrderShouldNotBeDuplicated.Value = Resources.LocalizedResources.SortOrderShouldNotBeDuplicated;
        hidSortOrderShouldNotBeZero.Value = Resources.LocalizedResources.SortOrderShouldNotBeZero;
        hidAreYouSureYouWantToDeleteThisRemarkCategory.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisRemarkCategory;
        btnSave.Text= oResourceManager.GetString(hidSave.Value.Replace(" ", string.Empty));
    }
    #endregion private methods
}