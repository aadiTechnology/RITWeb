using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using SchoolEntities.Admin;
using Utility;
using System.Reflection;
using PhotoUploadEntities;
using CrystalDecisions.Shared;
using System.Threading;

public partial class GuestManagementUI : SchoolBase
{
    #region Constant(s)

    private const string S_DELETE_MESSAGE = "Guest details deleted successfully !!!";
    private const string S_UPDATE_MESSAGE = "Guest details updated successfully !!!";
    private const string S_SAVE_MESSAGE = "Guest details saved successfully !!!";
    private const string S_UPDATE_TEXT = "Update";
    private const string S_UPDATE_AND_PRINT_TEXT = "Update & Print";
    private const string S_SAVE_TEXT = "Save";
    private const string S_SAVE_AND_PRINT_TEXT = "Save & Print";
    private const string S_TIME_FORMAT = "hh:mm tt";

    #endregion

    #region DataMember

    private SchoolGuestDetailsBL moSchoolGuestDetailsBL;

    #endregion

    #region Event's

    /// <summary>
    /// This event is used to intialize page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSchoolGuestDetailsBL = new SchoolGuestDetailsBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                FillSalutationComboBox();
                SetJavascriptAttributes();
                FillGuestDetailsListView();
                FillCategoryTypeCombobox();
                               
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used save data into Database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iGuestId = SaveGuestDetails();

