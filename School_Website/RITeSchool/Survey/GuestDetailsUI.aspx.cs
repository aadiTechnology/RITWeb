//// File Name  : GuestDetailsUI.aspx.cs
//// Created By : Sanket Bhujbal
//// Date       : 21/03/2016
//// Description :This class is used to maintain guest details functionality. 

using System;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using BusinessLogic.Survey;
using SchoolEntities.Survey;
using System.Collections.Generic;
using Utility;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Collections;

public partial class GuestDetailsUI : ExportDataTable
{

    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Guest Details saved successfully !!!";
    private const string S_UPDATE_MESSAGE = "Guest Details updated successfully !!!";
    private const string S_DELETE_MESSAGE = "Guest Details deleted successfully !!!";
    private const string S_SORT_ROW = "SortRow";

    #endregion

    #region DataMember(s)

    private GuestDetailsBL moGuestDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This is prerender event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            base.AddSortImage(lstvwGuestDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is page load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moGuestDetailsBL = new GuestDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                hidGuestId.Value = Constants.S_ZERO;
                chkSendSMS.Checked = true;
                SetJavascriptAttributes();
                InitializeControls();
                FillGuestDetailsListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is button save click event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            GuestDetails oGuestDetails = PopulateDetails();
            if (hidGuestId.Value == Constants.S_ZERO)
            {
                moGuestDetailsBL.Save(oGuestDetails);
                lblMessage.Visible = true;
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            }
            else
            {
                moGuestDetailsBL.Update(hidGuestId.Value.ToInt(), oGuestDetails);
                lblMessage.Visible = true;
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
            }
            ClearFields();
            hidSortDirection.Value = Constants.S_ASCENDING;
            FillGuestDetailsListView();
            btnSave.Text = "Save";            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is ItemDataBound event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGuestDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                bool bSentSMS = lstvwGuestDetails.DataKeys[e.Item.DisplayIndex]["IsSendSMS"].ToBool();
                var imgbtn = e.Item.FindControl("ImgBtn1") as ImageButton;
                imgbtn.Style.Add("Cursor","Default");
                imgbtn.Attributes.Add("onclick", "return false;");
                if (bSentSMS == true)
                    imgbtn.Visible = true;
                else
                    imgbtn.Visible = false;
                ImageButton btnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");                
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display listview record according to value in page combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwGuestDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is Item Command Event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGuestDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                chkSendSMS.Checked = false;
                int iGuestId = lstvwGuestDetails.DataKeys[e.Item.DisplayIndex]["GuestId"].ToInt();
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    FillControls(iGuestId);
                    hidGuestId.Value = iGuestId.ToString();
                    btnSave.Text = Resources.LocalizedResources.Update;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moGuestDetailsBL.Delete(iGuestId);                    
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                    ClearFields();
                    FillGuestDetailsListView();
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {
                base.RevertSortOrder(hidSortDirection);
                hidSortExpression.Value = e.CommandArgument.ToString();
                FillGuestDetailsListView();
            }
        }
        catch (SqlException se)
        {
            base.DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is cancel button click event which is used to clear the fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            hidSortDirection.Value = Constants.S_ASCENDING;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Export guest details in excel sheet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            int iCount = 0;
            List<GuestDetails> lstGuestDetails = moGuestDetailsBL.GetAll(miSchoolId, miAcademicYearId, hidSortExpression.Value, hidSortDirection.Value, 10000, 0);
            DataTable dtGuestDetails = new DataTable();
            dtGuestDetails.Columns.Add("Sr No", typeof(int));
            dtGuestDetails.Columns.Add("Name", typeof(string));
            dtGuestDetails.Columns.Add("Area", typeof(string));
            dtGuestDetails.Columns.Add("Mobile Number", typeof(string));
            dtGuestDetails.Columns.Add("Reference Name", typeof(string));
            dtGuestDetails.Columns.Add("IsSentSMS?", typeof(string));
            foreach (GuestDetails oGuestDetails in lstGuestDetails)
            {
                iCount++;
                dtGuestDetails.Rows.Add(iCount, oGuestDetails.FullName, oGuestDetails.Area, oGuestDetails.MobileNumber, oGuestDetails.ReferenceGuestFullName, (oGuestDetails.IsSendSMS == true) ? "Yes" : "No");
            }
            ExportToExcel("GuestDetails.xls", dtGuestDetails);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is data bound event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGuestDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwGuestDetails.Items.Count > Constants.I_ZERO)
            {
                ControlUtility.FillListViewPagerFooter(lstvwGuestDetails, DtPgCount);
            }
            else
            {
                DtPgCount.Visible = false;
            }
            InitializeControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to clear the fields.
    /// </summary>
    public void ClearFields()
    {
        hidGuestId.Value = Constants.S_ZERO;
        cmbSalutation.SelectedValue = Constants.S_ONE;
        txtFirstName.Text = string.Empty;
        txtMiddleName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtArea.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        chkIsReference.Checked = false;
        cmbReferenceGuestName.SelectedValue = Constants.S_ZERO;
        cmbReferenceGuestName.Enabled = false;
        //chkSendSMS.Checked = false;
        hidSortDirection.Value = string.Empty;
        chkSendSMS.Checked = true;
    }

