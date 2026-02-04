/* File Name :- AdmissionProcessDetailsUI.aspx.cs
 * Created Date :- 19-Oct-2015
 * Class Description :- This class is used to manage Admission Process Details. 
 * Created By :- 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities;
using System.Data;

public partial class AdmissionProcessDetailsUI : SchoolBase
{

    #region Constant(s)

    private const string S_DELETE_MESSAGE = "Admission process details deleted successfully !!!";
    private const string S_UPDATE_MESSAGE = "Admission process details updated successfully !!!";
    private const string S_SAVE_MESSAGE = "Admission process details saved successfully !!!";
    private const string S_STANDARD_MESSAGE = "Admission process details are already exist for selected standard.";
    private const string S_UPDATE_TEXT = "Update";
    private const string S_SAVE_TEXT = "Save";

    #endregion

    #region DataMember

    private AdmissionProcessDetailsBL moAdmissionProcessDetailsBL;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to set the page Load Events.
    /// </summary>

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moAdmissionProcessDetailsBL = new AdmissionProcessDetailsBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                FillAdmissionProcessListview();
                FillStanderdCombo();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Save Admission Process Details.
    /// </summary>

    protected void btnSave_Click(object sender, EventArgs e)
    {
           try
          {
           if (Page.IsValid)
           {
             AdmissionProcessDetails oAdmissionProcessDetails = Populate();
             bool bIsExist = moAdmissionProcessDetailsBL.IsConfigurationAlreadyExist(oAdmissionProcessDetails.AdmissionProcessId, oAdmissionProcessDetails.StanderdId);
            if (!bIsExist)
            {
                moAdmissionProcessDetailsBL.Save(oAdmissionProcessDetails);
                if (oAdmissionProcessDetails.AdmissionProcessId == 0)
                {
                    base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
                }
                else
                {
                    base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
                }

                ClearFields();
                FillAdmissionProcessListview();
            }
            else
            {
                base.DisplayMessage(S_STANDARD_MESSAGE, true, tdMessage);
            }
          }
       }
       catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the  listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void lstvwAdmissionProcessDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iAdmissionProcessId = Convert.ToInt32(lstvwAdmissionProcessDetails.DataKeys[e.Item.DisplayIndex]["AdmissionProcessId"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    AdmissionProcessDetails oAdmissionProcessDetails = moAdmissionProcessDetailsBL.Get(iAdmissionProcessId);
                    hidAdmissionId.Value = oAdmissionProcessDetails.AdmissionProcessId.ToString();
                    cmbStanderds.SelectedValue = oAdmissionProcessDetails.StanderdId.ToString();
                    if (oAdmissionProcessDetails.TotalForms != -1)
                        txtTotalForm.Text = oAdmissionProcessDetails.TotalForms.ToString();
                    else txtTotalForm.Text = string.Empty;
                    if (oAdmissionProcessDetails.TotalOnlineForms != -1)
                        txtTotalOnlineForm.Text = oAdmissionProcessDetails.TotalOnlineForms.ToString();
                    else txtTotalForm.Text = string.Empty;
                    if (oAdmissionProcessDetails.FormOpenDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    {
                        txtFormOpenDate.Text = oAdmissionProcessDetails.FormOpenDate.ToString(Constants.S_DATE_FORMAT);
                        txtStartTime.Text = oAdmissionProcessDetails.FormOpenDate.ToString("hh:mm tt");
                    }
                    else txtFormOpenDate.Text = string.Empty;
                    if (oAdmissionProcessDetails.FormCloseDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    {
                        txtFormCloseDate.Text = oAdmissionProcessDetails.FormCloseDate.ToString(Constants.S_DATE_FORMAT);
                        txtEndTime.Text = oAdmissionProcessDetails.FormCloseDate.ToString("hh:mm tt");
                    }
                    else txtFormCloseDate.Text = string.Empty;
                    if (oAdmissionProcessDetails.LottoryDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                        txtLotteryDate.Text = oAdmissionProcessDetails.LottoryDate.ToString(Constants.S_DATE_FORMAT);
                    else txtLotteryDate.Text = string.Empty;
                    if (oAdmissionProcessDetails.AdmissionConfirmLastDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                        txtAdmissionConfirmDate.Text = oAdmissionProcessDetails.AdmissionConfirmLastDate.ToString(Constants.S_DATE_FORMAT);
                    else txtAdmissionConfirmDate.Text = string.Empty;
                    chkIsLotteryConfirm.Checked = oAdmissionProcessDetails.IsLotteryConfirmed;
                    chkCanConfirmDirectly.Checked = oAdmissionProcessDetails.CanConfirmDirectly;
                    txtAmount.Text = oAdmissionProcessDetails.Amount.ToString();
                    if (oAdmissionProcessDetails.DOBMax.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                        txtDOBMax.Text = oAdmissionProcessDetails.DOBMax.ToString(Constants.S_DATE_FORMAT);
                    else txtDOBMax.Text = string.Empty;
                    if (oAdmissionProcessDetails.DOBMin.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                        txtDOBMin.Text = oAdmissionProcessDetails.DOBMin.ToString(Constants.S_DATE_FORMAT);
                    else txtDOBMin.Text = string.Empty;
                    chkEnableAdmissionFormFee.Checked = oAdmissionProcessDetails.EnableAdmissionFormFee;
                    chkIsInternalAdmission.Checked = oAdmissionProcessDetails.IsInternalAdmission;
                    chkEnableWaitingList.Checked = oAdmissionProcessDetails.EnableWaitingList;
                    txtWaitingListURL.Text = oAdmissionProcessDetails.WaitingListURL;
                    // Bind new fields to UI
                    chkEnableInternalLink.Checked = oAdmissionProcessDetails.EnableInternalLink;
                    txtExternalSiteMessage.Text = oAdmissionProcessDetails.ExternalSiteMessage;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moAdmissionProcessDetailsBL.Delete(iAdmissionProcessId);
                    FillAdmissionProcessListview();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                    if (Convert.ToInt32(hidAdmissionId.Value) == iAdmissionProcessId)
                    {
                        ClearFields();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

     /// <summary>
    /// This event is used to Bound Data in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void lstvwAdmissionProcessDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                AdmissionProcessDetails oAdmissionProcessDetails = e.Item.DataItem as AdmissionProcessDetails;
                Label lblFormOpenDate = e.Item.FindControl("lblFormOpenDate") as Label;
                if (oAdmissionProcessDetails.FormOpenDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblFormOpenDate.Text = oAdmissionProcessDetails.FormOpenDate.ToString(Constants.S_DATE_FORMAT);
                else lblFormOpenDate.Text = "-";

                Label lblFormCloseDate = e.Item.FindControl("lblFormCloseDate") as Label;
                if (oAdmissionProcessDetails.FormCloseDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblFormCloseDate.Text = oAdmissionProcessDetails.FormCloseDate.ToString(Constants.S_DATE_FORMAT);
                else lblFormCloseDate.Text = "-";

                Label lblDOBMax = e.Item.FindControl("lblDOBMax") as Label;
                if (oAdmissionProcessDetails.DOBMax.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblDOBMax.Text = oAdmissionProcessDetails.DOBMax.ToString(Constants.S_DATE_FORMAT);
                else lblDOBMax.Text = "-";

                Label lblDOBMin = e.Item.FindControl("lblDOBMin") as Label;
                if (oAdmissionProcessDetails.DOBMin.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblDOBMin.Text = oAdmissionProcessDetails.DOBMin.ToString(Constants.S_DATE_FORMAT);
                else lblDOBMin.Text = "-";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to Deleting Item From the listview.
    /// </summary>

    protected void lstvwAdmissionProcessDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    { }

    /// <summary>
    /// This event is used to Editing the Items From listview.
    /// </summary>

    protected void lstvwAdmissionProcessDetails_ItemEditing(object sender, ListViewEditEventArgs e)
    { }

    /// <summary>
    /// This event is used to select SelectedIndexChanged in listview.
    /// </summary>

    protected void lstvwAdmissionProcessDetails_SelectedIndexChanged(object sender, EventArgs e)
    { }

    /// <summary>
    /// This event is used to call ResetFields Method.
    /// </summary>

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

    #endregion

    #region Method(s)

    /// <summary>
    /// This Method is used to Populate the Save Details..
    /// </summary>

    public AdmissionProcessDetails Populate()
    
    {
        AdmissionProcessDetails oAdmissionProcessDetails = new AdmissionProcessDetails();
        oAdmissionProcessDetails.StanderdId = cmbStanderds.SelectedValue.ToInt();
        oAdmissionProcessDetails.AdmissionProcessId = Convert.ToInt32(hidAdmissionId.Value);
        if (txtTotalForm.Text.Trim() != string.Empty)
            oAdmissionProcessDetails.TotalForms = Convert.ToInt32(txtTotalForm.Text);
        else
            oAdmissionProcessDetails.TotalForms = -1;
        if (txtTotalOnlineForm.Text.Trim() != String.Empty)
            oAdmissionProcessDetails.TotalOnlineForms = Convert.ToInt32(txtTotalOnlineForm.Text);
        else
            oAdmissionProcessDetails.TotalOnlineForms = -1;
        if (txtFormOpenDate.Text.Trim() != String.Empty)
            oAdmissionProcessDetails.FormOpenDate = Convert.ToDateTime(txtFormOpenDate.Text.ToString() + ' ' + txtStartTime.Text.ToString()) ;
        if (txtFormCloseDate.Text.Trim() != String.Empty)
            oAdmissionProcessDetails.FormCloseDate = Convert.ToDateTime(txtFormCloseDate.Text.ToString()+ ' '+ txtEndTime.Text.ToString());
        if (txtLotteryDate.Text.Trim() != String.Empty)
            oAdmissionProcessDetails.LottoryDate = Convert.ToDateTime(txtLotteryDate.Text);
        if (txtAdmissionConfirmDate.Text.Trim() != String.Empty)
            oAdmissionProcessDetails.AdmissionConfirmLastDate = Convert.ToDateTime(txtAdmissionConfirmDate.Text);
        oAdmissionProcessDetails.IsLotteryConfirmed = chkIsLotteryConfirm.Checked;
        oAdmissionProcessDetails.CanConfirmDirectly = chkCanConfirmDirectly.Checked;
        if (txtAmount.Text.Trim() != String.Empty)
            oAdmissionProcessDetails.Amount = Convert.ToInt32(txtAmount.Text.Trim());
        else
            oAdmissionProcessDetails.Amount = 0;
        if (txtDOBMax.Text.Trim() != String.Empty)
            oAdmissionProcessDetails.DOBMax = Convert.ToDateTime(txtDOBMax.Text);
        if (txtDOBMin.Text.Trim() != String.Empty)
            oAdmissionProcessDetails.DOBMin = Convert.ToDateTime(txtDOBMin.Text);
        oAdmissionProcessDetails.EnableAdmissionFormFee = chkEnableAdmissionFormFee.Checked;
        oAdmissionProcessDetails.IsInternalAdmission = chkIsInternalAdmission.Checked;
        oAdmissionProcessDetails.EnableWaitingList = chkEnableWaitingList.Checked;
        oAdmissionProcessDetails.WaitingListURL = txtWaitingListURL.Text.Trim();
        // Map new fields from UI to entity
        oAdmissionProcessDetails.EnableInternalLink = chkEnableInternalLink.Checked;
        oAdmissionProcessDetails.ExternalSiteMessage = txtExternalSiteMessage.Text.Trim();
        return (oAdmissionProcessDetails);
    }

    /// <summary>
    /// This Method is used to set Javascript Attributes..
    /// </summary>

    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnBack.PostBackUrl = "ScreensUI.aspx";
        btnSave.Attributes.Add("Onclick", "ClearMessage()");
    }

    /// <summary>
    /// This Method is used Fill Standard DropDown List.
    /// </summary>

    private void FillStanderdCombo()
    {
        var oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        cmbStanderds.Bind(oDtStandardCollection, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT);
    }

    /// <summary>
    /// This Method is used Fill Admission Process Details ListView.
    /// </summary>

    private void FillAdmissionProcessListview()
    {
        List<AdmissionProcessDetails> olstAdmissionProcessDetails = moAdmissionProcessDetailsBL.GetAll();
        lstvwAdmissionProcessDetails.DataSource = olstAdmissionProcessDetails;
        lstvwAdmissionProcessDetails.DataBind();
    }

    /// <summary>
    /// This Method is used to Reset All Fields.
    /// </summary>

    private void ClearFields()
    {
        hidAdmissionId.Value = Constants.S_ZERO;
        cmbStanderds.ClearSelection();
        txtTotalForm.Text = string.Empty;
        txtTotalOnlineForm.Text = string.Empty;
        txtFormOpenDate.Text = string.Empty;
        txtStartTime.Text = string.Empty;
        txtFormCloseDate.Text = string.Empty;
        txtEndTime.Text = string.Empty;
        txtLotteryDate.Text = string.Empty;
        txtAdmissionConfirmDate.Text = string.Empty;
        chkIsLotteryConfirm.Checked = false;
        chkCanConfirmDirectly.Checked = false;
        txtAmount.Text = string.Empty;
        txtDOBMax.Text = string.Empty;
        txtDOBMin.Text = string.Empty;
        chkEnableAdmissionFormFee.Checked = false;
        chkIsInternalAdmission.Checked = false;
        chkEnableWaitingList.Checked = false;
        txtWaitingListURL.Text = string.Empty;
        // Clear new fields
        chkEnableInternalLink.Checked = false;
        txtExternalSiteMessage.Text = string.Empty;
        btnSave.Text = S_SAVE_TEXT;
    }

    #endregion
}