            if (btnSave.Text == S_SAVE_TEXT)
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);

            FillGuestDetailsListView();
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used clear all the fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetGuestDetailsControls();
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This event is used to bound the data to listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstSchoolGuestDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstSchoolGuestDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstSchoolGuestDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Set the data to row in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstSchoolGuestDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                SchoolGuestDetails oSchoolGuestDetails = e.Item.DataItem as SchoolGuestDetails;

                Label lblDate = e.Item.FindControl("lblDate") as Label;
                lblDate.Text = oSchoolGuestDetails.Date.ToString(Constants.S_DATE_FORMAT);

                Label lblOutTime = e.Item.FindControl("lblOutTime") as Label;
                if (oSchoolGuestDetails.OutTime == string.Empty || oSchoolGuestDetails.OutTime == null)
                    lblOutTime.Text = "-";
                else
                    lblOutTime.Text = oSchoolGuestDetails.OutTime;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Edit, Delete & Print the data in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstSchoolGuestDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iGuestId = Convert.ToInt32(lstSchoolGuestDetails.DataKeys[e.Item.DisplayIndex]["GuestId"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    FillSalutationComboBox();

                    SchoolGuestDetails oSchoolGuestDetails = moSchoolGuestDetailsBL.Get(iGuestId);

                    hidGuestId.Value = oSchoolGuestDetails.GuestId.ToString();

                    ddlCategoryType.SelectedValue = oSchoolGuestDetails.CategoryId.ToString();                    
                    cmbSalutation.SelectedValue = oSchoolGuestDetails.SalutaionId.ToString();
                    txtVisitorName.Text = oSchoolGuestDetails.GuestName;
                    txtVisitDate.Text = oSchoolGuestDetails.Date.ToString(Constants.S_DATE_FORMAT);
                    txtInTime.Text = oSchoolGuestDetails.InTime;
                    if (oSchoolGuestDetails.OutTime != null)
                        txtOutTime.Text = oSchoolGuestDetails.OutTime;
                    else
                        txtOutTime.Text = DateTime.Now.ToString(S_TIME_FORMAT);
                    txtMobileNo.Text = oSchoolGuestDetails.MobileNum;

                    if(oSchoolGuestDetails.AadharCardNo != null)
                        txtAadharNumber.Text = oSchoolGuestDetails.AadharCardNo;
                    if (oSchoolGuestDetails.PanCardNo != null)
                        txtPanNo.Text = oSchoolGuestDetails.PanCardNo;

                    txtPurpose.Text = oSchoolGuestDetails.PurposeOfVisit;
                    txtOrganisation.Text = oSchoolGuestDetails.OrganisationName;
                    txtName.Text = oSchoolGuestDetails.WhomToMeet;
                    txtDesignation.Text = oSchoolGuestDetails.Designation;
                    hidIsPhotoCaptured.Value = Constants.S_YES;

                    if (oSchoolGuestDetails.GuestPhoto != null)
                    {
                        imgPhoto.Visible = true;
                        string sQueryString = "Value=" + iGuestId + "&IsFromGuestScreen=" + Constants.S_ONE;
                        imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + sQueryString;
                    }

                    FillGuestDetailsListView();
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moSchoolGuestDetailsBL.Delete(iGuestId);

                    FillGuestDetailsListView();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                    if (Convert.ToInt32(hidGuestId.Value) == iGuestId)
                        ClearFields();
                }
                else if (e.CommandName == "EXPORT")
                {
                    PrintGuestDetails(iGuestId);
                }
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This event is used to edit the data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstSchoolGuestDetails_ItemEditing(object sender, ListViewEditEventArgs e)
    { }

    /// <summary>
    /// This event is used to Delete the data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstSchoolGuestDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    { }

    /// <summary>
    /// This event is used to Selected index changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstSchoolGuestDetails_SelectedIndexChanged(object sender, EventArgs e)
    { }

    /// <summary>
    /// This event is used to display listview record according to value in page combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstSchoolGuestDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used search Guest Name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearchGuest_Click(object sender, EventArgs e)
    {
        try
        {
            FillGuestDetailsListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change the value of Guest type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlGuestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillGuestDetailsListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    ///  This event is used to change the value of Category Type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlGuestCategoryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillGuestDetailsListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }  

    }
  
    /// <summary>
    /// This event is used for Auto search functionality.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SetDesignation();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for Export All guest Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportAll_Click(object sender, EventArgs e)
    {
        try
        {
            string sRecordSelectionFormula = "(usp_GetSchoolGuestDetailsForExport.SchoolId}=" + miSchoolId + "AND usp_GetSchoolGuestDetailsForExport.GuestTypeId}=" + ddlGuestType.SelectedValue.ToInt() + "AND usp_GetSchoolGuestDetailsForExport.UserName}=" + txtGuestName.Text + "AND usp_GetSchoolGuestDetailsForExport.CategoryId}=" + ddlGuestCategoryType.SelectedValue + ")" + "@ ";

            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.SchoolGuestDetailsForExport, sRecordSelectionFormula, ExportFormatType.Excel);
            oReportDisplay.DisplayReport();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This method is used for Populate all the details for Saving.
    /// </summary>
    public SchoolGuestDetails Pupulate()
    {
        SchoolGuestDetails aoSchoolGuestDetails = new SchoolGuestDetails();

        aoSchoolGuestDetails.GuestId = Convert.ToInt32(hidGuestId.Value);
        aoSchoolGuestDetails.CategoryId = ddlCategoryType.SelectedValue.ToInt();
        aoSchoolGuestDetails.SalutaionId = cmbSalutation.SelectedValue.ToInt();
        aoSchoolGuestDetails.GuestName = txtVisitorName.Text.TrimAll();
        aoSchoolGuestDetails.Date = Convert.ToDateTime(txtVisitDate.Text.TrimAll());
        aoSchoolGuestDetails.InTime = txtInTime.Text.TrimAll();
        aoSchoolGuestDetails.OutTime = txtOutTime.Text.TrimAll();
        aoSchoolGuestDetails.MobileNum = txtMobileNo.Text.TrimAll();

        if(txtAadharNumber.Text != string.Empty)
            aoSchoolGuestDetails.AadharCardNo = txtAadharNumber.Text.TrimAll();
        if(txtPanNo.Text != string.Empty)
            aoSchoolGuestDetails.PanCardNo = txtPanNo.Text.TrimAll();

        aoSchoolGuestDetails.PurposeOfVisit = txtPurpose.Text.TrimAll();
        aoSchoolGuestDetails.OrganisationName = txtOrganisation.Text.TrimAll();
        aoSchoolGuestDetails.WhomToMeet = txtName.Text.TrimAll();
        aoSchoolGuestDetails.Designation = txtDesignation.Text.TrimAll();

        if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES)
        {
            List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
            var oImage = lstImageData.Where(lst => lst.UserID == 0).LastOrDefault();

            aoSchoolGuestDetails.GuestPhoto = oImage.ImagesData;
        }
        else
            aoSchoolGuestDetails.GuestPhoto = null;

        return aoSchoolGuestDetails;
    }

    /// <summary>
    /// This method is used for Save all the details of guest.
    /// </summary>
    private int SaveGuestDetails()
    {
        int iGuestId = Constants.I_ZERO;
        SchoolGuestDetails oSchoolGuestDetails = Pupulate();
        moSchoolGuestDetailsBL.Save(oSchoolGuestDetails, out iGuestId);     

        return iGuestId;
    }
    /// <summary>
    /// This method is used for Reset the Controls 
    /// </summary>
    private void ResetGuestDetailsControls()
    {
        if (miSchoolId == Constants.SchoolId.MCPS.ToInt())
            ddlCategoryType.SelectedValue = "2";
        else
            ddlCategoryType.SelectedValue = "1";
    }

    /// <summary>
    /// This method is used for Filling the Guest Listview.
    /// </summary>
    private void FillGuestDetailsListView()
    {
        lstSchoolGuestDetails.DataSourceID = lstvwDSobj.ID;
    }
    /// <summary>
    /// This Method is used Fill Category Comboobx.
    /// </summary>
    private void FillCategoryTypeCombobox()
    {
        List<SchoolGuestDetails> lstCategory = moSchoolGuestDetailsBL.GetCategoryType();
        ListSource.FillDropDownList(lstCategory, ddlCategoryType, "CategoryName", "CategoryId", Constants.S_SELECT);
        ListSource.FillDropDownList(lstCategory, ddlGuestCategoryType, "CategoryName", "CategoryId", Constants.S_ALL);
        ResetGuestDetailsControls();
    }
   
   
    /// <summary>
    /// This Method is used to set Javascript Attributes..
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnSearch, btnExportAll });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        base.SetDefaultButton(btnSearch);
        string sQueryString = "UserId=" + hidGuestId.Value;
        ImgWebCam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
        txtVisitDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        txtInTime.Text = DateTime.Now.ToString(S_TIME_FORMAT);
    }

    /// <summary>
    /// This method is used for Clear All the controls.
    /// </summary>
    private void ClearFields()
    {
        hidGuestId.Value = Constants.S_ZERO;
        ddlCategoryType.ClearSelection();
        txtVisitorName.Text = string.Empty;
        txtVisitDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        txtInTime.Text = DateTime.Now.ToString(S_TIME_FORMAT);
        txtOutTime.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtAadharNumber.Text = string.Empty;
        txtPurpose.Text = string.Empty;
        txtOrganisation.Text = string.Empty;
        txtName.Text = string.Empty;
        txtDesignation.Text = string.Empty;
        Session.Remove(Constants.S_SESSION_USER_IMAGE_DATA);
        imgPhoto.Visible = false;
        btnSave.Text = S_SAVE_TEXT;
        ResetGuestDetailsControls();
    }

    /// <summary>
    /// This method is used for fill the Designation combobox.
    /// </summary>
    private void SetDesignation()
    {
        string asDesignation = string.Empty;

        moSchoolGuestDetailsBL.GetDesignationForGuestStaff(miSchoolId, txtName.Text, out asDesignation);
        txtDesignation.Text = asDesignation;
    }

    /// <summary>
    /// This method is used to print the guest details for Gate entry.
    /// </summary>
    private void PrintGuestDetails(int iGuestId)
    {
        string sRecordSelectionFormula = "(usp_GetSchoolGuestDetailsForGatePass.GuestId}=" + iGuestId + "AND usp_GetSchoolGuestDetailsForGatePass.SchoolId}=" + miSchoolId + ")" + "@ ";

        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.SchoolGuestDetails, sRecordSelectionFormula, ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method is used to fill salutation combo box.
    /// </summary>
    private void FillSalutationComboBox()
    {
        var oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
    }

    #endregion    
   
    
}