    /// <summary>
    /// This method is used to fill the controls.
    /// </summary>
    /// <param name="aiGuestId"></param>
    private void FillControls(int aiGuestId)
    {
        GuestDetails oGuestDetails = moGuestDetailsBL.Get(aiGuestId);
        cmbSalutation.SelectedValue = oGuestDetails.SalutationId.ToString();
        txtFirstName.Text = oGuestDetails.FirstName;
        txtMiddleName.Text = oGuestDetails.MiddleName;
        txtLastName.Text = oGuestDetails.LastName;
        txtArea.Text = oGuestDetails.Area;
        txtMobileNo.Text = oGuestDetails.MobileNumber;
        if (oGuestDetails.ReferenceGuestId > 0 || oGuestDetails == null)
        {
            chkIsReference.Checked = true;
            cmbReferenceGuestName.SelectedValue = oGuestDetails.ReferenceGuestId.ToString();
        }
        else
        {
            chkIsReference.Checked = false;
        }
    }

    /// <summary>
    /// This method is used to fill guest details listview.
    /// </summary>
    private void FillGuestDetailsListView()
    {
        lstvwGuestDetails.DataSourceID = lstvwDSobj.ID;
    }

    /// <summary>
    /// This method is used to fetch the guest details.
    /// </summary>
    /// <returns></returns>
    private GuestDetails PopulateDetails()
    {
        GuestDetails oGuestDetails = new GuestDetails();
        oGuestDetails.SalutationId = cmbSalutation.SelectedValue.ToInt();
        oGuestDetails.FirstName = txtFirstName.Text.Trim();
        oGuestDetails.MiddleName = txtMiddleName.Text.Trim();
        oGuestDetails.LastName = txtLastName.Text.Trim();
        oGuestDetails.Area = txtArea.Text;
        oGuestDetails.MobileNumber = txtMobileNo.Text.Trim();
        if (chkIsReference.Enabled == true && chkIsReference.Checked == true)
            oGuestDetails.ReferenceGuestId = cmbReferenceGuestName.SelectedValue.ToInt();
        oGuestDetails.IsSendSMS = chkSendSMS.Checked;
        if (chkSendSMS.Checked)
        {
            SendSMS();
        }
        return oGuestDetails;
    }

    /// <summary>
    /// This method is used to send SMS to guest.
    /// </summary>
    public void SendSMS()
    {
        string sMobileNo = txtMobileNo.Text;
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        Hashtable oHTUsersMobileNo = new Hashtable();
        if (sMobileNo != string.Empty)
            oHTUsersMobileNo[sMobileNo] = sMobileNo;
        SMS oSMS = new SMS();
        oSMS.InsertedByID = -9999;
        oSMS.Sender = oSchoolBL.SMSSenderName;
        oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
        oSMS.SenderID = oSchoolBL.AdminId;
        oSMS.School_Name = oSchoolBL.SchoolName;
        string sURL = "https://www.facebook.com/jaywantpublicschool/";        
        oSMS.SMSText = "Dear Sir/Madam,\n" +
            "Welcome to Jaywant Public School, Sanaswadi. Please visit our facebook page " + sURL + " and like it.\n" +
               "Thank You - Jaywant Public School.";
        oSMS.AcademicYearID = miAcademicYearId;
        oSMS.SchoolID = miSchoolId;
        oSMS.DisplayText = cmbSalutation.SelectedItem + " " + txtFirstName.Text + " " + txtMiddleName.Text + " " + txtLastName.Text;
        oSMS.ToManualNumbers = oHTUsersMobileNo;
        oSMS.Send();
        oHTUsersMobileNo.Clear();
    }

    /// <summary>
    /// This method is used to initialize the controls.
    /// </summary>
    private void InitializeControls()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
        List<GuestReferenceDetails> lstGuestReferenceDetails = moGuestDetailsBL.GetReferenceGuestName();
        if (lstGuestReferenceDetails.Count == Constants.S_ZERO.ToInt())
        {
            chkIsReference.Enabled = false;
        }
        else
        {
            chkIsReference.Enabled = true;
        }
        ListSource.FillDropDownList(lstGuestReferenceDetails, cmbReferenceGuestName, "GuestFullName", "GuestId", Constants.S_SELECT);
        //hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set Javascript attributes
    /// </summary>
    private void SetJavascriptAttributes()
    {
        hidSortDirection.Value = Constants.S_ASCENDING;
        cmbSalutation.Focus();
        chkIsReference.Attributes.Add("onclick", "SetFieldStatus(this)");
        base.ApplyMouseHoverEffect(new List<Button> {btnSave, BtnCancel, btnExport});
    }

    #endregion

}