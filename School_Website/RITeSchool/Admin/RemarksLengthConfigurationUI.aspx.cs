/*  This Class is used to   
 *  - UI and functional validation of student's Remark Length information.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;
using MasterEntities;
using Utility;

public partial class RemarksLengthConfigurationUI : SchoolBase
{    

    #region Data Member(s)
        
    private RemarksConfigurationBL moRemarksConfigurationBL = null;    

    #endregion

    #region --Events(s)--
    /// <summary>
    /// This method is used to load data in firsttime.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moRemarksConfigurationBL = new RemarksConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                RefreshValue();
                SetJavaScriptAttributes();
                FillStandardCombo();
                FillTermCombo();
                FillRemarkLengthConfigListview();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                RefreshValue();               
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
    protected void lstvwRemarkLengthConfiguration_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                hidConfigId.Value = (oCurrentItem.FindControl("HidStandardwiseRemarkLengthId") as HiddenField).Value;
                switch (e.CommandName)
                {
                    case Constants.S_COMMAND_UPDATE:

                        StandardwiseRemarkLength oStandardwiseRemarkLength = moRemarksConfigurationBL.GetRemarkConfiguration(hidConfigId.Value.ToInt());
                        {
                            txtRemarkLength.Text = Convert.ToString(oStandardwiseRemarkLength.MaxRemarkLength);
                            cmbTerm.SelectedValue = Convert.ToString(oStandardwiseRemarkLength.TermId);
                            cmbStandard.SelectedValue = Convert.ToString(oStandardwiseRemarkLength.StandardId);
                        }

                        int iMaxStudLength = moRemarksConfigurationBL.GetMaxRemarkLength(Convert.ToInt32(cmbStandard.SelectedValue), Convert.ToInt32(cmbTerm.SelectedValue));
                        hidMaxRemarkLength.Value = iMaxStudLength.ToString();
                        btnSave.Text = Resources.LocalizedResources.Update;
                        break;

                    case Constants.S_COMMAND_REMOVE:
                        moRemarksConfigurationBL.DeleteProgressRemarkLength(hidConfigId.Value.ToInt());
                        hidConfigId.Value = Constants.S_ZERO;
                        FillRemarkLengthConfigListview();
                        SetMessage(Resources.LocalizedResources.Deleted, false);
                        ClearFields();
                        break;
                }
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
            if (Page.IsValid)
            {
                StandardwiseRemarkLength oStandardwiseRemarkLength = new StandardwiseRemarkLength
                {
                    MaxRemarkLength = Convert.ToInt32(txtRemarkLength.Text),
                    TermId = cmbTerm.SelectedValue.ToInt(),
                    StandardId = cmbStandard.SelectedValue.ToInt(),
                    StandardwiseRemarkLengthId = hidConfigId.Value.ToInt()
                };

                moRemarksConfigurationBL.InsertRemarkLengthDetails(oStandardwiseRemarkLength);
                if (hidConfigId.Value.ToInt() == Constants.I_ZERO)
                    SetMessage(Resources.LocalizedResources.Added, false);

                else if (hidConfigId.Value.ToInt() > Constants.I_ZERO)
                    SetMessage(Resources.LocalizedResources.Updated, false);

                ClearFields();
                FillRemarkLengthConfigListview();

                if (QueryString["Is_Configured"] != Constants.S_YES)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ProgressRemarksLengthConfiguration));
            }
            else
                lblUpdateMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set delete confirmation message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRemarkLengthConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                imgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
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
            lblErrorMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion --Events(s)--

    #region --Method(s)--


    /// <summary>
    /// This method is called to fill Term in combo.
    /// </summary>
    private void FillTermCombo()
    {
        DataTable oDataTable = StudentwiseRemarkMasterBL.GetTestwiseTerm(miSchoolId);
        ControlUtility.FillDropDownList(oDataTable, ref cmbTerm, "Term_Id", "Term_Name", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill standard Names.
    /// </summary>
    private void FillStandardCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        List<StandardMaster> olstStandards = oStandardCollectionBL.GetExamConfiguredStandards();
        ListSource.FillDropDownList(olstStandards, cmbStandard, "StandardName", "StandardId", Constants.S_SELECT);
    }
    /// <summary>
    /// This method is called to set appropriate messages.
    /// </summary>
    /// <param name="asMessage"></param>
    /// <param name="abIsErrorMessage"></param>
    private void SetMessage(string asOperation, bool abIsErrorMessage)
    {
        lblErrorMsg.Text = string.Empty;       
        lblUpdateMessage.Text = Resources.LocalizedResources.SaveRemarkLength.Replace("%OPERATION%", asOperation.ToLower());
        lblUpdateMessage.Font.Bold = true;

        if (abIsErrorMessage)
            lblUpdateMessage.ForeColor = Color.Red;
        else
            lblUpdateMessage.ForeColor = Color.Blue;
    }

    /// <summary>
    /// This method is called to set the javascript attributes on page load.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related));
        cmbStandard.Focus();       
        btnSave.Attributes.Add("onclick", "if(!ValidRemarkLength())return false;");
    }

    /// <summary>
    /// This method is called to clear the fields.
    /// </summary>
    private void ClearFields()
    {
        btnSave.Text = Resources.LocalizedResources.Save;
        hidConfigId.Value = Constants.S_ZERO;
        cmbTerm.ClearSelection();
        txtRemarkLength.Text = string.Empty;
        cmbStandard.ClearSelection();
    }

    /// <summary>
    /// This method is called to fill the listview.
    /// </summary>
    private void FillRemarkLengthConfigListview()
    {
        lstvwRemarkLengthConfiguration.DataSource = moRemarksConfigurationBL.GetAllStandardwiseRemarkLengths();
        lstvwRemarkLengthConfiguration.DataBind();
        if (lstvwRemarkLengthConfiguration.Items.Count == 0)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ProgressRemarksLengthConfiguration));
    }
        
    /// <summary>
    /// This method is used to fill hidden variable information
    /// </summary>
    private void RefreshValue()
    {
        hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
        hidValBlankTimeSpan.Value = Resources.LocalizedResources.ValBlankTimeSpan;
        hidAlertDeleteUser.Value = Resources.LocalizedResources.AlertDeleteProgressRemark;
        hidValTimeSpan.Value = Resources.LocalizedResources.ValTimeSpan;
        hidProgressRemarkLengthAlertMessage.Value = Resources.LocalizedResources.ProgressRemarkAlert;
        hidRemarkExistErrorMessage.Value = Resources.LocalizedResources.S_REMARK_EXISTS;
        btnSave.Text = Resources.LocalizedResources.Save;
        hidRemarkLength.Value = Resources.LocalizedResources.RemarkLengthZero;
        hidRemarkLengthCondition.Value = Resources.LocalizedResources.RemarkLengthCondition;
    }

    #endregion --Method(s)--
}